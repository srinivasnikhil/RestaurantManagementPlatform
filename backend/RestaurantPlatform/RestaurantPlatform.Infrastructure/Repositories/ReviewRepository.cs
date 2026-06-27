using Microsoft.EntityFrameworkCore;
using RestaurantPlatform.Application.Interfaces;
using RestaurantPlatform.Domain.Entities;
using RestaurantPlatform.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Infrastructure.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(AppDbContext context) : base(context) { }

        public async Task<IReadOnlyList<Review>> GetByMenuItemAsync(int menuItemId)
        {
            return await _context.Reviews
            .Include(r => r.User)
            .Where(r => r.MenuItemId == menuItemId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
        }

        public async Task<Review?> GetByUserAndItemAsync(int userId, int menuItemId)
        {
            return await _context.Reviews
            .FirstOrDefaultAsync(r => r.UserId == userId && r.MenuItemId == menuItemId);
        }
    }
}
