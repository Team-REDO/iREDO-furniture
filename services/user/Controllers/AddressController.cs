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
    [Route("api/address")]
    public class AddressController : BaseController
    {
        public AddressController(AppDbContext db, JwtService jwtService) : base(db, jwtService) { }

        [Authorize]
        [HttpGet("GetAddress")]
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
