using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RestaurantPlatform.Application.DTOs
{
    public class UpdateMenuItemDTO
    {
        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        [Required, StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Range(0.01, 10000)]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }
        public bool IsVeg { get; set; }

        [Range(0, 3)]
        public int SpiceLevel { get; set; }

        public bool IsAvailable { get; set; }
    }
}
