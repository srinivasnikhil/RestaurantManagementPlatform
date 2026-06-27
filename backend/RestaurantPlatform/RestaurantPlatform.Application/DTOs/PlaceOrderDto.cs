using RestaurantPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RestaurantPlatform.Application.DTOs
{
    public class PlaceOrderDto
    {
        [Required]
        public OrderType Type { get; set; }   // Pickup or Delivery
    }
}
