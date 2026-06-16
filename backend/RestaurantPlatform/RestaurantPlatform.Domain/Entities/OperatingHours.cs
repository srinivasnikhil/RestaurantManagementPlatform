using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Domain.Entities
{
    public class OperatingHours
    {
        public int Id { get; set; }
        public int RestaurantInfoId { get; set; }
        public RestaurantInfo RestaurantInfo { get; set; } = null!;
        public int DayOfWeek { get; set; }   // 0 to 6
        public TimeOnly OpenTime { get; set; }
        public TimeOnly CloseTime { get; set; }
        public string? Label { get; set; }   // e.g. Dosa Night
    }
}
