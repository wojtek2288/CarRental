using CarRental.ForeignAPI;
using CarRental.POCO;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalTests
{
    internal class RentCarAPIUtilsTests
    {
        private RentCarAPIUtils utils;
        [SetUp]
        public void Setup()
        {
            utils = new();
        }

        [Test]
        public void GetCarsUniqueIdsTest()
        {
            HashSet<Guid> idSet = new();
            var cars = utils.GetCars();
            foreach (Car car in cars)
            {
                if (idSet.Contains(car.Id)) Assert.Fail();
                idSet.Add(car.Id);
            }
            Assert.True(idSet.Count > 1);
        }

        [Test]
        public void GetPriceTest()
        {
            Car car = utils.GetCars().First();

            User user = new User
            {
                Id = Guid.Parse("bbc591e4-eb41-4f8d-a030-1e892393224a"),
                AuthID = "googleid2",
                DateOfBirth = new DateTime(2000, 09, 20),
                DriversLicenseDate = new DateTime(2018, 10, 10),
                Email = "user2@website.com",
                Location = "Warsaw",
                Role = User.UserRole.CLIENT
            };

            Quota quota = utils.GetPrice(car.Id, user, new TimeSpan(10, 0, 0, 0));
            Assert.IsTrue(quota.Id != Guid.Empty);
            Assert.IsTrue(quota.Price > 0);
        }

        [Test]
        public void RentCarAndReturnTest()
        {
            Car car = utils.GetCars().First();

            User user = new User
            {
                Id = Guid.Parse("bbc591e4-eb41-4f8d-a030-1e892393224a"),
                AuthID = "googleid2",
                DateOfBirth = new DateTime(2000, 09, 20),
                DriversLicenseDate = new DateTime(2018, 10, 10),
                Email = "user2@website.com",
                Location = "Warsaw",
                Role = User.UserRole.CLIENT
            };

            Quota quota = utils.GetPrice(car.Id, user, new TimeSpan(10, 0, 0, 0));
            Guid rentId = utils.RentCar(DateTime.Today, quota.Id);
            Assert.IsTrue(rentId != Guid.Empty);

            Assert.IsTrue(utils.ReturnCar(rentId));
        }
    }
}
