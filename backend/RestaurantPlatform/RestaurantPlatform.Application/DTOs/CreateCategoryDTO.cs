using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.DTOs
{
    public class CreateCategoryDTO
    {
        public string Name { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }
}
