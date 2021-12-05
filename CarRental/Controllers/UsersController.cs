using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Data;
using CarRental.Models;

namespace CarRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<UsersController> _logger;
        public UsersController(ILogger<UsersController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost("clients")]
        public ActionResult Post([FromBody]User NewUser)
        {
            NewUser.Role = Models.User.UserRole.CLIENT;
            _context.Users.Add(NewUser);
            _context.SaveChanges();
            return StatusCode(200);
        }

        public IEnumerable<User> GetAll()
        {
            IEnumerable<User> Users = _context.Users.ToList();
            return Users;
        }
    }
}
