using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantPlatform.Application.DTOs;
using RestaurantPlatform.Application.Interfaces;
using RestaurantPlatform.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantPlatform.Api.Controllers
{
    [Route("api/payments/paypal")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPayPalService _paypal;
        private readonly IOrderService _orders;
        public PaymentsController(IPayPalService paypal, IOrderService orders)
        {
            _paypal = paypal;
            _orders = orders;
        }

        [AllowAnonymous]
        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder(PlaceGuestOrderDto dto)
        {
            if (dto.Type != OrderType.Pickup && dto.Type != OrderType.Delivery)
                return BadRequest("Online orders must be Pickup or Delivery.");

            try
            {
                var amount = await _orders.QuoteAsync(dto);
                var paypalOrderId = await _paypal.CreateOrderAsync(amount);
                return Ok(new { paypalOrderId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // step 2: capture the payment, then place the restaurant order
        [AllowAnonymous]
        [HttpPost("capture")]
        public async Task<IActionResult> Capture(CapturePaymentDto dto)
        {
            var result = await _paypal.CaptureOrderAsync(dto.PaypalOrderId);
            if (!result.Success)
                return BadRequest($"Payment was not completed (status: {result.Status}).");

            var order = await _orders.PlaceGuestOrderAsync(dto.Order, result.CaptureId);
            return Ok(order);
        }
        // staff: create a PayPal order for a Dine-in / Takeaway counter sale
        [Authorize(Roles = "Admin,Employee")]
        [HttpPost("staff/create-order")]
        public async Task<IActionResult> CreateStaffOrder(PlaceGuestOrderDto dto)
        {
            if (dto.Type != OrderType.DineIn && dto.Type != OrderType.Takeaway)
                return BadRequest("Staff orders must be Dine-in or Takeaway.");

            try
            {
                var amount = await _orders.QuoteAsync(dto);
                var paypalOrderId = await _paypal.CreateOrderAsync(amount);
                return Ok(new { paypalOrderId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // staff: capture and place the counter order
        [Authorize(Roles = "Admin,Employee")]
        [HttpPost("staff/capture")]
        public async Task<IActionResult> CaptureStaff(CapturePaymentDto dto)
        {
            var result = await _paypal.CaptureOrderAsync(dto.PaypalOrderId);
            if (!result.Success)
                return BadRequest($"Payment was not completed (status: {result.Status}).");

            var order = await _orders.PlaceGuestOrderAsync(dto.Order, result.CaptureId);
            return Ok(order);
        }
    }
}
