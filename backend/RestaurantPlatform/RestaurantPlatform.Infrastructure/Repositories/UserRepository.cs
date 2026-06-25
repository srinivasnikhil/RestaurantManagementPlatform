using Microsoft.EntityFrameworkCore;
using RestaurantPlatform.Application.Interfaces;
using RestaurantPlatform.Domain.Entities;
using RestaurantPlatform.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
