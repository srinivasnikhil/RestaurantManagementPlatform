using Microsoft.EntityFrameworkCore;
using RestaurantPlatform.Application.Interfaces;
using RestaurantPlatform.Domain.Entities;
using RestaurantPlatform.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Infrastructure.Repositories
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(AppDbContext context) : base(context) { }

        public async Task<Cart?> GetByUserIdWithItemsAsync(int userId)
        {
            return await _context.Carts.Include(c => c.Items).ThenInclude(i => i.MenuItem).FirstOrDefaultAsync(c => c.UserId == userId);
        }
    }
}
