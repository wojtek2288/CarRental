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
    public class CarsController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<CarsController> _logger;
        public CarsController(ILogger<CarsController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public ActionResult Post([FromBody]Car NewCar)
        {
            _context.Cars.Add(NewCar);
            _context.SaveChanges();
            return StatusCode(200);
        }

        public IEnumerable<Car> GetAll()
        {
            IEnumerable<Car> Cars = _context.Cars.ToList();
            return Cars;
        }
    }
}
