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

namespace CarRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarsController : ControllerBase
    {
        private readonly ILogger<CarsController> _logger;
        private DbUtils dbUtils;
        public class Dates
        {
            public DateTime from { get; set; }
            public DateTime to { get; set; }
        }

        public CarsController(ILogger<CarsController> logger, DatabaseContext context)
        {
            _logger = logger;
            dbUtils = new DbUtils(context);
        }

        /// <summary>
        /// Adds a new car.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(503)]
        public ActionResult Post([FromBody] Car NewCar)
        {
            if (dbUtils.AddCar(NewCar)) return StatusCode(200);

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
            User user = dbUtils.FindUserByAuthID(user_id);
            if (user == null) return StatusCode(404);

            Guid carId = Guid.Parse(car_id);
            Car car = dbUtils.FindCar(carId);

            TimeSpan rentDuration = dates.to - dates.from;

            if (car == null)
            {
                Quota quota = APIUtils.GetPrice(carId, user, rentDuration);
                if (quota == null) return StatusCode(500);
                quota = dbUtils.AddQuota(quota);
                return Ok(quota);
            }
            else
            {
                (int price, string currency) = Price.CalculatePrice(user, rentDuration, car);

                Quota quota = new Quota()
                {
                    Currency = currency,
                    Price = price,
                    ExpiredAt = DateTime.Now + TimeSpan.FromDays(1),
                    UserId = user.Id,
                    CarId = car.Id,
                    RentDuration = (int)rentDuration.TotalDays
                };

                quota = dbUtils.AddQuota(quota);

                if (quota == null) return StatusCode(500);

                return Ok(quota);
            }
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
            Guid Id = Guid.Parse(quotaId);
            Quota quota = dbUtils.FindQuota(Id);
            Rental rental;

            if (dbUtils.FindCar(quota.CarId) != null)
            {
                rental = new Rental()
                {
                    CarId = quota.CarId,
                    UserId = quota.UserId,
                    Currency = quota.Currency,
                    Price = quota.Price,
                    From = startDate,
                    To = startDate.AddDays(quota.RentDuration),
                };
            }
            else
            {
                Guid rentalId = APIUtils.RentCar(startDate, Id);
                if (rentalId == Guid.Empty) return StatusCode(500);
                rental = new Rental()
                {
                    Id = rentalId,
                    CarId = quota.CarId,
                    UserId = quota.UserId,
                    Currency = quota.Currency,
                    From = startDate,
                    To = startDate.AddDays(quota.RentDuration),
                    Price = quota.Price
                };
            }

            if(dbUtils.VerifyRental(rental))
            {
                if (dbUtils.AddRental(rental))
                {
                    new EmailSender(dbUtils).SendRentalEmail(rental);
                    return Ok();
                }
                else
                {
                    return StatusCode(500);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Gets all cars.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(200)]
        public IEnumerable<Car> Get()
        {
            return dbUtils.GetCars().ToArray();
        }
    }
}
