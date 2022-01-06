using CarRental.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalTests
{
    internal class DbMock
    {
        private Mock<DatabaseContext> mockContext;
        private Mock<DbSet<CarRental.Models.User>> mockUsers;
        private Mock<DbSet<CarRental.Models.Car>> mockCars;
        private Mock<DbSet<CarRental.Models.Rental>> mockRentals;
        private Mock<DbSet<CarRental.Models.Quota>> mockQuotas;

        private int changes = 0;

        public DbMock()
        {
            var userData = new List<CarRental.Models.User>() {
                    new CarRental.Models.User
                    {
                        Id = Guid.Parse("bbc591e4-eb41-4f8d-a030-1e892393224a"),
                        AuthID = "googleid2",
                        DateOfBirth = System.DateTime.Today,
                        DriversLicenseDate = System.DateTime.MinValue,
                        Email = "user2@website.com",
                        Location = "Warsaw",
                        Role = CarRental.Models.User.UserRole.ADMINISTRATOR
                    },
                    new CarRental.Models.User
                    {
                        Id = Guid.Parse("c72d2e73-93b6-4ddb-bf6e-c778dd425e6b"),
                        AuthID = "googleid",
                        DateOfBirth = System.DateTime.Today,
                        DriversLicenseDate = System.DateTime.MinValue,
                        Email = "user@website.com",
                        Location = "Warsaw",
                        Role = CarRental.Models.User.UserRole.CLIENT
                    }
            }.AsQueryable();
            mockUsers = new Mock<DbSet<CarRental.Models.User>>();
            mockUsers.Setup(m => m.Add(It.IsAny<CarRental.Models.User>()))
                .Callback(() => changes++);
            mockUsers.As<IQueryable<CarRental.Models.User>>().Setup(m => m.Provider).Returns(userData.Provider);
            mockUsers.As<IQueryable<CarRental.Models.User>>().Setup(m => m.Expression).Returns(userData.Expression);
            mockUsers.As<IQueryable<CarRental.Models.User>>().Setup(m => m.ElementType).Returns(userData.ElementType);
            mockUsers.As<IQueryable<CarRental.Models.User>>().Setup(m => m.GetEnumerator()).Returns(userData.GetEnumerator());

            var carData = new List<CarRental.Models.Car>() {
                    new CarRental.Models.Car
                    {
                        Id = Guid.Parse("de8725ba-e24d-4bea-b3eb-61f459c4b0c3"),
                        Brand = "Lego",
                        Model = "Custom Model",
                        Description = "Taki fajny kolorowy i szybki",
                        Horsepower = 9999,
                        YearOfProduction = 2040,
                        TimeAdded = new System.DateTime(2021, 12, 1)
                    }
            }.AsQueryable();
            mockCars = new Mock<DbSet<CarRental.Models.Car>>();
            mockCars.Setup(m => m.Add(It.IsAny<CarRental.Models.Car>()))
                            .Callback(() => changes++);
            mockCars.As<IQueryable<CarRental.Models.Car>>().Setup(m => m.Provider).Returns(carData.Provider);
            mockCars.As<IQueryable<CarRental.Models.Car>>().Setup(m => m.Expression).Returns(carData.Expression);
            mockCars.As<IQueryable<CarRental.Models.Car>>().Setup(m => m.ElementType).Returns(carData.ElementType);
            mockCars.As<IQueryable<CarRental.Models.Car>>().Setup(m => m.GetEnumerator()).Returns(carData.GetEnumerator());

            var rentalData = new List<CarRental.Models.Rental>()
            {
            }.AsQueryable();
            mockRentals = new Mock<DbSet<CarRental.Models.Rental>>();
            mockRentals.Setup(m => m.Add(It.IsAny<CarRental.Models.Rental>()))
                            .Callback(() => changes++);
            mockRentals.As<IQueryable<CarRental.Models.Rental>>().Setup(m => m.Provider).Returns(rentalData.Provider);
            mockRentals.As<IQueryable<CarRental.Models.Rental>>().Setup(m => m.Expression).Returns(rentalData.Expression);
            mockRentals.As<IQueryable<CarRental.Models.Rental>>().Setup(m => m.ElementType).Returns(rentalData.ElementType);
            mockRentals.As<IQueryable<CarRental.Models.Rental>>().Setup(m => m.GetEnumerator()).Returns(rentalData.GetEnumerator());

            var quotaData = new List<CarRental.Models.Quota>()
            {
            }.AsQueryable();
            mockQuotas = new Mock<DbSet<CarRental.Models.Quota>>();
            mockQuotas.Setup(m => m.Add(It.IsAny<CarRental.Models.Quota>()))
                            .Callback(() => changes++);
            mockQuotas.As<IQueryable<CarRental.Models.Quota>>().Setup(m => m.Provider).Returns(quotaData.Provider);
            mockQuotas.As<IQueryable<CarRental.Models.Quota>>().Setup(m => m.Expression).Returns(quotaData.Expression);
            mockQuotas.As<IQueryable<CarRental.Models.Quota>>().Setup(m => m.ElementType).Returns(quotaData.ElementType);
            mockQuotas.As<IQueryable<CarRental.Models.Quota>>().Setup(m => m.GetEnumerator()).Returns(quotaData.GetEnumerator());


            mockContext = new Mock<DatabaseContext>(new object[] { new DbContextOptions<DatabaseContext>() });
            mockContext.Setup(m => m.Users).Returns(mockUsers.Object);
            mockContext.Setup(m => m.Cars).Returns(mockCars.Object);
            mockContext.Setup(m => m.Rentals).Returns(mockRentals.Object);
            mockContext.Setup(m => m.Quotas).Returns(mockQuotas.Object);
            mockContext.Setup(m => m.SaveChanges()).Returns(() => changes);
        }

        public DatabaseContext Context { get { return mockContext.Object; } }
        public Mock<DbSet<CarRental.Models.User>> Users { get { return mockUsers; } }
        public Mock<DbSet<CarRental.Models.Car>> Cars { get { return mockCars; } }
        public Mock<DbSet<CarRental.Models.Rental>> Rentals { get { return mockRentals; } }
        public Mock<DbSet<CarRental.Models.Quota>> Quotas { get { return mockQuotas; } }
    }
}
