using CarRental.Data;
using CarRental.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalTests
{
    [TestFixture]
    internal class CarsServiceTests
    {
        CarsService service;
        DbUtils dbUtils;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase(databaseName: "CarRental").Options;
            var context = new DatabaseContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            dbUtils = new(context);
            service = new(context);
        }

        [Test]
        public void AddCarTest()
        {
            var car = new CarRental.POCO.Car
            {
                Id = Guid.NewGuid(),
                Brand = "Volvo",
                Description = "Gitowy",
                Horsepower = 100,
                Model = "Jakis",
                YearOfProduction = 2000
            };
            service.AddCar(car);

            var c = dbUtils.FindCar(car.Id);
            Assert.True(c.Brand == car.Brand &&
                        c.Description == car.Description &&
                        c.Horsepower == car.Horsepower &&
                        c.Model == car.Model &&
                        c.YearOfProduction == car.YearOfProduction);
        }

        [Test]
        public void GetPriceLocalTest()
        {
            var quota = service.GetPrice(new CarRental.Controllers.CarsController.Dates { from = DateTime.Today.AddDays(2), to = DateTime.Today.AddDays(7) },
                                         "googleid",
                                         "de8725ba-e24d-4bea-b3eb-61f459c4b0c3");
            Assert.True(quota.Price > 0);
            var q = dbUtils.FindQuota(quota.Id);
            Assert.True(q.CarId == Guid.Parse("de8725ba-e24d-4bea-b3eb-61f459c4b0c3") &&
                        q.UserId == dbUtils.FindUserByAuthID("googleid").Id &&
                        q.RentDuration == 5);
        }

        [Test]
        public void GetPriceForeignTest()
        {
            string carId = CarRental.ForeignAPI.APIUtils.GetCars().First().Id.ToString();
            var quota = service.GetPrice(new CarRental.Controllers.CarsController.Dates { from = DateTime.Today.AddDays(2), to = DateTime.Today.AddDays(7) },
                                         "googleid",
                                         carId);
            Assert.True(quota.Price > 0);
            var q = dbUtils.FindQuota(quota.Id);
            Assert.True(q.CarId == Guid.Parse(carId) &&
                        q.UserId == dbUtils.FindUserByAuthID("googleid").Id &&
                        q.RentDuration == 5);
        }

        [Test]
        public void RentCarLocalTest()
        {
            var quota = service.GetPrice(new CarRental.Controllers.CarsController.Dates { from = DateTime.Today.AddDays(2), to = DateTime.Today.AddDays(7) },
                                         "googleid",
                                         "de8725ba-e24d-4bea-b3eb-61f459c4b0c3");

            service.RentCar(DateTime.Today.AddDays(2), quota.Id.ToString());
            CarRental.POCO.Rental rental = null;
            foreach(var r in dbUtils.GetRentals())
            {
                if (r.CarId == quota.CarId && r.UserId == quota.UserId)
                {
                    rental = r;
                    break;
                }
            }
            Assert.True(rental.Price == quota.Price &&
                        rental.CarId == quota.CarId &&
                        rental.UserId == quota.UserId &&
                        rental.Currency == quota.Currency);
        }
    }
}
