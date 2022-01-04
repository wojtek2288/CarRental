using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Data;
using CarRental.POCO;
using CarRental.ForeignAPI;
using CarRental.API;
using Newtonsoft.Json.Linq;
using CarRental.Email;
using Microsoft.AspNetCore.Http;
using CarRental.AzureFiles;
using CarRental.Services;

namespace CarRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarsController : ControllerBase
    {
        private readonly ILogger<CarsController> _logger;
        private ICarsService _carsService;
        public class Dates
        {
            public DateTime from { get; set; }
            public DateTime to { get; set; }
        }

        public CarsController(ILogger<CarsController> logger, ICarsService carService)
        {
            _logger = logger;
            _carsService = carService;
        }

        /// <summary>
        /// Adds a new car.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(503)]
        public ActionResult Post([FromBody] Car NewCar)
        {
            if (_carsService.AddCar(NewCar)) return StatusCode(200);

            return StatusCode(503);
        }

        /// <summary>
        /// Checks price of rental.
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="404">User not found</response>
        [HttpPost("GetPrice/{user_id}/{car_id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetPrice([FromBody] Dates dates, [FromRoute]string user_id, [FromRoute]string car_id)
        {
            Quota quota = _carsService.GetPrice(dates, user_id, car_id);
            return Ok(quota);
        }

        /// <summary>
        /// Rents a car.
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="404">Rent dates are not valid</response>
        [HttpPost("Rent/{quotaId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult RentCar([FromBody] DateTime startDate, string quotaId)
        {
            _carsService.RentCar(startDate, quotaId);
            return Ok();
        }

        /// <summary>
        /// Gets all cars.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(200)]
        public IEnumerable<Car> Get()
        {
            return _carsService.GetCars();
        }
    }
}
