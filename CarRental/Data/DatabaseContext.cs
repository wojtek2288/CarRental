using Microsoft.EntityFrameworkCore;

namespace CarRental.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.Car> Cars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Car>()
                .HasIndex(c => c.Brand);
            modelBuilder.Entity<Models.Car>()
                .HasIndex(c => c.Model);
            modelBuilder.Entity<Models.Car>()
                .HasIndex(c => c.Horsepower);
            modelBuilder.Entity<Models.Car>()
                .HasIndex(c => c.YearOfProduction);
        }
    }
}
