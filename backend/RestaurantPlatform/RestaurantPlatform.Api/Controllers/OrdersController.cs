using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantPlatform.Application.DTOs;
using RestaurantPlatform.Application.Interfaces;
using RestaurantPlatform.Domain.Enums;
using System.Diagnostics;
using System.Security.Claims;

namespace RestaurantPlatform.Api.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;
        private readonly IReceiptService _receiptService;

        public OrdersController(IOrderService service, IReceiptService receiptService)
        {
            _service = service;
            _receiptService = receiptService;
        }

        private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        private bool IsAdmin => User.IsInRole("Admin");

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Place(PlaceGuestOrderDto dto)
        {
            if (dto.Type != OrderType.Pickup && dto.Type != OrderType.Delivery)
                return BadRequest("Online orders must be Pickup or Delivery.");

            if (dto.Type == OrderType.Delivery && string.IsNullOrWhiteSpace(dto.CustomerAddress))
                return BadRequest("A delivery address is required for delivery orders.");

            try
            {
                var order = await _service.PlaceGuestOrderAsync(dto);
                return CreatedAtAction(nameof(Track), new { code = order.TrackingCode }, order);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // public order tracking by unguessable code
        [AllowAnonymous]
        [HttpGet("track/{code}")]
        public async Task<IActionResult> Track(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return NotFound();
            var order = await _service.GetByTrackingCodeAsync(code);
            return order is null ? NotFound() : Ok(order);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetMyOrders()
        //{
        //    return Ok(await _service.GetMyOrdersAsync(UserId));
        //}
        [Authorize(Roles = "Admin,Employee")]
        [HttpPost("staff")]
        public async Task<IActionResult> PlaceStaff(PlaceGuestOrderDto dto)
        {
            if (dto.Type != OrderType.DineIn && dto.Type != OrderType.Takeaway)
                return BadRequest("Staff orders must be Dine-in or Takeaway.");

            try
            {
                var order = await _service.PlaceGuestOrderAsync(dto);
                return Ok(order);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _service.GetByIdAsync(id);
            return order is null ? NotFound() : Ok(order);
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateOrderStatusDto dto)
        {
            return await _service.UpdateStatusAsync(id, dto) ? NoContent() : NotFound();
        }
        // public: customer downloads their own receipt with the tracking code
        [AllowAnonymous]
        [HttpGet("track/{code}/receipt")]
        public async Task<IActionResult> ReceiptByCode(string code)
        {
            var order = await _service.GetByTrackingCodeAsync(code);
            if (order is null) return NotFound();

            var pdf = _receiptService.GenerateReceiptPdf(order);
            return File(pdf, "application/pdf", $"receipt-order-{order.Id}.pdf");
        }

        // staff: reprint any order by id
        [Authorize(Roles = "Admin,Employee")]
        [HttpGet("{id:int}/receipt")]
        public async Task<IActionResult> ReceiptById(int id)
        {
            var order = await _service.GetByIdAsync(id);
            if (order is null) return NotFound();

            var pdf = _receiptService.GenerateReceiptPdf(order);
            return File(pdf, "application/pdf", $"receipt-order-{order.Id}.pdf");
        }
    }
}
