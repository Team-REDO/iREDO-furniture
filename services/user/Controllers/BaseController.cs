using Microsoft.AspNetCore.Mvc;
using user.Data;
using user.Messaging.Publishers;
using user.Services;

namespace user.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly AppDbContext _db;
        protected readonly JwtService _jwtService;
        protected readonly RabbitMqService _rabbitMq;
        protected readonly UserEventPublisher _publisher;
        protected BaseController(AppDbContext db,JwtService jwtService, RabbitMqService rabbitMq, UserEventPublisher publisher)
        {
            _db = db;
            _jwtService = jwtService;
            _rabbitMq = rabbitMq;
            _publisher = publisher;
        }
    }
}