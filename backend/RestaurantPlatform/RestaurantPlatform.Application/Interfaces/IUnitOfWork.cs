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
        ICartRepository Carts { get; }
        IOrderRepository Orders { get; }
        IReservationRepository Reservations { get; }
        IReviewRepository Reviews { get; }
        Task<int> SaveChangesAsync();
    }
}
