using RestaurantPlatform.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto?> PlaceOrderAsync(int userId, PlaceOrderDto dto);
        Task<IReadOnlyList<OrderDto>> GetMyOrdersAsync(int userId);
        Task<OrderDto?> GetByIdAsync(int orderId, int userId, bool isAdmin);
        Task<IReadOnlyList<OrderDto>> GetAllAsync();
        Task<bool> UpdateStatusAsync(int orderId, UpdateOrderStatusDto dto);
    }
}
