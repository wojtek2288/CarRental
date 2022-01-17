using CarRental.Controllers;
using CarRental.Data;
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
    internal class UsersCotrollerTests
    {
        private UsersController controller;
        private DbUtils dbUtils;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase(databaseName: "CarRental").Options;
            var context = new DatabaseContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            controller = new(null, new UsersService(context));
            dbUtils = new(context);
        }

        [Test]
        public void PostTest()
        {
            var user = new CarRental.POCO.User
            {
                AuthID = "randomgoogleid",
                DateOfBirth = new DateTime(2000, 2, 3),
                DriversLicenseDate = new DateTime(2018, 4, 5),
                Email = "randomemail@randomsite.com",
                Location = "Warsaw"
            };
            Assert.IsInstanceOf<OkObjectResult>(controller.Post(user));

            var u = dbUtils.FindUserByAuthID("randomgoogleid");

            Assert.True(user.DateOfBirth == u.DateOfBirth &&
                        user.DriversLicenseDate == u.DriversLicenseDate &&
                        user.Email == u.Email &&
                        user.Location == u.Location &&
                        u.Role == CarRental.POCO.User.UserRole.CLIENT);
        }
    }
}
