using RestaurantPlatform.Application.DTOs;
using RestaurantPlatform.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repo;
        public DashboardService(IDashboardRepository repo)
        {
            _repo = repo;
        }

        public async Task<DashboardDto> GetDashboardAsync()
        {
            return await _repo.GetDashboardAsync();
        }
    }
}
