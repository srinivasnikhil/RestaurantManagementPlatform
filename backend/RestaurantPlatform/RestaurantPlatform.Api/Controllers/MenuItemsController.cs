using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantPlatform.Application.DTOs;
using RestaurantPlatform.Application.Interfaces;

namespace RestaurantPlatform.Api.Controllers
{
    [Route("api/menu-items")]
    [ApiController]
    public class MenuItemsController : ControllerBase
    {
        private readonly IMenuItemService _service;
        public MenuItemsController(IMenuItemService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? categoryId,[FromQuery] bool? isVeg,[FromQuery] string? search)
        {
            return Ok(await _service.GetAllAsync(categoryId, isVeg, search));
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateMenuItemDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return created is null
                ? BadRequest("That category does not exist.")
                : CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateMenuItemDTO dto)
        {
            return await _service.UpdateAsync(id, dto) ? NoContent() : NotFound();
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _service.DeleteAsync(id) ? NoContent() : NotFound();
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpPatch("{id:int}/availability")]
        public async Task<IActionResult> SetAvailability(int id, [FromBody] bool isAvailable)
        {
            return await _service.SetAvailabilityAsync(id, isAvailable) ? NoContent() : NotFound();
        }

    }
}
