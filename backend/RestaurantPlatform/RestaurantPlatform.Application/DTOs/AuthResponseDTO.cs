using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.DTOs
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public UserDTO User { get; set; } = new();
    }
}
