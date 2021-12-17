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
        public IActionResult GetPrice([FromBody]Requests.RentDates dates, string user_id, string car_id)
        {
            var today = DateTime.Now;
            POCO.User user = dbUtils.FindUser(user_id);
            POCO.Car car = dbUtils.FindCar(Guid.Parse(car_id));
            Responses.GetPriceResponse response;

            Requests.GetPriceRequest request = new Requests.GetPriceRequest();
            request.age = today.Year - user.DateOfBirth.Year;
            request.yearsOfHavingDriverLicense = today.Year - user.DriversLicenseDate.Year;
            request.rentDuration = dates.to.DayOfYear - dates.from.DayOfYear;
            request.location = user.Location;
            request.currentlyRentedCount = 0;
            request.overallRentedCount = 0;

            if (car == null || user == null)
            {
                response = APIUtils.GetPrice(request, car_id);
                return Ok(response);
            }
            else
            {
                response = Price.CalculatePrice(request, car);
                POCO.Rental newRental = new POCO.Rental()
                {
                    Id = Guid.NewGuid(),
                    from = dates.from,
                    to = dates.to,
                    price = response.price,
                    currency = response.currency,
                    isConfirmed = false,
                    car = car,
                    user = user
                };
                if (dbUtils.AddRental(newRental))
                {
                    response.quotaId = newRental.Id.ToString();
                    return Ok(response);
                }
            }

            return StatusCode(500);
        }

        [HttpPost("Rent/{quoteId}")]
        public IActionResult RentCar([FromBody]DateTime startDate, string quoteId)
        {
            Guid Id = Guid.Parse(quoteId);
            POCO.Rental rental = dbUtils.FindRental(Id);

            if (rental != null)
            {
                dbUtils.ConfirmRental(Id);
                Responses.RentResponse response = new Responses.RentResponse()
                {
                    quoteId = rental.Id.ToString(),
                    rentId = rental.Id.ToString(),
                    rentAt = DateTime.Now,
                    startDate = rental.from,
                    endDate = rental.to
                };
                return Ok(response);
            }
            else
            {
                Responses.RentResponse response = APIUtils.RentCar(startDate, quoteId);
                return Ok(response);
            }
        }

        [HttpGet]
        public IEnumerable<Car> Get()
        {
            return dbUtils.GetCars().ToArray();
        }
    }
}
