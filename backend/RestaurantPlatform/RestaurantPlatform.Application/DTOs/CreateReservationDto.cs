using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RestaurantPlatform.Application.DTOs
{
    public class CreateReservationDto
    {
        [Required]
        public DateTime ReservationDateTime { get; set; }

        [Range(1, 50)]
        public int PartySize { get; set; }

        public string? Notes { get; set; }
    }
}
