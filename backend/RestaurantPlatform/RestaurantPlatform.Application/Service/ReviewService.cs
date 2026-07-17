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

        public async Task<ReviewDto?> AddAsync(int menuItemId, CreateReviewDto dto)
        {
            var item = await _uow.MenuItems.GetByIdAsync(menuItemId);
            if (item is null) return null;

            var review = new Review
            {
                MenuItemId = menuItemId,
                UserId = null,
                ReviewerName = dto.ReviewerName,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _uow.Reviews.AddAsync(review);
            await _uow.SaveChangesAsync();

            return new ReviewDto
            {
                Id = review.Id,
                Rating = review.Rating,
                Comment = review.Comment,
                UserName = dto.ReviewerName,
                CreatedAt = review.CreatedAt
            };
        }

        public async Task<bool> DeleteAsync(int reviewId)
        {
            var review = await _uow.Reviews.GetByIdAsync(reviewId);
            if (review is null) return false;

            _uow.Reviews.Delete(review);
            await _uow.SaveChangesAsync();
            return true;
        }

        private static ReviewDto MapDto(Review r) => new()
        {
            Id = r.Id,
            Rating = r.Rating,
            Comment = r.Comment,
            UserName = r.User?.Name ?? r.ReviewerName ?? "Anonymous",
            CreatedAt = r.CreatedAt
        };
    }
}
