using RestaurantPlatform.Application.DTOs;
using RestaurantPlatform.Application.Interfaces;
using RestaurantPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _uow;
        public ReviewService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IReadOnlyList<ReviewDto>> GetForItemAsync(int menuItemId)
        {
            return (await _uow.Reviews.GetByMenuItemAsync(menuItemId)).Select(MapDto).ToList();
        }

        public async Task<ReviewDto?> AddAsync(int userId, int menuItemId, CreateReviewDto dto)
        {
            // rule 1: the item must exist
            var item = await _uow.MenuItems.GetByIdAsync(menuItemId);
            if (item is null) return null;

            // rule 2: one review per user per item
            var existing = await _uow.Reviews.GetByUserAndItemAsync(userId, menuItemId);
            if (existing is not null)
                throw new InvalidOperationException("You have already reviewed this item.");

            var review = new Review
            {
                UserId = userId,
                MenuItemId = menuItemId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _uow.Reviews.AddAsync(review);
            await _uow.SaveChangesAsync();

            // reload with the user attached so we can show the name
            var saved = await _uow.Reviews.GetByUserAndItemAsync(userId, menuItemId);
            var user = await _uow.Users.GetByIdAsync(userId);
            return new ReviewDto
            {
                Id = saved!.Id,
                Rating = saved.Rating,
                Comment = saved.Comment,
                UserName = user?.Name ?? string.Empty,
                CreatedAt = saved.CreatedAt
            };
        }

        public async Task<bool> DeleteAsync(int userId, int reviewId, bool isAdmin)
        {
            var review = await _uow.Reviews.GetByIdAsync(reviewId);
            if (review is null) return false;

            // rule: you can delete your own review; an admin can delete any
            if (!isAdmin && review.UserId != userId) return false;

            _uow.Reviews.Delete(review);
            await _uow.SaveChangesAsync();
            return true;
        }

        private static ReviewDto MapDto(Review r) => new()
        {
            Id = r.Id,
            Rating = r.Rating,
            Comment = r.Comment,
            UserName = r.User?.Name ?? string.Empty,
            CreatedAt = r.CreatedAt
        };
    }
}
