using Microsoft.EntityFrameworkCore;
using RestaurantPlatform.Application.Interfaces;
using RestaurantPlatform.Domain.Entities;
using RestaurantPlatform.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Infrastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context) { }

        public async Task<IReadOnlyList<Order>> GetAllWithItemsAsync()
        {
            return await _context.Orders.Include(o => o.User)
            .Include(o => o.Items).ThenInclude(i => i.MenuItem)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
        }

        public async Task<Order?> GetByIdWithItemsAsync(int orderId)
        {
            return await _context.Orders.Include(o => o.User)
            .Include(o => o.Items).ThenInclude(i => i.MenuItem)
            .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<Order?> GetByTrackingCodeAsync(string code)
        {
            return await _context.Orders
                                .Include(o => o.Items).ThenInclude(i => i.MenuItem)
                                .FirstOrDefaultAsync(o => o.TrackingCode == code);
        }

        public async Task<IReadOnlyList<Order>> GetByUserIdAsync(int userId)
        {
            return await _context.Orders
            .Include(o => o.Items).ThenInclude(i => i.MenuItem)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
        }
    }
}
