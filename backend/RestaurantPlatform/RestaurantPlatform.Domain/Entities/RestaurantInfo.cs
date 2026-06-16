using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Domain.Entities
{
    public class RestaurantInfo
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
        public ICollection<OperatingHours> Hours { get; set; } = new List<OperatingHours>();
    }
}
