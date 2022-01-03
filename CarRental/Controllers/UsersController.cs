using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Data;
using CarRental.POCO;
using Microsoft.AspNetCore.Http;
using CarRental.Services;

namespace CarRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private UsersService usersService;
        public UsersController(ILogger<UsersController> logger, DatabaseContext context)
        {
            _logger = logger;
            usersService = new UsersService(context, this);
        }

        /// <summary>
        /// Adds a new user.
        /// </summary>
        /// <response code="200">Success</response>
        [HttpPost("clients")]
        [ProducesResponseType(200)]
        [ProducesResponseType(503)]
        public ActionResult Post([FromBody] User NewUser)
        {
            return usersService.Post(NewUser);
        }
    }
}
