using CRN.Product.API.Entities;
using Microsoft.EntityFrameworkCore;
using ProductEntity = CRN.Product.API.Entities.Product;
using UserEntity = CRN.Product.API.Entities.User;

namespace CRN.Product.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProductEntity> Products => Set<ProductEntity>();
        public DbSet<User> Users => Set<User>();
    }
}