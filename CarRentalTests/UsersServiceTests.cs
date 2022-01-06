using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.Services;
using Moq;

namespace CarRentalTests
{
    [TestFixture]
    internal class UsersServiceTests
    {
        private DbMock dbMock;
        private UsersService usersService;

        [SetUp]
        public void Setup()
        {
            dbMock = new();
            usersService = new UsersService(dbMock.Context);
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
                Role = CarRental.POCO.User.UserRole.CLIENT
            };

            usersService.Post(user);

            dbMock.Users.Verify(m => m.Add(It.Is<CarRental.Models.User>(arg => arg.AuthID == user.AuthID &&
                                                                            arg.DateOfBirth == user.DateOfBirth &&
                                                                            arg.DriversLicenseDate == user.DriversLicenseDate &&
                                                                            arg.Email == user.Email &&
                                                                            arg.Location == user.Location &&
                                                                            arg.Role == CarRental.Models.User.UserRole.CLIENT)), Times.Once);

        }
    }
}
