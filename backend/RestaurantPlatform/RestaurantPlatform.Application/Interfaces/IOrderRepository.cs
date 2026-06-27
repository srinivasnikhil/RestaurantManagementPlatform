using RestaurantPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IReadOnlyList<Order>> GetByUserIdAsync(int userId);
        Task<Order?> GetByIdWithItemsAsync(int orderId);
        Task<IReadOnlyList<Order>> GetAllWithItemsAsync();
    }
}
