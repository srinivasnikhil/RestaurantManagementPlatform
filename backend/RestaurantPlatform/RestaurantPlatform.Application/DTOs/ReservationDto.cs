using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.DTOs
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public DateTime ReservationDateTime { get; set; }
        public int PartySize { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}
