using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.Services;
using Microsoft.EntityFrameworkCore;
using CarRental.Data;

namespace CarRentalTests
{
    [TestFixture]
    internal class UsersServiceTests
    {
        private UsersService usersService;
        private DbUtils dbUtils;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase(databaseName: "CarRental").Options;
            var context = new DatabaseContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            usersService = new UsersService(context);
            dbUtils = new DbUtils(context);
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
                Location = "Warsaw",
            };

            usersService.Post(user);

            var u = dbUtils.FindUserByAuthID("randomgoogleid");

            Assert.True(u.AuthID == user.AuthID &&
                        u.DateOfBirth == user.DateOfBirth &&
                        u.DriversLicenseDate == user.DriversLicenseDate &&
                        u.Email == user.Email &&
                        u.Location == user.Location &&
                        u.Role == CarRental.POCO.User.UserRole.CLIENT);
        }
    }
}
