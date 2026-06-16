using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Domain.Entities
{
    public class ItemOption
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public decimal ExtraPrice { get; set; }
    }
}
