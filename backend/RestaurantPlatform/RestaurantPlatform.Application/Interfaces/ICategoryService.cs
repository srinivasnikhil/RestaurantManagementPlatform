using RestaurantPlatform.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<CategoryDTO>> GetAllAsync();
        Task<CategoryDTO?> GetByIdAsync(int id);
        Task<CategoryDTO> CreateAsync(CreateCategoryDTO dto);
        Task<bool> UpdateAsync(int id, UpdateCategoryDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
