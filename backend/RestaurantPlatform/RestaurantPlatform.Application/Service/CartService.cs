using RestaurantPlatform.Application.DTOs;
using RestaurantPlatform.Application.Interfaces;
using RestaurantPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.Service
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _uow;

        public CartService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // get-or-create: every user has exactly one cart
        private async Task<Cart> GetOrCreateCartAsync(int userId)
        {
            var cart = await _uow.Carts.GetByUserIdWithItemsAsync(userId);
            if (cart is null)
            {
                cart = new Cart { UserId = userId };
                await _uow.Carts.AddAsync(cart);
                await _uow.SaveChangesAsync();
            }
            return cart;
        }

        public async Task<CartDto> GetCartAsync(int userId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            return MapCart(cart);
        }

        public async Task<CartDto> AddItemAsync(int userId, AddCartItemDto dto)
        {
            var cart = await GetOrCreateCartAsync(userId);

            // rule: you can only add an item that exists and is available
            var menuItem = await _uow.MenuItems.GetByIdAsync(dto.MenuItemId);
            if (menuItem is null || !menuItem.IsAvailable)
                throw new InvalidOperationException("That item is not available.");

            // if the same item is already in the cart, bump its quantity
            var existing = cart.Items.FirstOrDefault(i => i.MenuItemId == dto.MenuItemId);
            if (existing is not null)
            {
                existing.Quantity += dto.Quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    MenuItemId = dto.MenuItemId,
                    Quantity = dto.Quantity,
                    SelectedOptions = dto.SelectedOptions,
                    Notes = dto.Notes
                });
            }

            await _uow.SaveChangesAsync();

            var updated = await _uow.Carts.GetByUserIdWithItemsAsync(userId);
            return MapCart(updated!);
        }

        public async Task<CartDto?> UpdateItemAsync(int userId, int cartItemId, UpdateCartItemDto dto)
        {
            var cart = await _uow.Carts.GetByUserIdWithItemsAsync(userId);
            var item = cart?.Items.FirstOrDefault(i => i.Id == cartItemId);
            if (item is null) return null;

            item.Quantity = dto.Quantity;
            item.SelectedOptions = dto.SelectedOptions;
            item.Notes = dto.Notes;

            await _uow.SaveChangesAsync();
            var updated = await _uow.Carts.GetByUserIdWithItemsAsync(userId);
            return MapCart(updated!);
        }

        public async Task<bool> RemoveItemAsync(int userId, int cartItemId)
        {
            var cart = await _uow.Carts.GetByUserIdWithItemsAsync(userId);
            var item = cart?.Items.FirstOrDefault(i => i.Id == cartItemId);
            if (item is null) return false;

            cart!.Items.Remove(item);   // removing a tracked child deletes the row
            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task ClearCartAsync(int userId)
        {
            var cart = await _uow.Carts.GetByUserIdWithItemsAsync(userId);
            if (cart is null) return;

            cart.Items.Clear();
            await _uow.SaveChangesAsync();
        }

        // build the DTO with live prices computed right now
        private static CartDto MapCart(Cart cart)
        {
            var items = cart.Items.Select(i => new CartItemDto
            {
                Id = i.Id,
                MenuItemId = i.MenuItemId,
                MenuItemName = i.MenuItem?.Name ?? string.Empty,
                UnitPrice = i.MenuItem?.Price ?? 0m,
                Quantity = i.Quantity,
                LineTotal = (i.MenuItem?.Price ?? 0m) * i.Quantity,
                SelectedOptions = i.SelectedOptions,
                Notes = i.Notes
            }).ToList();

            return new CartDto
            {
                Id = cart.Id,
                Items = items,
                Subtotal = items.Sum(x => x.LineTotal)
            };
        }
    }
}
