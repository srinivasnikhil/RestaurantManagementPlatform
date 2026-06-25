using RestaurantPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.Interfaces
{
    public interface IMenuItemRepository : IGenericRepository<MenuItem>
    {
        Task<IReadOnlyList<MenuItem>> GetFilteredAsync(int? categoryId, bool? isVeg, string? search);
        Task<MenuItem?> GetByIdWithDetailsAsync(int id);
    }
}
