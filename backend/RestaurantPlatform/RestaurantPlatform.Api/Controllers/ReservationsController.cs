using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantPlatform.Application.DTOs;
using RestaurantPlatform.Application.Interfaces;
using System.Security.Claims;

namespace RestaurantPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _service;

        public ReservationsController(IReservationService service)
        {
            _service = service;
        }

        private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost]
        public async Task<IActionResult> Create(CreateReservationDto dto)
        {
            return Ok(await _service.CreateAsync(UserId, dto));
        }

        [HttpGet]
        public async Task<IActionResult> GetMine()
        {
            return Ok(await _service.GetMyReservationsAsync(UserId));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateReservationStatusDto dto)
        {
            return await _service.UpdateStatusAsync(id, dto) ? NoContent() : NotFound();
        }
    }
}
