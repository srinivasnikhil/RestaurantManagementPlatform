using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Domain.Entities
{
    public class MenuItem
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsVeg { get; set; }
        public int SpiceLevel { get; set; }   // 0 to 3
        public bool IsAvailable { get; set; } = true;

        public ICollection<ItemOption> Options { get; set; } = new List<ItemOption>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
