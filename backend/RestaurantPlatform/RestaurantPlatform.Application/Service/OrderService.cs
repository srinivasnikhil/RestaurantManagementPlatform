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

        public async Task<OrderDto?> PlaceOrderAsync(int userId, PlaceOrderDto dto)
        {
            // 1. read the cart with its items and live menu prices
            var cart = await _uow.Carts.GetByUserIdWithItemsAsync(userId);
            if (cart is null || cart.Items.Count == 0)
                return null;   // nothing to order

            // 2. build the order, FREEZING each price as we go
            var order = new Order
            {
                UserId = userId,
                Type = dto.Type,
                Status = OrderStatus.Placed,
                CreatedAt = DateTime.UtcNow,
                Items = cart.Items.Select(ci => new OrderItem
                {
                    MenuItemId = ci.MenuItemId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.MenuItem!.Price,   // <-- the freeze: today's price, copied
                    SelectedOptions = ci.SelectedOptions
                }).ToList()
            };

            // 3. compute and freeze the money on the order
            order.Subtotal = order.Items.Sum(i => i.UnitPrice * i.Quantity);
            order.Tax = Math.Round(order.Subtotal * TaxRate, 2);
            order.Total = order.Subtotal + order.Tax;

            // 4. save order, then empty the cart (the order is now the record)
            await _uow.Orders.AddAsync(order);
            cart.Items.Clear();
            await _uow.SaveChangesAsync();

            var saved = await _uow.Orders.GetByIdWithItemsAsync(order.Id);
            return MapOrder(saved!);
        }

        public async Task<IReadOnlyList<OrderDto>> GetMyOrdersAsync(int userId)
        {
            var orders = await _uow.Orders.GetByUserIdAsync(userId);
            return orders.Select(MapOrder).ToList();
        }

        public async Task<OrderDto?> GetByIdAsync(int orderId, int userId, bool isAdmin)
        {
            var order = await _uow.Orders.GetByIdWithItemsAsync(orderId);
            if (order is null) return null;

            // rule: a customer can only see their own order; an admin can see any
            if (!isAdmin && order.UserId != userId) return null;

            return MapOrder(order);
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
            Items = o.Items.Select(i => new OrderItemDto
            {
                MenuItemId = i.MenuItemId,
                MenuItemName = i.MenuItem?.Name ?? string.Empty,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,                 // read the FROZEN price
                LineTotal = i.UnitPrice * i.Quantity,
                SelectedOptions = i.SelectedOptions
            }).ToList()
        };
    }
}
