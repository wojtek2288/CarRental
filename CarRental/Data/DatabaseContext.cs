using Microsoft.EntityFrameworkCore;
using System;

namespace CarRental.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public virtual DbSet<Models.User> Users { get; set; }
        public virtual DbSet<Models.Car> Cars { get; set; }
        public virtual DbSet<Models.Rental> Rentals { get; set; }
        public virtual DbSet<Models.Quota> Quotas { get; set; }

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
                        Id = Guid.Parse("de8725ba-e24d-4bea-b3eb-61f459c4b0c3"),
                        Brand = "Lego",
                        Model = "Custom Model",
                        Description = "Taki fajny kolorowy i szybki",
                        Horsepower = 9999,
                        YearOfProduction = 2040,
                        TimeAdded = new System.DateTime(2021, 12, 1)
                    }
                });

            modelBuilder.Entity<Models.User>()
                .HasData(new Models.User[]
                {
                    new Models.User
                    {
                        Id = Guid.Parse("bbc591e4-eb41-4f8d-a030-1e892393224a"),
                        AuthID = "googleid2",
                        DateOfBirth = System.DateTime.Today,
                        DriversLicenseDate = System.DateTime.MinValue,
                        Email = "user2@website.com",
                        Location = "Warsaw",
                        Role = Models.User.UserRole.ADMINISTRATOR
                    },
                    new Models.User
                    {
                        Id = Guid.Parse("c72d2e73-93b6-4ddb-bf6e-c778dd425e6b"),
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
