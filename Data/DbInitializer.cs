using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using ProductUser = CRN.Product.API.Entities.User;

namespace CRN.Product.API.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            await context.Database.MigrateAsync();

            if (await context.Users.AnyAsync())
                return;

            var user = new ProductUser
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = "Admin"
            };

            context.Users.Add(user);

            await context.SaveChangesAsync();
        }
    }
}