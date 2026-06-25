using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.DTOs
{
    public class MenuItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsVeg { get; set; }
        public int SpiceLevel { get; set; }
        public bool IsAvailable { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public double AverageRating { get; set; }
    }
}
