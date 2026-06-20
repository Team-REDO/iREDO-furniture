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
    [Route("api/address")]
    public class AddressController : BaseController
    {
        public AddressController(AppDbContext db, JwtService jwtService) : base(db, jwtService) { }

        [Authorize]
        [HttpGet]
        public IActionResult GetAddress()
        {
            var personGuid = User.GetPersonGuid();

            if (personGuid is not Guid guid)
                return Unauthorized();

            var person = _db.Persons
                .Include(x => x.Role)
                .FirstOrDefault(x => x.PersonGuid == guid);

            if (person == null)
                return NotFound("User not found");
            
            if (person.Role == null)
                return Unauthorized();

            var address = _db.Address
                .Where(x => x.PersonId == person.Id)
                .OrderByDescending(x => x.ModifiedAt)
                .FirstOrDefault();

            return Ok(address);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddAddress(AddressRequestDto request)
        {
            var personGuid = User.GetPersonGuid();

            if (personGuid is not Guid guid)
                return Unauthorized();

            var person = _db.Persons
                .Include(x => x.Role)
                .FirstOrDefault(x => x.PersonGuid == guid);

            if (person == null)
                return NotFound("User not found");

            if (person.Role == null)
                return Unauthorized();

            var addressEntity = new Address
            {
                PersonId = person.Id,
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
