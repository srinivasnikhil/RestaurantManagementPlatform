using RestaurantPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RestaurantPlatform.Application.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
        public string? CustomerName { get; set; }
        public string TrackingCode { get; set; } = string.Empty;
        public string? CustomerPhone { get; set; }
        public string? CustomerAddress { get; set; }
    }
    public class PlaceOrderItemDto
    {
        [Range(1, int.MaxValue)]
        public int MenuItemId { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; }

        public string? SelectedOptions { get; set; }
        public string? Notes { get; set; }
    }

    public class PlaceGuestOrderDto
    {
        [Required]
        public OrderType Type { get; set; }

        [Required, StringLength(100, MinimumLength = 2)]
        public string CustomerName { get; set; } = string.Empty;

        [Required, StringLength(30, MinimumLength = 7)]
        public string? CustomerPhone { get; set; } = string.Empty;

        public string? CustomerAddress { get; set; }

        [MinLength(1, ErrorMessage = "Your order must have at least one item.")]
        public List<PlaceOrderItemDto> Items { get; set; } = new();
    }
}
