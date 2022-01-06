using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.EntityFrameworkCore;
using CarRental.Data;

namespace CarRentalTests
{
    [TestFixture]
    internal class DbUtilsTests
    {
        DbMock dbMock;
        DbUtils dbUtils;

        [SetUp]
        public void Setup()
        {
            dbMock = new();
            dbUtils = new(dbMock.Context);
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

            dbMock.Users.Verify(m => m.Add(It.Is<CarRental.Models.User>(arg => arg.AuthID == user.AuthID &&
                                                                            arg.DateOfBirth == user.DateOfBirth &&
                                                                            arg.DriversLicenseDate == user.DriversLicenseDate &&
                                                                            arg.Email == user.Email &&
                                                                            arg.Location == user.Location &&
                                                                            arg.Role == CarRental.Models.User.UserRole.CLIENT)), Times.Once);
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

            dbMock.Cars.Verify(m => m.Add(It.Is<CarRental.Models.Car>(arg => arg.Brand == car.Brand &&
                                                                          arg.Description == car.Description &&
                                                                          arg.Horsepower == car.Horsepower &&
                                                                          arg.Model == car.Model &&
                                                                          arg.YearOfProduction == car.YearOfProduction)), Times.Once);
        }

        [Test]
        public void AddRentalTest()
        {
            var rental = new CarRental.POCO.Rental
            {
                CarId = Guid.NewGuid(),
                Currency = "USD",
                Price = 100,
                From = DateTime.Today.AddDays(2),
                To = DateTime.Today.AddDays(10),
                UserId = Guid.NewGuid()
            };
            dbUtils.AddRental(rental);

            dbMock.Rentals.Verify(m => m.Add(It.Is<CarRental.Models.Rental>(arg => arg.CarId == rental.CarId &&
                                                                                arg.Currency == rental.Currency &&
                                                                                arg.Price == rental.Price &&
                                                                                arg.From == rental.From &&
                                                                                arg.To == rental.To &&
                                                                                arg.UserId == rental.UserId)));
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
            dbUtils.AddQuota(quota);

            dbMock.Quotas.Verify(m => m.Add(It.Is<CarRental.Models.Quota>(arg => arg.CarId == quota.CarId &&
                                                                              arg.Currency == quota.Currency &&
                                                                              arg.ExpiredAt == quota.ExpiredAt &&
                                                                              arg.Price == quota.Price &&
                                                                              arg.RentDuration == quota.RentDuration &&
                                                                              arg.UserId == quota.UserId)));
        }
    }
}
