using Microsoft.EntityFrameworkCore;
using RestaurantPlatform.Application.Interfaces;
using RestaurantPlatform.Domain.Entities;
using RestaurantPlatform.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Infrastructure.Repositories
{
    public class MenuItemRepository : GenericRepository<MenuItem>, IMenuItemRepository
    {
        public MenuItemRepository(AppDbContext context) : base(context) { }
        public async Task<MenuItem?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.MenuItems
            .Include(m => m.Category)
            .Include(m => m.Options)
            .Include(m => m.Reviews)
            .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IReadOnlyList<MenuItem>> GetFilteredAsync(int? categoryId, bool? isVeg, string? search)
        {
            var query = _context.MenuItems
            .Include(m => m.Category)
            .Include(m => m.Reviews)
            .AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(m => m.CategoryId == categoryId.Value);

            if (isVeg.HasValue)
                query = query.Where(m => m.IsVeg == isVeg.Value);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(m => m.Name.Contains(search) || m.Description.Contains(search));

            return await query.OrderBy(m => m.Name).ToListAsync();
        }
    }
}
