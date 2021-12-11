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
    public class CarsController : ControllerBase
    {
        private readonly ILogger<CarsController> _logger;
        private DbUtils dbUtils;
        public CarsController(ILogger<CarsController> logger, DatabaseContext context)
        {
            _logger = logger;
            dbUtils = new DbUtils(context);
        }

        [HttpPost]
        public ActionResult Post([FromBody] Car NewCar)
        {
            if (dbUtils.AddCar(NewCar)) return StatusCode(200);

            return StatusCode(503);
        }
    }
}
