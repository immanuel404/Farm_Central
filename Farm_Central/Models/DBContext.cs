using Microsoft.EntityFrameworkCore;

namespace Farm_Central.Models
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions options) : base(options)
        {}
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            // Populate->USERS
            builder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "John",
                    Surname = "Doe",
                    Email = "john@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("testing@1"),
                    Role = "Employee"
                },
                new User
                {
                    Id = 2,
                    Name = "Jane",
                    Surname = "Doe",
                    Email = "jane@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("testing@1"),
                    Role = "Farmer"
                }
            );

            // Pre-Populate->PRODUCTS
            builder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    UserId = 2,
                    ProductName = "Banana",
                    ProductQty = 50,
                    Price = 50,
                    ProductType = "Fruit"
                },
                new Product
                {
                    Id = 2,
                    UserId = 2,
                    ProductName = "Pears",
                    Price = 55,
                    ProductQty = 85,
                    ProductType = "Fruit"
                },
                new Product
                {
                    Id = 3,
                    UserId = 2,
                    ProductName = "Wheat",
                    ProductQty = 20,
                    Price = 120,
                    ProductType = "Grain"
                },
                new Product
                {
                    Id = 4,
                    UserId = 2,
                    ProductName = "Apples",
                    ProductQty = 50,
                    Price = 40,
                    ProductType = "Fruit"
                },
                new Product
                {
                    Id = 5,
                    UserId = 2,
                    ProductName = "Spinach",
                    ProductQty = 35,
                    Price = 80,
                    ProductType = "Vegetable"
                }
            );
        }
    }
}