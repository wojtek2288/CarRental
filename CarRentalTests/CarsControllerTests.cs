using CarRental.Controllers;
using CarRental.Data;
using CarRental.Exceptions;
using CarRental.Services;
using Microsoft.AspNetCore.Mvc;
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
    internal class CarsControllerTests
    {
        private CarsController controller;
        private DbUtils dbUtils;
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase(databaseName: "CarRental").Options;
            var context = new DatabaseContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            dbUtils = new(context);
            controller = new(null, new CarsService(context));
        }

        [Test]
        public void PostTest()
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
            IActionResult result = controller.Post(car);
            Assert.IsInstanceOf<StatusCodeResult>(result);

            Assert.AreEqual((result as StatusCodeResult).StatusCode, 200);
        }

        [Test]
        public void GetPriceTest()
        {
            var result = controller.GetPrice(new CarsController.Dates { from = DateTime.Today.AddDays(2), to = DateTime.Today.AddDays(7) },
                                         "googleid",
                                         "de8725ba-e24d-4bea-b3eb-61f459c4b0c3");
            Assert.IsInstanceOf<OkObjectResult>(result);
            var quota = (CarRental.POCO.Quota)(result as OkObjectResult).Value;
            Assert.True(quota.Price > 0);
            var q = dbUtils.FindQuota(quota.Id);
            Assert.True(q.CarId == Guid.Parse("de8725ba-e24d-4bea-b3eb-61f459c4b0c3") &&
                        q.UserId == dbUtils.FindUserByAuthID("googleid").Id &&
                        q.RentDuration == 5);
        }

        [Test]
        public void GetPriceTestFail()
        {
            Assert.Throws<NotFoundException>(() => controller.GetPrice(new CarsController.Dates { from = DateTime.Today.AddDays(2), to = DateTime.Today.AddDays(7) },
                                         "non-existant-id",
                                         "de8725ba-e24d-4bea-b3eb-61f459c4b0c3"));
        }

        [Test]
        public void RentCar()
        {
            var result = controller.GetPrice(new CarRental.Controllers.CarsController.Dates { from = DateTime.Today.AddDays(2), to = DateTime.Today.AddDays(7) },
                                         "googleid",
                                         "de8725ba-e24d-4bea-b3eb-61f459c4b0c3");

            Assert.IsInstanceOf<OkObjectResult>(result);

            var quota = (CarRental.POCO.Quota)(result as OkObjectResult).Value;

            controller.RentCar(DateTime.Today.AddDays(2), quota.Id.ToString());
            CarRental.POCO.Rental rental = null;
            foreach (var r in dbUtils.GetRentals())
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

        [Test]
        public void GetCars()
        {
            var cars = controller.Get();

            Assert.AreEqual(cars.Count(), dbUtils.GetCars().Count());
        }
    }
}
