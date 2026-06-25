using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
