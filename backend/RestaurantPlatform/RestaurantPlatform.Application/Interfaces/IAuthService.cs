using RestaurantPlatform.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO?> RegisterAsync(RegisterDTO dto);
        Task<AuthResponseDTO?> LoginAsync(LoginDTO dto);
        Task<UserDTO?> GetByIdAsync(int id);
    }
}
