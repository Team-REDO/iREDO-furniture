using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using user.Data;
using user.DTOs;
using user.Extensions;
using user.Services;
using UserService.DomainModels;


namespace user.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : BaseController
    {
        public AuthController(AppDbContext db, JwtService jwtService): base(db,jwtService) { }

        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = "/api/auth/google-response"
            }, "Google");
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {

            var result = await HttpContext.AuthenticateAsync("Google");

            if (!result.Succeeded)
                return BadRequest("Google authentication failed");

            var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var firstName = result.Principal.FindFirst(ClaimTypes.GivenName)?.Value;
            var lastName = result.Principal.FindFirst(ClaimTypes.Surname)?.Value;
            var fullName = result.Principal.FindFirst(ClaimTypes.Name)?.Value;

            if (email == null)
                return BadRequest("Email not returned from Google");

            // 🔍 find existing person
            var details = _db.Person_Details
                .Include(x => x.Person)
                .ThenInclude(x => x.Role)
                .Where(x => x.Email == email)
                .OrderByDescending(x => x.ModifiedAt)
                .FirstOrDefault();


            if (details != null && _db.IsRemoved(details.PersonId))
            {
                details = null;
            }


            if (details == null)
            {
                try
                {
                    var defaultRole = _db.Roles
                        .First(r => r.Id == 1); //.First(r => r.Name == "User");

                    var person = new Person
                    {
                        PersonGuid = Guid.NewGuid(),
                        RoleId = defaultRole.Id,
                        CreatedAt = DateTime.UtcNow,
                        Details = new List<PersonDetails>
                        {
                            new PersonDetails
                            {
                                Email = email,
                                Firstname = firstName ?? "",
                                Lastname = lastName ?? ""
                            }

                        }
                    };

                    _db.Persons.Add(person);
                    _db.SaveChanges();

                    details = _db.Person_Details
                        .Include(x => x.Person)
                        .ThenInclude(x => x.Role)
                        .Where(x => x.Email == email)
                        .OrderByDescending(x => x.ModifiedAt)
                        .FirstOrDefault();
                }
                catch (Exception ex)
                {
                    // fallback in case of race condition / duplicate
                    details = _db.Person_Details
                        .Include(x => x.Person)
                        .ThenInclude(x => x.Role)
                        .Where(x => x.Email == email)
                        .OrderByDescending(x => x.ModifiedAt)
                        .FirstOrDefault();
                    Console.WriteLine(ex);
                }
            }

            var userPerson = details.Person;
            //  issue JWT
            var token = _jwtService.GenerateJwt(
                details.Email,
                userPerson.Role.Name,
                userPerson.PersonGuid
            );
            //var token = _jwtService.GenerateJwt(details.Email);
            Response.Cookies.Append("token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // true when using HTTPS
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            return Redirect("/catalogue");
            //return Ok(new { token });
        }

        

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // Remove JWT
            Response.Cookies.Delete("token");

            // Remove Google/Cookie auth session
            await HttpContext.SignOutAsync("Cookies");

            return Ok(new
            {
                message = "Logged out"
            });
        }

        //POST /api/auth/refresh



        // ---------------------------------------------------------------------------------------
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var personGuid = User.GetPersonGuid();

            if (personGuid == null)
                return Unauthorized();

            var person = _db.Persons
                .FirstOrDefault(x => x.PersonGuid == personGuid.Value);

            if (person == null)
                return NotFound("User not found");

            if (_db.IsRemoved(person.Id))
                return Unauthorized("User has been removed"); // is user remove

            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (email == null || role == null)
                return Unauthorized();

            var details = _db.Person_Details
                .Where(x => x.PersonId == person.Id)
                .OrderByDescending(x => x.ModifiedAt)
                .FirstOrDefault();

            return Ok(new MeResponseDto
            {
                Email = email,
                Role = role,
                PersonGuid = personGuid.Value,
                Firstname = details?.Firstname,
                Middlename = details?.Middlename,
                Lastname = details?.Lastname,
                PhoneNumber = details?.PhoneNumber
            });
        }

        [Authorize]
        [HttpDelete("delete-user")]
        public IActionResult DeleteUser()
        {
            var personGuid = User.GetPersonGuid();

            if (personGuid == null)
                return Unauthorized();

            var person = _db.Persons
                .FirstOrDefault(x => x.PersonGuid == personGuid.Value);

            if (person == null)
                return NotFound("User not found");

            var alreadyRemoved = _db.Person_Removed
                .Any(x => x.PersonId == person.Id);

            if (alreadyRemoved)
                return Conflict("User already removed");

            var personRemoved = new PersonRemoved
            {
                PersonId = person.Id
            };

                _db.Person_Removed.Add(personRemoved);
                _db.SaveChanges();

            return Ok(new
            {
                Message = "User marked as removed",
                RemovedAt = personRemoved.RemovedAt
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-test")]
        public IActionResult AdminTest()
        {
            return Ok("Admin only");
        }
    }

}
