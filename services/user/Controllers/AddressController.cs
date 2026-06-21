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
                .FirstOrDefault(x => x.PersonGuid == guid);

            if (person == null)
                return NotFound("User not found");

            if (_db.IsRemoved(person.Id))
                return Unauthorized("User has been removed"); // is user remove

            var address = _db.Address
                .Where(x => x.PersonId == person.Id)
                .OrderByDescending(x => x.ModifiedAt)
                .FirstOrDefault();

            if (address == null)
                return NotFound("Address not found");

            return Ok(new AddressResponseDto
            {
                Street = address.Street,
                StreetNumber = address.StreetNumber,
                FloorDoor = address.FloorDoor,
                ZipCode = address.ZipCode,
                City = address.City,
                Country = address.Country
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddAddress(AddressRequestDto request)
        {
            var personGuid = User.GetPersonGuid();

            if (personGuid is not Guid guid)
                return Unauthorized();

            var person = _db.Persons
                .FirstOrDefault(x => x.PersonGuid == guid);

            if (person == null)
                return NotFound("User not found");
            
            if (_db.IsRemoved(person.Id))
                return Unauthorized("User has been removed"); // is user remove

            var addressEntity = new Address
            {
                PersonId = person.Id,
                Street = request.Street,
                StreetNumber = request.StreetNumber,
                FloorDoor = request.FloorDoor,
                ZipCode = request.ZipCode,
                City = request.City,
                Country = request.Country
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
