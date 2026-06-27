using RestaurantPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.Interfaces
{
    public interface IReservationRepository : IGenericRepository<Reservation>
    {
        Task<IReadOnlyList<Reservation>> GetByUserIdAsync(int userId);
        Task<IReadOnlyList<Reservation>> GetAllOrderedAsync();
    }
}
