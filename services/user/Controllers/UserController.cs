using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using user.Data;
using user.DTOs;
using user.Extensions;
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
        [HttpPost("AddDetails")]
        public IActionResult AddDetails(DetailsRequestDto request)
        {
            var personGuid = User.GetPersonGuid();

            if (personGuid is not Guid guid)
                return Unauthorized();

            var person = _db.Persons
                .Include(x => x.Role)
                .FirstOrDefault(x => x.PersonGuid == guid);

            if (person == null)
                return NotFound("User not found");

            if(person.Role == null)
                return Unauthorized();

            var currentDetails = _db.Person_Details
                .Where(d => d.PersonId == person.Id)
                .OrderByDescending(d => d.ModifiedAt)
                .FirstOrDefault();

            if (currentDetails == null)
                return NotFound("User not found");

            var details = new PersonDetails
            {
                PersonId = person.Id,
                Firstname = request.Firstname,
                Middlename = request.Middlename,
                Lastname = request.Lastname,
                PhoneNumber = request.PhoneNumber,
                Email = currentDetails.Email, // ail comes from so you will could change it
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
