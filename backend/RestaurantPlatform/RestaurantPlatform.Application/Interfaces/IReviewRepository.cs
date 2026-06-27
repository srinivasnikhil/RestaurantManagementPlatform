using RestaurantPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<IReadOnlyList<Review>> GetByMenuItemAsync(int menuItemId);
        Task<Review?> GetByUserAndItemAsync(int userId, int menuItemId);
    }
}
