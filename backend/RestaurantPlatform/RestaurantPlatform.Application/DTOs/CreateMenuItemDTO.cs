using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RestaurantPlatform.Application.DTOs
{
    public class CreateMenuItemDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "A valid CategoryId is required.")]
        public int CategoryId { get; set; }

        [Required, StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Range(0.01, 10000, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }
        public bool IsVeg { get; set; }

        [Range(0, 3, ErrorMessage = "Spice level is 0 to 3.")]
        public int SpiceLevel { get; set; }
    }
}
