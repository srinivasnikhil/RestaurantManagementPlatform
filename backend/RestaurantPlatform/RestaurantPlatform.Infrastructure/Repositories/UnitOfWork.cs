using RestaurantPlatform.Application.Interfaces;
using RestaurantPlatform.Domain.Entities;
using RestaurantPlatform.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IGenericRepository<Category> Categories { get; }
        public IMenuItemRepository MenuItems { get; }
        public IUserRepository Users { get; }
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Categories = new GenericRepository<Category>(context);
            MenuItems = new MenuItemRepository(context);
            Users = new UserRepository(context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose() 
        {
            _context.Dispose();
        }
    }
}
