using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Data;
using CarRental.POCO;

namespace CarRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private DbUtils dbUtils;
        private readonly ILogger<UsersController> _logger;
        public UsersController(ILogger<UsersController> logger, DatabaseContext context)
        {
            _logger = logger;
            dbUtils = new DbUtils(context);
        }

        [HttpPost("clients")]
        public ActionResult Post([FromBody] User NewUser)
        {
            NewUser.Role = POCO.User.UserRole.CLIENT;
            if (dbUtils.AddUser(NewUser)) return StatusCode(200);

            return StatusCode(500);
        }
    }
}
