using RestaurantPlatform.Application.DTOs;
using RestaurantPlatform.Application.Interfaces;
using RestaurantPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _uow;
        public CategoryService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<CategoryDTO> CreateAsync(CreateCategoryDTO dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                DisplayOrder = dto.DisplayOrder,
                IsActive = true
            };

            await _uow.Categories.AddAsync(category);
            await _uow.SaveChangesAsync();      // the manager commits

            return MapToDto(category);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _uow.Categories.GetByIdAsync(id);
            if (category is null) return false;

            // soft delete: we hide it, we don't erase it (matches your API contract)
            category.IsActive = false;
            _uow.Categories.Update(category);
            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<IReadOnlyList<CategoryDTO>> GetAllAsync()
        {
            var categories = await _uow.Categories.GetAllAsync();
            return categories.Where(c => c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .Select(MapToDto)
            .ToList();
        }

        public async Task<CategoryDTO?> GetByIdAsync(int id)
        {
            var category = await _uow.Categories.GetByIdAsync(id);
            return category is null ? null : MapToDto(category);
        }

        public async Task<bool> UpdateAsync(int id, UpdateCategoryDTO dto)
        {
            var category = await _uow.Categories.GetByIdAsync(id);
            if (category is null) return false;

            category.Name = dto.Name;
            category.DisplayOrder = dto.DisplayOrder;
            category.IsActive = dto.IsActive;

            _uow.Categories.Update(category);
            await _uow.SaveChangesAsync();
            return true;
        }

        private static CategoryDTO MapToDto(Category c) => new()
        {
            Id = c.Id,
            Name = c.Name,
            DisplayOrder = c.DisplayOrder,
            IsActive = c.IsActive
        };
    }
}
