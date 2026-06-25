using RestaurantPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<Category> Categories { get; }
        IMenuItemRepository MenuItems { get; }
        IUserRepository Users { get; }
        Task<int> SaveChangesAsync();
    }
}
