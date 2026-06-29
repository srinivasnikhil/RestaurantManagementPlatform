using RestaurantPlatform.Application.DTOs;
using RestaurantPlatform.Application.Interfaces;
using RestaurantPlatform.Domain.Enums;
using RestaurantPlatform.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace RestaurantPlatform.Infrastructure.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext _context;
        public DashboardRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<DashboardDto> GetDashboardAsync()
        {
            var now = DateTime.UtcNow;
            var startOfToday = now.Date;
            var startOfWeek = startOfToday.AddDays(-(int)startOfToday.DayOfWeek); // Sunday-based
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var trendStart = startOfToday.AddDays(-29); // last 30 days inclusive

            // base query: every order that counts as real revenue
            var paidOrders = _context.Orders.Where(o => o.Status != OrderStatus.Cancelled);

            var dto = new DashboardDto
            {
                RevenueToday = await paidOrders
                    .Where(o => o.CreatedAt >= startOfToday)
                    .SumAsync(o => (decimal?)o.Total) ?? 0,

                RevenueThisWeek = await paidOrders
                    .Where(o => o.CreatedAt >= startOfWeek)
                    .SumAsync(o => (decimal?)o.Total) ?? 0,

                RevenueThisMonth = await paidOrders
                    .Where(o => o.CreatedAt >= startOfMonth)
                    .SumAsync(o => (decimal?)o.Total) ?? 0,

                RevenueAllTime = await paidOrders
                    .SumAsync(o => (decimal?)o.Total) ?? 0,

                OrdersToday = await _context.Orders
                    .CountAsync(o => o.CreatedAt >= startOfToday),

                OrdersAllTime = await _context.Orders.CountAsync(),
            };

            // average order value across paid orders
            var paidCount = await paidOrders.CountAsync();
            dto.AverageOrderValue = paidCount == 0 ? 0 : dto.RevenueAllTime / paidCount;

            // revenue per day for the last 30 days, grouped in the database
            var trendRows = await paidOrders
                .Where(o => o.CreatedAt >= trendStart)
                .GroupBy(o => o.CreatedAt.Date)
                .Select(g => new DailyRevenueDto
                {
                    Date = g.Key,
                    Revenue = g.Sum(o => o.Total)
                })
                .ToListAsync();

            // fill in any missing days with zero so the chart has 30 continuous points
            dto.RevenueTrend = Enumerable.Range(0, 30)
                .Select(i => trendStart.AddDays(i))
                .Select(date => new DailyRevenueDto
                {
                    Date = date,
                    Revenue = trendRows.FirstOrDefault(r => r.Date == date)?.Revenue ?? 0
                })
                .ToList();

            // top 5 selling items, aggregated from order lines
            dto.TopItems = await _context.OrderItems
                .Where(oi => oi.Order.Status != OrderStatus.Cancelled)
                .GroupBy(oi => oi.MenuItem.Name)
                .Select(g => new TopItemDto
                {
                    Name = g.Key,
                    QuantitySold = g.Sum(oi => oi.Quantity),
                    Revenue = g.Sum(oi => oi.UnitPrice * oi.Quantity)
                })
                .OrderByDescending(x => x.QuantitySold)
                .Take(5)
                .ToListAsync();

            // count of orders in each status (includes Cancelled here)
            dto.StatusBreakdown = await _context.Orders
                .GroupBy(o => o.Status)
                .Select(g => new StatusCountDto
                {
                    Status = g.Key.ToString(),
                    Count = g.Count()
                })
                .ToListAsync();

            return dto;
        }
    }
}
