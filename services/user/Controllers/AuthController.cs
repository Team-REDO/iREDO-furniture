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
            var details = _db.PersonDetails
                .FirstOrDefault(p => p.Email == email);

            if (details == null)
            {
                try
                {
                    var person = new Person
                    {
                        Guid = Guid.NewGuid(),
                        Details = new PersonDetails
                        {
                            Email = email,
                            Firstname = firstName ?? "",
                            Lastname = lastName ?? "",
                            ModifiedDate = DateTime.UtcNow
                        }
                    };

                    _db.Persons.Add(person);
                    _db.SaveChanges();

                    details = person.Details;
                }
                catch (Exception ex)
                {
                    // fallback in case of race condition / duplicate
                    details = _db.PersonDetails
                        .SingleOrDefault(p => p.Email == email);
                }
            }

            // 🎟️ issue JWT
            var token = _jwtService.GenerateJwt(details.Email);

            return Ok(new { token });
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

            var details = _db.PersonDetails
                .FirstOrDefault(p => p.Email == email);

            if (details == null)
                return NotFound("User not found");

            // 🔹 Create DB entity
            var addressEntity = new Address
            {
                PersonDetailsId = details.Id,
                Street = request.Street,
                StreetNumber = request.StreetNumber,
                FloorDoor = request.FloorDoor,
                ZipCode = request.ZipCode,
                City = request.City,
                Country = request.Country,
                ModifiedDate = DateTime.UtcNow
            };

            _db.Addresses.Add(addressEntity);
            _db.SaveChanges();

            // 🔹 Create response DTO
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
