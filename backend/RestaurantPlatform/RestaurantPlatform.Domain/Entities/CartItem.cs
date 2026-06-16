using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Domain.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; } = null!;
        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; } = null!;
        public int Quantity { get; set; }
        public string? SelectedOptions { get; set; }   // JSON string
        public string? Notes { get; set; }
    }
}
