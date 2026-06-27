using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantPlatform.Application.DTOs;
using RestaurantPlatform.Application.Interfaces;
using System.Security.Claims;

namespace RestaurantPlatform.Api.Controllers
{
    [Route("api/cart")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;

        public CartController(ICartService service)
        {
            _service = service;
        }

        // pull the logged-in user's id straight off their token
        private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            return Ok(await _service.GetCartAsync(UserId));
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItem(AddCartItemDto dto)
        {
            try
            {
                return Ok(await _service.AddItemAsync(UserId, dto));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("items/{cartItemId:int}")]
        public async Task<IActionResult> UpdateItem(int cartItemId, UpdateCartItemDto dto)
        {
            var result = await _service.UpdateItemAsync(UserId, cartItemId, dto);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpDelete("items/{cartItemId:int}")]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            return await _service.RemoveItemAsync(UserId, cartItemId) ? NoContent() : NotFound();
        } 

        [HttpDelete("clear")]
        public async Task<IActionResult> Clear()
        {
            await _service.ClearCartAsync(UserId);
            return NoContent();
        } 
    }
}
