using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantPlatform.Application.DTOs;
using RestaurantPlatform.Application.Interfaces;
using System.Security.Claims;

namespace RestaurantPlatform.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _service;

        public ReviewsController(IReviewService service) 
        { 
            _service = service; 
        }

        private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        private bool IsAdmin => User.IsInRole("Admin");

        // public: anyone can read reviews for an item
        [HttpGet("menu-items/{menuItemId:int}/reviews")]
        public async Task<IActionResult> GetForItem(int menuItemId)
        {
            return Ok(await _service.GetForItemAsync(menuItemId));
        }

        // logged-in users can post a review
        [AllowAnonymous]
        [HttpPost("menu-items/{menuItemId:int}/reviews")]
        public async Task<IActionResult> Add(int menuItemId, CreateReviewDto dto)
        {
            try
            {
                var review = await _service.AddAsync(menuItemId, dto);
                return review is null ? NotFound("Menu item not found.") : Ok(review);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin, Employee")]
        [HttpDelete("reviews/{reviewId:int}")]
        public async Task<IActionResult> Delete(int reviewId)
        {
            return await _service.DeleteAsync(reviewId) ? NoContent() : NotFound();
        }
    }
}
