using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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
                .FirstOrDefault(p => p.Email == email);

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

                    details = person.Details.First();
                }
                catch (Exception ex)
                {
                    // fallback in case of race condition / duplicate
                    details = _db.Person_Details
                        .SingleOrDefault(p => p.Email == email);
                }
            }

            // 🎟️ issue JWT
            var token = _jwtService.GenerateJwt(details.Email);

            Response.Cookies.Append("token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // true when using HTTPS
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            return Redirect("/catalogue");
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
        [HttpGet("me")]
        public IActionResult Me()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            return Ok(new { email });
        }

        [Authorize]
        [HttpPost("add-address")]
        public IActionResult AddAddress(AddressRequestDto request)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (email == null)
                return Unauthorized();

            var details = _db.Person_Details
                .FirstOrDefault(p => p.Email == email);

            if (details == null)
                return NotFound("User not found");

            // Find existing address
            var addressEntity = _db.Address
                .FirstOrDefault(a => a.PersonId == details.PersonId);

            // Create if missing
            if (addressEntity == null)
            {
                addressEntity = new Address
                {
                    PersonId = details.PersonId

                };

                _db.Address.Add(addressEntity);
            }

            // Update values
            addressEntity.Street = request.Street;
            addressEntity.StreetNumber = request.StreetNumber;
            addressEntity.FloorDoor = request.FloorDoor;
            addressEntity.ZipCode = request.ZipCode;
            addressEntity.City = request.City;
            addressEntity.Country = request.Country;
            addressEntity.ModifiedAt = DateTime.UtcNow;

            _db.SaveChanges();

            var response = new AddressResponseDto
            {
                Street = addressEntity.Street,
                StreetNumber = addressEntity.StreetNumber,
                FloorDoor = addressEntity.FloorDoor,
                ZipCode = addressEntity.ZipCode,
                City = addressEntity.City,
                Country = addressEntity.Country
            };

            return Ok(response);
        }
    }

}
