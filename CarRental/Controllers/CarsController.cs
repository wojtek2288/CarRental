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

        [HttpPost("GetPrice/{user_id}/{car_id}")]
        public IActionResult GetPrice([FromBody] Dictionary<string, DateTime> dates, string user_id, string car_id)
        {
            User user = dbUtils.FindUserByAuthID(user_id);
            if (user == null) return StatusCode(404);

            Guid carId = Guid.Parse(car_id);
            Car car = dbUtils.FindCar(carId);

            TimeSpan rentDuration = dates["to"] - dates["from"];

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

        [HttpPost("Rent/{quotaId}")]
        public IActionResult RentCar([FromBody] DateTime startDate, string quotaId)
        {
            Guid Id = Guid.Parse(quotaId);
            Quota quota = dbUtils.FindQuota(Id);

            if (dbUtils.FindCar(quota.CarId) != null)
            {
                Rental rental = new Rental()
                {
                    CarId = quota.CarId,
                    UserId = quota.UserId,
                    Currency = quota.Currency,
                    Price = quota.Price,
                    From = startDate,
                    To = startDate.AddDays(quota.RentDuration),
                };
                if (dbUtils.AddRental(rental))
                {
                    new EmailSender(dbUtils).SendRentalEmail(rental);
                    return Ok();
                }
                return StatusCode(500);
            }
            else
            {
                Guid rentalId = APIUtils.RentCar(startDate, Id);
                if (rentalId == Guid.Empty) return StatusCode(500);
                Rental rental = new Rental()
                {
                    Id = rentalId,
                    CarId = quota.CarId,
                    UserId = quota.UserId,
                    Currency = quota.Currency,
                    From = startDate,
                    To = startDate.AddDays(quota.RentDuration),
                    Price = quota.Price
                };
                if (dbUtils.AddRental(rental))
                {
                    new EmailSender(dbUtils).SendRentalEmail(rental);
                    return Ok();
                }
                return StatusCode(500);
            }
        }

        [HttpGet]
        public IEnumerable<Car> Get()
        {
            return dbUtils.GetCars().ToArray();
        }
    }
}
