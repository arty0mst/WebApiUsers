using Microsoft.EntityFrameworkCore;
using Entites;
using Configurations;

namespace DataAccess
{
    public class UserStoreDbContext : DbContext
    {
        public UserStoreDbContext(DbContextOptions<UserStoreDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }

        public DbSet<UserEntity> Users { get; set; }
    }
}