using Microsoft.EntityFrameworkCore;
using RestaurantPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public AppDbContext() { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<MenuItem> MenuItems => Set<MenuItem>();
        public DbSet<ItemOption> ItemOptions => Set<ItemOption>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<Reservation> Reservations => Set<Reservation>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<RestaurantInfo> RestaurantInfos => Set<RestaurantInfo>();
        public DbSet<OperatingHours> OperatingHours => Set<OperatingHours>();


        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            // money columns need explicit precision
            b.Entity<MenuItem>().Property(x => x.Price).HasPrecision(10, 2);
            b.Entity<ItemOption>().Property(x => x.ExtraPrice).HasPrecision(10, 2);
            b.Entity<Order>().Property(x => x.Subtotal).HasPrecision(10, 2);
            b.Entity<Order>().Property(x => x.Tax).HasPrecision(10, 2);
            b.Entity<Order>().Property(x => x.Total).HasPrecision(10, 2);
            b.Entity<OrderItem>().Property(x => x.UnitPrice).HasPrecision(10, 2);
            b.Entity<Payment>().Property(x => x.Amount).HasPrecision(10, 2);

            // one cart per user, one payment per order
            b.Entity<Cart>().HasOne(c => c.User).WithOne(u => u.Cart).HasForeignKey<Cart>(c => c.UserId);
            b.Entity<Payment>().HasOne(p => p.Order).WithOne(o => o.Payment).HasForeignKey<Payment>(p => p.OrderId);

            // email must be unique
            b.Entity<User>().HasIndex(u => u.Email).IsUnique();

            // stop SQL Server complaining about "multiple cascade paths"
            b.Entity<Review>().HasOne(r => r.MenuItem).WithMany(m => m.Reviews)
                .HasForeignKey(r => r.MenuItemId).OnDelete(DeleteBehavior.Restrict);
            b.Entity<OrderItem>().HasOne(oi => oi.MenuItem).WithMany()
                .HasForeignKey(oi => oi.MenuItemId).OnDelete(DeleteBehavior.Restrict);
            b.Entity<CartItem>().HasOne(ci => ci.MenuItem).WithMany()
                .HasForeignKey(ci => ci.MenuItemId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
