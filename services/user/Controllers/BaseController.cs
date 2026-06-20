using Microsoft.AspNetCore.Mvc;
using user.Data;
using user.Services;

namespace user.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly AppDbContext _db;
        protected readonly JwtService _jwtService;

        protected BaseController(AppDbContext db,JwtService jwtService)
        {
            _db = db;
            _jwtService = jwtService;
        }
    }
}