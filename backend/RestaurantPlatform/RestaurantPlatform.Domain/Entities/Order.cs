using RestaurantPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public OrderStatus Status { get; set; } = OrderStatus.Placed;
        public OrderType Type { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public Payment? Payment { get; set; }
    }
}
