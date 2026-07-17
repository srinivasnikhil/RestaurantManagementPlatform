using Microsoft.EntityFrameworkCore;
using RestaurantPlatform.Domain.Entities;
using RestaurantPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
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

            b.Entity<Order>().HasIndex(o => o.TrackingCode);

            SeedData(b);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            // ---- Categories (Ids 1..13, fixed) ----
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Dosa", DisplayOrder = 1, IsActive = true },
                new Category { Id = 2, Name = "Biryani & Pulao", DisplayOrder = 2, IsActive = true },
                new Category { Id = 3, Name = "Idly", DisplayOrder = 3, IsActive = true },
                new Category { Id = 4, Name = "Uttapam Spcls", DisplayOrder = 4, IsActive = true },
                new Category { Id = 5, Name = "Curries", DisplayOrder = 5, IsActive = true },
                new Category { Id = 6, Name = "Indian Bread", DisplayOrder = 6, IsActive = true },
                new Category { Id = 7, Name = "Appetizers - Veg", DisplayOrder = 7, IsActive = true },
                new Category { Id = 8, Name = "Appetizers - Non Veg", DisplayOrder = 8, IsActive = true },
                new Category { Id = 9, Name = "Fried Rice / Noodles", DisplayOrder = 9, IsActive = true },
                new Category { Id = 10, Name = "Frankie's", DisplayOrder = 10, IsActive = true },
                new Category { Id = 11, Name = "South Indian Spcl Snacks", DisplayOrder = 11, IsActive = true },
                new Category { Id = 12, Name = "Mandi", DisplayOrder = 12, IsActive = true },
                new Category { Id = 13, Name = "Drinks", DisplayOrder = 13, IsActive = true }
            );

            // ---- Menu items (Ids fixed; prices marked "real" are off the live menu) ----
            modelBuilder.Entity<MenuItem>().HasData(
                // Dosa (Category 1)
                new MenuItem { Id = 1, CategoryId = 1, Name = "Plain Dosa", Description = "Crisp rice and lentil crepe.", Price = 6.99m, IsVeg = true, IsAvailable = true, DisplayOrder = 1, ImageUrl = "/images/menu/plain-dosa.jpg" },       // real
                new MenuItem { Id = 2, CategoryId = 1, Name = "Masala Dosa", Description = "Dosa filled with spiced potato masala.", Price = 6.99m, IsVeg = true, IsAvailable = true, DisplayOrder = 2, ImageUrl = "/images/menu/masala-dosa.jpg" },      // real
                new MenuItem { Id = 3, CategoryId = 1, Name = "Ghee Dosa", Description = "Dosa roasted in clarified butter.", Price = 7.49m, IsVeg = true, IsAvailable = true, DisplayOrder = 3, ImageUrl = "/images/menu/ghee-dosa.jpg" },
                new MenuItem { Id = 4, CategoryId = 1, Name = "Onion Rava Dosa", Description = "Crisp semolina dosa with onions and green chili.", Price = 8.99m, IsVeg = true, IsAvailable = true, DisplayOrder = 4, ImageUrl = "/images/menu/rava-dosa.jpg" },

                // Biryani & Pulao (Category 2)
                new MenuItem { Id = 5, CategoryId = 2, Name = "Vijayawada Biryani", Description = "Spicy Vijayawada style chicken biryani.", Price = 17.99m, IsVeg = false, IsAvailable = true, DisplayOrder = 1, ImageUrl = "/images/menu/vijayawada-biryani.jpg" }, // real
                new MenuItem { Id = 6, CategoryId = 2, Name = "Veg Dum Biryani", Description = "Basmati rice with seasonal vegetables and spices.", Price = 14.99m, IsVeg = true, IsAvailable = true, DisplayOrder = 2, ImageUrl = "/images/menu/veg-biryani.jpg" },
                new MenuItem { Id = 7, CategoryId = 2, Name = "Chicken Dum Biryani", Description = "Slow-cooked layered chicken biryani.", Price = 16.99m, IsVeg = false, IsAvailable = true, DisplayOrder = 3, ImageUrl = "/images/menu/chicken-biryani.jpg" },
                new MenuItem { Id = 8, CategoryId = 2, Name = "Paneer Pulao", Description = "Fragrant rice tossed with paneer and spices.", Price = 14.49m, IsVeg = true, IsAvailable = true, DisplayOrder = 4, ImageUrl = "/images/menu/paneer-pulao.jpg" },

                // Idly (Category 3)
                new MenuItem { Id = 9, CategoryId = 3, Name = "Plain Idly", Description = "Two steamed rice cakes with chutney and sambar.", Price = 4.99m, IsVeg = true, IsAvailable = true, DisplayOrder = 1, ImageUrl = "/images/menu/plain-idly.jpg" },  // real
                new MenuItem { Id = 10, CategoryId = 3, Name = "Ghee Karam Idly", Description = "Idly tossed in ghee and spicy podi.", Price = 6.99m, IsVeg = true, IsAvailable = true, DisplayOrder = 2, ImageUrl = "/images/menu/karam-idly.jpg" },

                // Uttapam (Category 4)
                new MenuItem { Id = 11, CategoryId = 4, Name = "Plain Uttapam", Description = "Thick savory pancake.", Price = 6.99m, IsVeg = true, IsAvailable = true, DisplayOrder = 1, ImageUrl = "/images/menu/plain-uttapam.jpg" }, // real
                new MenuItem { Id = 12, CategoryId = 4, Name = "Onion Uttapam", Description = "Uttapam topped with onions and chili.", Price = 7.99m, IsVeg = true, IsAvailable = true, DisplayOrder = 2, ImageUrl = "/images/menu/onion-uttapam.jpg" },

                // Curries (Category 5)
                new MenuItem { Id = 13, CategoryId = 5, Name = "Paneer Butter Masala", Description = "Paneer in a rich tomato butter gravy.", Price = 13.99m, IsVeg = true, IsAvailable = true, DisplayOrder = 1, ImageUrl = "/images/menu/paneer-butter-masala.jpg" },
                new MenuItem { Id = 14, CategoryId = 5, Name = "Chicken Curry", Description = "Home-style chicken curry.", Price = 14.99m, IsVeg = false, IsAvailable = true, DisplayOrder = 2, ImageUrl = "/images/menu/chicken-curry.jpg" },

                // Indian Bread (Category 6)
                new MenuItem { Id = 15, CategoryId = 6, Name = "Rumali Roti with Curry", Description = "Thin handkerchief bread served with curry.", Price = 9.99m, IsVeg = true, IsAvailable = true, DisplayOrder = 1, ImageUrl = "/images/menu/rumali-roti.jpg" }, // real (item)
                new MenuItem { Id = 16, CategoryId = 6, Name = "Butter Naan", Description = "Tandoor-baked naan brushed with butter.", Price = 3.99m, IsVeg = true, IsAvailable = true, DisplayOrder = 2, ImageUrl = "/images/menu/butter-naan.jpg" },

                // Appetizers - Veg (Category 7)
                new MenuItem { Id = 17, CategoryId = 7, Name = "Gobi Manchurian", Description = "Crispy cauliflower in tangy Manchurian sauce.", Price = 11.99m, IsVeg = true, IsAvailable = true, DisplayOrder = 1, ImageUrl = "/images/menu/gobi-manchurian.jpg" },
                new MenuItem { Id = 18, CategoryId = 7, Name = "Punjabi Samosa", Description = "Two pastry pockets with spiced potato.", Price = 3.99m, IsVeg = true, IsAvailable = true, DisplayOrder = 2, ImageUrl = "/images/menu/samosa.jpg" }, // real

                // Appetizers - Non Veg (Category 8)
                new MenuItem { Id = 19, CategoryId = 8, Name = "Chicken 65", Description = "Spicy deep-fried chicken bites.", Price = 13.99m, IsVeg = false, IsAvailable = true, DisplayOrder = 1, ImageUrl = "/images/menu/chicken-65.jpg" },
                new MenuItem { Id = 20, CategoryId = 8, Name = "Apollo Fish", Description = "Crispy fried fish tossed in chili and curry leaf.", Price = 15.99m, IsVeg = false, IsAvailable = true, DisplayOrder = 2, ImageUrl = "/images/menu/apollo-fish.jpg" },

                // Fried Rice / Noodles (Category 9)
                new MenuItem { Id = 21, CategoryId = 9, Name = "Street Style Fried Noodles", Description = "Wok-tossed street style noodles.", Price = 14.99m, IsVeg = true, IsAvailable = true, DisplayOrder = 1, ImageUrl = "/images/menu/fried-noodles.jpg" }, // real
                new MenuItem { Id = 22, CategoryId = 9, Name = "Chicken Fried Rice", Description = "Indian street style chicken fried rice.", Price = 14.99m, IsVeg = false, IsAvailable = true, DisplayOrder = 2, ImageUrl = "/images/menu/chicken-fried-rice.jpg" },

                // Frankie's (Category 10)
                new MenuItem { Id = 23, CategoryId = 10, Name = "Paneer Frankie", Description = "Roll stuffed with spiced paneer.", Price = 9.99m, IsVeg = true, IsAvailable = true, DisplayOrder = 1, ImageUrl = "/images/menu/paneer-frankie.jpg" },
                new MenuItem { Id = 24, CategoryId = 10, Name = "Chicken Frankie", Description = "Roll stuffed with spiced chicken.", Price = 10.99m, IsVeg = false, IsAvailable = true, DisplayOrder = 2, ImageUrl = "/images/menu/chicken-frankie.jpg" },

                // South Indian Spcl Snacks (Category 11)
                new MenuItem { Id = 25, CategoryId = 11, Name = "Neyyi Karam Punugulu", Description = "Ghee and spice tossed fried fritters.", Price = 10.99m, IsVeg = true, IsAvailable = true, DisplayOrder = 1, ImageUrl = "/images/menu/punugulu.jpg" }, // real
                new MenuItem { Id = 26, CategoryId = 11, Name = "Mysore Bonda", Description = "Soft fried lentil dumplings.", Price = 9.99m, IsVeg = true, IsAvailable = true, DisplayOrder = 2, ImageUrl = "/images/menu/mysore-bonda.jpg" }, // real

                // Mandi (Category 12)
                new MenuItem { Id = 27, CategoryId = 12, Name = "Chicken Mandi", Description = "Smoky spiced rice with chicken, Yemeni style.", Price = 18.99m, IsVeg = false, IsAvailable = true, DisplayOrder = 1, ImageUrl = "/images/menu/chicken-mandi.jpg" },

                // Drinks (Category 13)
                new MenuItem { Id = 28, CategoryId = 13, Name = "Irani Chai", Description = "Signature sweet milky tea.", Price = 2.99m, IsVeg = true, IsAvailable = true, DisplayOrder = 1, ImageUrl = "/images/menu/irani-chai.jpg" },
                new MenuItem { Id = 29, CategoryId = 13, Name = "Mango Lassi", Description = "Chilled yogurt and mango drink.", Price = 4.99m, IsVeg = true, IsAvailable = true, DisplayOrder = 2, ImageUrl = "/images/menu/mango-lassi.jpg" }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 2,
                    Name = "Admin",
                    Email = "admin@gmail.com",
                    PasswordHash = "$2a$11$CUx5HnTlYDICDSeOfavhXOU2YXdhc9onAEO1X5Ba.gakC84GJ.it.",
                    Role = UserRole.Admin,
                    CreatedAt = new DateTime(2026, 6, 25)
                }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 6,
                    Name = "Employee",
                    Email = "employee@dosthi.com",
                    PasswordHash = "$2a$11$N0NMPg7x.c2cEfyZGyb72.fWUe3esmtZl0j49DH1UeV9e7CCS0o.C",
                    Role = UserRole.Employee,
                    CreatedAt = new DateTime(2026, 6, 25)
                }
            );
        }

    }
}
