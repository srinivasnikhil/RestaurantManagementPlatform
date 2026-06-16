using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }   // price captured at order time
        public string? SelectedOptions { get; set; }
    }
}
