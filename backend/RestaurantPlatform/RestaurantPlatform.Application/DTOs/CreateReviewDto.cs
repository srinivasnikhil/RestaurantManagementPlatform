using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RestaurantPlatform.Application.DTOs
{
    public class CreateReviewDto
    {
        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string? Comment { get; set; }
        [Required, StringLength(60, MinimumLength = 2)]
        public string ReviewerName { get; set; } = string.Empty;
    }
}
