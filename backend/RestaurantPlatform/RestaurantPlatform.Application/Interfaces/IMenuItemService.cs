using RestaurantPlatform.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.Interfaces
{
    public interface IMenuItemService
    {
        Task<IReadOnlyList<MenuItemDTO>> GetAllAsync(int? categoryId, bool? isVeg, string? search);
        Task<MenuItemDetailDTO?> GetByIdAsync(int id);
        Task<MenuItemDTO?> CreateAsync(CreateMenuItemDTO dto);
        Task<bool> UpdateAsync(int id, UpdateMenuItemDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> SetAvailabilityAsync(int id, bool isAvailable);
    }
}
