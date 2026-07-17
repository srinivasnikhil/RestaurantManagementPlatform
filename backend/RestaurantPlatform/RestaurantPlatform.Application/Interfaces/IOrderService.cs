using RestaurantPlatform.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> PlaceGuestOrderAsync(PlaceGuestOrderDto dto, string? paymentReference = null);
        Task<OrderDto?> GetByTrackingCodeAsync(string code);
        Task<OrderDto?> GetByIdAsync(int orderId);
        Task<IReadOnlyList<OrderDto>> GetAllAsync();
        Task<bool> UpdateStatusAsync(int orderId, UpdateOrderStatusDto dto);
        Task<decimal> QuoteAsync(PlaceGuestOrderDto dto);
    }
}
