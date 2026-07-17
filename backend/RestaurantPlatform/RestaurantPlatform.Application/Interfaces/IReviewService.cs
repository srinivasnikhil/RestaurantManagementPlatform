using RestaurantPlatform.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.Interfaces
{
    public interface IReviewService
    {
        Task<IReadOnlyList<ReviewDto>> GetForItemAsync(int menuItemId);
        Task<ReviewDto?> AddAsync(int menuItemId, CreateReviewDto dto);
        Task<bool> DeleteAsync(int reviewId);
    }
}
