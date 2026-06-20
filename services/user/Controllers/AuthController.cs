using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using user.Data;
using user.DTOs;
using user.Services;
using UserService.DomainModels;
namespace user.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext db, JwtService jwtService)
        {
            _db = db;
            _jwtService = jwtService;
        }


        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            Console.WriteLine($"PATH: {Request.Path}");
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
                .FirstOrDefault(x => x.Email == email);

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
                        Details = new List<PersonDetails>
                        {
                            new PersonDetails
                            {
                                Email = email,
                                Firstname = firstName ?? "",
                                Lastname = lastName ?? "",
                                ModifiedAt = DateTime.UtcNow
                            }
                        }
                    };

                    _db.Persons.Add(person);
                    _db.SaveChanges();

                    details = _db.Person_Details
                        .Include(x => x.Person)
                        .ThenInclude(x => x.Role)
                        .Single(x => x.Email == email);
                }
                catch (Exception ex)
                {
                    // fallback in case of race condition / duplicate
                    details = _db.Person_Details
                        .Include(x => x.Person)
                        .ThenInclude(x => x.Role)
                        .Single(x => x.Email == email);
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

        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = "/api/auth/google-response"
            }, "Google");
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





        // ---------------------------------------------------------------------------------------
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var personGuidClaim = User.FindFirst("personGuid")?.Value;

            if (!Guid.TryParse(personGuidClaim, out var personGuid))
                return Unauthorized();

            if (email == null || role == null)
                return Unauthorized();

            var details = _db.Person_Details
                .Include(x => x.Person)
                .Where(x => x.Person.PersonGuid == personGuid)
                .OrderByDescending(x => x.ModifiedAt)
                .FirstOrDefault();

            return Ok(new MeResponseDto
            {
                Email = email,
                Role = role,
                PersonGuid = personGuid,
                Firstname = details?.Firstname,
                Middlename = details?.Middlename,
                Lastname = details?.Lastname,
                PhoneNumber = details?.PhoneNumber
            });
        }

        [Authorize]
        [HttpGet("address")]
        public IActionResult GetAddress()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (email == null)
                return Unauthorized();

            var details = _db.Person_Details
                .Where(x => x.Email == email)
                .OrderByDescending(x => x.ModifiedAt)
                .FirstOrDefault();

            if (details == null)
                return NotFound();

            var address = _db.Address
                .Where(x => x.PersonId == details.PersonId)
                .OrderByDescending(x => x.ModifiedAt)
                .FirstOrDefault();

            return Ok(address);
        }

        [Authorize]
        [HttpPost("add-address")]
        public IActionResult AddAddress(AddressRequestDto request)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (email == null)
                return Unauthorized();

            var details = _db.Person_Details
                .Where(x => x.Email == email)
                .OrderByDescending(x => x.ModifiedAt)
                .FirstOrDefault(); ;

            if (details == null)
                return NotFound("User not found");

            var addressEntity = new Address
            {
                PersonId = details.PersonId,
                Street = request.Street,
                StreetNumber = request.StreetNumber,
                FloorDoor = request.FloorDoor,
                ZipCode = request.ZipCode,
                City = request.City,
                Country = request.Country,
                ModifiedAt = DateTime.UtcNow
            };

            _db.Address.Add(addressEntity);
            _db.SaveChanges();

            return Ok(new AddressResponseDto
            {
                Street = addressEntity.Street,
                StreetNumber = addressEntity.StreetNumber,
                FloorDoor = addressEntity.FloorDoor,
                ZipCode = addressEntity.ZipCode,
                City = addressEntity.City,
                Country = addressEntity.Country
            });
        }


    }

}
