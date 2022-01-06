using CarRental.Data;
using CarRental.Services;
using Moq;
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
        DbMock dbMock;
        CarsService service;

        [SetUp]
        public void Setup()
        {
            dbMock = new();
            service = new(dbMock.Context);
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
            service.AddCar(car);

            dbMock.Cars.Verify(m => m.Add(It.Is<CarRental.Models.Car>(arg => arg.Brand == car.Brand &&
                                                                             arg.Description == car.Description &&
                                                                             arg.Horsepower == car.Horsepower &&
                                                                             arg.Model == car.Model &&
                                                                             arg.YearOfProduction == car.YearOfProduction)), Times.Once);
        }

        [Test]
        public void GetPriceLocalTest()
        {
            var quota = service.GetPrice(new CarRental.Controllers.CarsController.Dates { from = DateTime.Today.AddDays(2), to = DateTime.Today.AddDays(7) },
                                         "googleid",
                                         "de8725ba-e24d-4bea-b3eb-61f459c4b0c3");
            Assert.True(quota.Price > 0);
            dbMock.Quotas.Verify(m => m.Add(It.IsAny<CarRental.Models.Quota>()), Times.Once);
        }

        [Test]
        public void GetPriceForeignTest()
        {
            string carId = CarRental.ForeignAPI.APIUtils.GetCars().First().Id.ToString();
            var quota = service.GetPrice(new CarRental.Controllers.CarsController.Dates { from = DateTime.Today.AddDays(2), to = DateTime.Today.AddDays(7) },
                                         "googleid",
                                         carId);
            Assert.True(quota.Price > 0);
            dbMock.Quotas.Verify(m => m.Add(It.IsAny<CarRental.Models.Quota>()), Times.Once);
        }

        [Test]
        public void RentCarLocalTest()
        {
            var quota = dbMock.quotaData.First();
            string quotaId = quota.Id.ToString();

            service.RentCar(DateTime.Today.AddDays(2), quotaId);
            dbMock.Rentals.Verify(m => m.Add(It.Is<CarRental.Models.Rental>(arg => arg.Price == quota.Price &&
                                                                                   arg.CarId == quota.CarId &&
                                                                                   arg.UserId == quota.UserId &&
                                                                                   arg.Currency == quota.Currency)));
        }

        [Test]
        public void RentCarForeignTest()
        {
            Guid carId = CarRental.ForeignAPI.APIUtils.GetCars().First().Id;
            var quota = CarRental.ForeignAPI.APIUtils.GetPrice(carId, (CarRental.POCO.User)dbMock.userData.First(), TimeSpan.FromDays(2));

            service.RentCar(DateTime.Today.AddDays(2), quota.Id.ToString());
            dbMock.Rentals.Verify(m => m.Add(It.Is<CarRental.Models.Rental>(arg => arg.Price == quota.Price &&
                                                                                   arg.CarId == quota.CarId &&
                                                                                   arg.UserId == quota.UserId &&
                                                                                   arg.Currency == quota.Currency)));
        }

        [Test]
        public void GetCarsTest()
        {
            int foreignCount = CarRental.ForeignAPI.APIUtils.GetCars().Count();
            Assert.True(service.GetCars().Count() == foreignCount + 1);
        }
    }
}
