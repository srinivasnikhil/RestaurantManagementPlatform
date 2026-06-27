using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantPlatform.Application.DTOs;
using RestaurantPlatform.Application.Interfaces;
using System.Security.Claims;

namespace RestaurantPlatform.Api.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrdersController(IOrderService service)
        {
            _service = service;
        }

        private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        private bool IsAdmin => User.IsInRole("Admin");

        [HttpPost]
        public async Task<IActionResult> Place(PlaceOrderDto dto)
        {
            var order = await _service.PlaceOrderAsync(UserId, dto);
            return order is null
                ? BadRequest("Your cart is empty.")
                : CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpGet]
        public async Task<IActionResult> GetMyOrders()
        {
            return Ok(await _service.GetMyOrdersAsync(UserId));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _service.GetByIdAsync(id, UserId, IsAdmin);
            return order is null ? NotFound() : Ok(order);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateOrderStatusDto dto)
        {
            return await _service.UpdateStatusAsync(id, dto) ? NoContent() : NotFound();
        }
    }
}
