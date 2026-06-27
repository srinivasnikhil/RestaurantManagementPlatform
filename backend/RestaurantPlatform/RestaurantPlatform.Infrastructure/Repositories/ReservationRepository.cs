using RestaurantPlatform.Application.Interfaces;
using RestaurantPlatform.Domain.Entities;
using RestaurantPlatform.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace RestaurantPlatform.Infrastructure.Repositories
{
    public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
    {
        public ReservationRepository(AppDbContext context) : base(context) { }

        public async Task<IReadOnlyList<Reservation>> GetByUserIdAsync(int userId)
        {
            return await _context.Reservations
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.ReservationDateTime)
            .ToListAsync();
        }

        public async Task<IReadOnlyList<Reservation>> GetAllOrderedAsync()
        {
            return await _context.Reservations
                .OrderByDescending(r => r.ReservationDateTime)
                .ToListAsync();
        }

    }
}
