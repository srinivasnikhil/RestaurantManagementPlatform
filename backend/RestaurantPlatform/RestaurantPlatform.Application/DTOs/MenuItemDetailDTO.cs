using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.DTOs
{
    public class MenuItemDetailDTO : MenuItemDTO
    {
        public List<ItemOptionDTO> Options { get; set; } = new();
    }
}
