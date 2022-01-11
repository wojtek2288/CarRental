using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CarRental.Data;

namespace CarRentalTests
{
    [TestFixture]
    internal class DbUtilsTests
    {
        DbUtils dbUtils;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase(databaseName: "CarRental").Options;
            var context = new DatabaseContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            dbUtils = new(context);
        }

        [Test]
        public void AddUserTest()
        {
            var user = new CarRental.POCO.User
            {
                AuthID = "randomgoogleid",
                DateOfBirth = new DateTime(2000, 2, 3),
                DriversLicenseDate = new DateTime(2018, 4, 5),
                Email = "randomemail@randomsite.com",
                Location = "Warsaw",
                Role = CarRental.POCO.User.UserRole.CLIENT
            };
            dbUtils.AddUser(user);

            user = dbUtils.FindUserByAuthID("randomgoogleid");
            Assert.True(user.Email == "randomemail@randomsite.com");
        }

        [Test]
        public void AddCarTest()
        {
            var car = new CarRental.POCO.Car
            {
                Brand = "Volvo",
                Description = "Gitowy",
                Horsepower = 100,
                Model = "Jakis",
                YearOfProduction = 2000
            };
            dbUtils.AddCar(car);

            foreach (var c in dbUtils.GetNewCars())
            {
                if (c.Brand == car.Brand &&
                   c.Description == car.Description &&
                   c.Horsepower == car.Horsepower &&
                   c.Model == car.Model &&
                   c.YearOfProduction == car.YearOfProduction) Assert.Pass();
            }

            Assert.Fail();
        }

        [Test]
        public void AddRentalTest()
        {
            var rental = new CarRental.POCO.Rental
            {
                Id = Guid.NewGuid(),
                CarId = Guid.NewGuid(),
                Currency = "USD",
                Price = 100,
                From = DateTime.Today.AddDays(2),
                To = DateTime.Today.AddDays(10),
                UserId = Guid.NewGuid()
            };
            dbUtils.AddRental(rental);

            var r = dbUtils.FindRental(rental.Id);

            Assert.True(r.CarId == rental.CarId &&
                     r.Currency == rental.Currency &&
                     r.Price == rental.Price &&
                     r.From == rental.From &&
                     r.To == rental.To &&
                     r.UserId == rental.UserId);
        }

        [Test]
        public void AddQuotaTest()
        {
            var quota = new CarRental.POCO.Quota
            {
                CarId = Guid.NewGuid(),
                Currency = "USD",
                ExpiredAt = DateTime.Now,
                Price = 100,
                RentDuration = 2,
                UserId = Guid.NewGuid()
            };
            CarRental.POCO.Quota r = dbUtils.AddQuota(quota);

            var q = dbUtils.FindQuota(r.Id);
            Assert.True(q.CarId == quota.CarId &&
                        q.Currency == quota.Currency &&
                        q.ExpiredAt == quota.ExpiredAt &&
                        q.Price == quota.Price &&
                        q.RentDuration == quota.RentDuration &&
                        q.UserId == quota.UserId);
        }
    }
}
