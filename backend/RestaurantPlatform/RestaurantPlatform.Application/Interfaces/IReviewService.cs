using RestaurantPlatform.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.Interfaces
{
    public interface IReviewService
    {
        Task<IReadOnlyList<ReviewDto>> GetForItemAsync(int menuItemId);
        Task<ReviewDto?> AddAsync(int userId, int menuItemId, CreateReviewDto dto);
        Task<bool> DeleteAsync(int userId, int reviewId, bool isAdmin);
    }
}
