using CarRental.Controllers;
using CarRental.Data;
using CarRental.POCO;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Services
{
    public class UsersService
    {
        private DbUtils dbUtils;
        private UsersController usersController;

        public UsersService(DatabaseContext context, UsersController usersController)
        {
            dbUtils = new DbUtils(context);
            this.usersController = usersController;
        }

        public ActionResult Post(User NewUser)
        {
            NewUser.Role = User.UserRole.CLIENT;
            if (dbUtils.AddUser(NewUser)) return usersController.StatusCode(200);

            return usersController.StatusCode(503);
        }
    }
}
