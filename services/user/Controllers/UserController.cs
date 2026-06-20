using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using user.Data;
using user.DTOs;
using user.Services;
using UserService.DomainModels;

namespace user.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : BaseController
    {
        public UserController(AppDbContext db, JwtService jwtService) : base(db, jwtService) { }

        [Authorize]
        [HttpPost("details")]
        public IActionResult UpdateDetails(DetailsRequestDto request)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (email == null)
                return Unauthorized();

            var currentDetails = _db.Person_Details
                .Where(d => d.Email == email)
                .OrderByDescending(d => d.ModifiedAt)
                .FirstOrDefault();

            if (currentDetails == null)
                return NotFound("User not found");

            var details = new PersonDetails
            {
                PersonId = currentDetails.PersonId,
                Firstname = request.Firstname,
                Middlename = request.Middlename,
                Lastname = request.Lastname,
                PhoneNumber = request.PhoneNumber,
                Email = currentDetails.Email,
                ModifiedAt = DateTime.UtcNow
            };

            _db.Person_Details.Add(details);
            _db.SaveChanges();

            return Ok(new DetailsResponseDto
            {
                Firstname = details.Firstname,
                Middlename = details.Middlename,
                Lastname = details.Lastname,
                PhoneNumber = details.PhoneNumber,
                Email = details.Email
            });
        }


        //DELETE /api/user

        //Later:
        //GET /api/user/details/history
    }
}
