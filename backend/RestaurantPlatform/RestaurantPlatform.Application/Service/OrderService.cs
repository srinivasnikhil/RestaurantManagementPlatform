using RestaurantPlatform.Application.DTOs;
using RestaurantPlatform.Application.Interfaces;
using RestaurantPlatform.Domain.Entities;
using RestaurantPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.Service
{
    public class OrderService : IOrderService
    {
        private const decimal TaxRate = 0.07m;   // 7% tax, adjust as you like

        private readonly IUnitOfWork _uow;

        public OrderService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<OrderDto> PlaceGuestOrderAsync(PlaceGuestOrderDto dto)
        {
            var order = new Order
            {
                UserId = null,                       // guest order, no account
                CustomerName = dto.CustomerName,
                CustomerPhone = dto.CustomerPhone,
                CustomerAddress = dto.CustomerAddress,
                Type = dto.Type,
                Status = OrderStatus.Placed,
                CreatedAt = DateTime.UtcNow,
                TrackingCode = Guid.NewGuid().ToString("N"),
                Items = new List<OrderItem>()
            };

            // the server prices every line from the database; client prices are ignored
            foreach (var line in dto.Items)
            {
                var menuItem = await _uow.MenuItems.GetByIdAsync(line.MenuItemId);
                if (menuItem is null || !menuItem.IsAvailable)
                    throw new InvalidOperationException("An item in your cart is no longer available.");

                order.Items.Add(new OrderItem
                {
                    MenuItemId = menuItem.Id,
                    Quantity = line.Quantity,
                    UnitPrice = menuItem.Price,      // captured and frozen here
                    SelectedOptions = line.SelectedOptions
                });
            }

            order.Subtotal = order.Items.Sum(i => i.UnitPrice * i.Quantity);
            order.Tax = Math.Round(order.Subtotal * TaxRate, 2);
            order.Total = order.Subtotal + order.Tax;

            await _uow.Orders.AddAsync(order);
            await _uow.SaveChangesAsync();

            var saved = await _uow.Orders.GetByIdWithItemsAsync(order.Id);
            return MapOrder(saved!);
        }

        public async Task<OrderDto?> GetByTrackingCodeAsync(string code)
        {
            var order = await _uow.Orders.GetByTrackingCodeAsync(code);
            return order is null ? null : MapOrder(order);
        }

        public async Task<OrderDto?> GetByIdAsync(int orderId)
        {
            var order = await _uow.Orders.GetByIdWithItemsAsync(orderId);
            return order is null ? null : MapOrder(order);
        }

        public async Task<IReadOnlyList<OrderDto>> GetAllAsync()
        {
            var orders = await _uow.Orders.GetAllWithItemsAsync();
            return orders.Select(MapOrder).ToList();
        }

        public async Task<bool> UpdateStatusAsync(int orderId, UpdateOrderStatusDto dto)
        {
            var order = await _uow.Orders.GetByIdAsync(orderId);
            if (order is null) return false;

            order.Status = dto.Status;
            _uow.Orders.Update(order);
            await _uow.SaveChangesAsync();
            return true;
        }

        private static OrderDto MapOrder(Order o) => new()
        {
            Id = o.Id,
            Status = o.Status.ToString(),
            Type = o.Type.ToString(),
            Subtotal = o.Subtotal,
            Tax = o.Tax,
            Total = o.Total,
            CreatedAt = o.CreatedAt,
            TrackingCode = o.TrackingCode,
            CustomerName = o.CustomerName,
            CustomerPhone = o.CustomerPhone,
            CustomerAddress = o.CustomerAddress,
            Items = o.Items.Select(i => new OrderItemDto
            {
                MenuItemId = i.MenuItemId,
                MenuItemName = i.MenuItem?.Name ?? string.Empty,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                LineTotal = i.UnitPrice * i.Quantity,
                SelectedOptions = i.SelectedOptions
            }).ToList()
        };
    }
}
