using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; } = null!;
        public int Rating { get; set; }   // 1 to 5
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
