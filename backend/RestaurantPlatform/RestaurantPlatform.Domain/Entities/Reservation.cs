using RestaurantPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Domain.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime ReservationDateTime { get; set; }
        public int PartySize { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
        public string? Notes { get; set; }
    }
}
