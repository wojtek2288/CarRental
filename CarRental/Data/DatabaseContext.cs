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

            modelBuilder.Entity<Models.Car>()
                .HasData(new Models.Car[]
                {
                    new Models.Car
                    {
                        Id = 1,
                        Brand = "Lego",
                        Model = "Custom Model",
                        Description = "Taki fajny kolorowy i szybki",
                        Horsepower = 9999,
                        YearOfProduction = 2040
                    }
                });

            modelBuilder.Entity<Models.User>()
                .HasData(new Models.User[]
                {
                    new Models.User
                    {
                        Id = 1,
                        AuthID = "googleid2",
                        DateOfBirth = System.DateTime.Today,
                        DriversLicenseDate = System.DateTime.MinValue,
                        Email = "user2@website.com",
                        Location = "Warsaw",
                        Role = Models.User.UserRole.ADMINISTRATOR
                    },
                    new Models.User
                    {
                        Id = 2,
                        AuthID = "googleid",
                        DateOfBirth = System.DateTime.Today,
                        DriversLicenseDate = System.DateTime.MinValue,
                        Email = "user@website.com",
                        Location = "Warsaw",
                        Role = Models.User.UserRole.CLIENT
                    }
                });
        }
    }
}
