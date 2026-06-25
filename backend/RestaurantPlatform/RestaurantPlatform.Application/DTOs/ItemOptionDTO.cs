using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.DTOs
{
    public class ItemOptionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal ExtraPrice { get; set; }
    }
}
