using RestaurantPlatform.Application.DTOs;
using RestaurantPlatform.Application.Interfaces;
using RestaurantPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.Service
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IUnitOfWork _uow;

        public MenuItemService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IReadOnlyList<MenuItemDTO>> GetAllAsync(int? categoryId, bool? isVeg, string? search)
        {
            var items = await _uow.MenuItems.GetFilteredAsync(categoryId, isVeg, search);
            return items.Select(MapToDto).ToList();
        }

        public async Task<MenuItemDetailDTO?> GetByIdAsync(int id)
        {
            var item = await _uow.MenuItems.GetByIdWithDetailsAsync(id);
            if (item is null) return null;

            var dto = new MenuItemDetailDTO();
            FillCommon(item, dto);
            dto.Options = item.Options
                .Select(o => new ItemOptionDTO { Id = o.Id, Name = o.Name, ExtraPrice = o.ExtraPrice })
                .ToList();
            return dto;
        }

        public async Task<MenuItemDTO?> CreateAsync(CreateMenuItemDTO dto)
        {
            // business rule: you can't attach an item to a category that doesn't exist
            var category = await _uow.Categories.GetByIdAsync(dto.CategoryId);
            if (category is null) return null;

            var item = new MenuItem
            {
                CategoryId = dto.CategoryId,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                IsVeg = dto.IsVeg,
                SpiceLevel = dto.SpiceLevel,
                IsAvailable = true
            };

            await _uow.MenuItems.AddAsync(item);
            await _uow.SaveChangesAsync();

            var created = await _uow.MenuItems.GetByIdWithDetailsAsync(item.Id);
            return MapToDto(created!);
        }

        public async Task<bool> UpdateAsync(int id, UpdateMenuItemDTO dto)
        {
            var item = await _uow.MenuItems.GetByIdAsync(id);
            if (item is null) return false;

            item.CategoryId = dto.CategoryId;
            item.Name = dto.Name;
            item.Description = dto.Description;
            item.Price = dto.Price;
            item.ImageUrl = dto.ImageUrl;
            item.IsVeg = dto.IsVeg;
            item.SpiceLevel = dto.SpiceLevel;
            item.IsAvailable = dto.IsAvailable;

            _uow.MenuItems.Update(item);
            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _uow.MenuItems.GetByIdAsync(id);
            if (item is null) return false;

            _uow.MenuItems.Delete(item);
            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetAvailabilityAsync(int id, bool isAvailable)
        {
            var item = await _uow.MenuItems.GetByIdAsync(id);
            if (item is null) return false;

            item.IsAvailable = isAvailable;
            _uow.MenuItems.Update(item);
            await _uow.SaveChangesAsync();
            return true;
        }

        // shared mapping so we don't repeat ourselves
        private static void FillCommon(MenuItem m, MenuItemDTO dto)
        {
            dto.Id = m.Id;
            dto.Name = m.Name;
            dto.Description = m.Description;
            dto.Price = m.Price;
            dto.ImageUrl = m.ImageUrl;
            dto.IsVeg = m.IsVeg;
            dto.SpiceLevel = m.SpiceLevel;
            dto.IsAvailable = m.IsAvailable;
            dto.CategoryId = m.CategoryId;
            dto.CategoryName = m.Category?.Name ?? string.Empty;
            dto.AverageRating = m.Reviews.Any() ? Math.Round(m.Reviews.Average(r => r.Rating), 1) : 0;
        }

        private static MenuItemDTO MapToDto(MenuItem m)
        {
            var dto = new MenuItemDTO();
            FillCommon(m, dto);
            return dto;
        }
    }
}
