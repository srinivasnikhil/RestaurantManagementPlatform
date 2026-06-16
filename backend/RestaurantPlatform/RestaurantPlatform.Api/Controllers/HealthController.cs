using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantPlatform.Infrastructure.Data;

namespace RestaurantPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly AppDbContext _db;
        public HealthController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var canConnect = await _db.Database.CanConnectAsync();
            return Ok(new
            {
                status = "healthy",
                database = canConnect ? "connected" : "unreachable",
                time = DateTime.UtcNow
            });
        }
    }
}
