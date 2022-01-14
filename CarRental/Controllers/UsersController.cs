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
using CarRental.Attributes;

namespace CarRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiKey]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private IUsersService _usersService;
        public UsersController(ILogger<UsersController> logger, IUsersService userService)
        {
            _logger = logger;
            _usersService = userService;
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
            _usersService.Post(NewUser);
            return Ok(NewUser.AuthID);
        }
    }
}
