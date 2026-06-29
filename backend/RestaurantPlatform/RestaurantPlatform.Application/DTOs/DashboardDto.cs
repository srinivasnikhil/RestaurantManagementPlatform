using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.DTOs
{
    public class DashboardDto
    {
        public decimal RevenueToday { get; set; }
        public decimal RevenueThisWeek { get; set; }
        public decimal RevenueThisMonth { get; set; }
        public decimal RevenueAllTime { get; set; }

        public int OrdersToday { get; set; }
        public int OrdersAllTime { get; set; }
        public decimal AverageOrderValue { get; set; }

        public List<DailyRevenueDto> RevenueTrend { get; set; } = new();   // last 30 days
        public List<TopItemDto> TopItems { get; set; } = new();            // best sellers
        public List<StatusCountDto> StatusBreakdown { get; set; } = new(); // orders per status
    }

    public class DailyRevenueDto
    {
        public DateTime Date { get; set; }
        public decimal Revenue { get; set; }
    }

    public class TopItemDto
    {
        public string Name { get; set; } = string.Empty;
        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }
    }

    public class StatusCountDto
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
