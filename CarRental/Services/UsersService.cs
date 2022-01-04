using CarRental.Controllers;
using CarRental.Data;
using CarRental.POCO;
using Microsoft.AspNetCore.Mvc;
using CarRental.Exceptions;

namespace CarRental.Services
{
    public interface IUsersService
    {
        void Post(User NewUser);
    }

    public class UsersService : IUsersService
    {
        private DbUtils dbUtils;

        public UsersService(DatabaseContext context)
        {
            dbUtils = new DbUtils(context);
        }

        public void Post(User NewUser)
        {
            NewUser.Role = User.UserRole.CLIENT;
            if (dbUtils.AddUser(NewUser)) return;

            throw new InternalServerErrorException("Internal server error");
        }
    }
}
