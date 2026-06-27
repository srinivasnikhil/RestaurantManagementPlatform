using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RestaurantPlatform.Application.DTOs
{
    public class AddCartItemDto
    {
        [Range(1, int.MaxValue)]
        public int MenuItemId { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; }

        public string? SelectedOptions { get; set; }
        public string? Notes { get; set; }
    }
}
