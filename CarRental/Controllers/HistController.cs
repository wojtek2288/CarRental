using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using CarRental.POCO;
using CarRental.Services;

namespace CarRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HistController : ControllerBase
    {
        private readonly ILogger<HistController> _logger;
        private IHistService _histService;
        
        public HistController(ILogger<HistController> logger, IHistService histService)
        {
            _logger = logger;
            _histService = histService;
        }

        /// <summary>
        /// Returns currently rented cars
        /// </summary>
        [HttpGet("Renting")]
        [ProducesResponseType(200)]
        public IEnumerable<Hist> AllRenting()
        {
            return _histService.CurrRented();
        }

        /// <summary>
        /// Returns previously rented cars
        /// </summary>
        [HttpGet("Rented")]
        [ProducesResponseType(200)]
        public IEnumerable<Hist> AllRented()
        {
            return _histService.PrevRented();
        }

        /// <summary>
        /// Returns currently rented cars by user
        /// </summary>
        [HttpGet("Renting/{user_id}")]
        [ProducesResponseType(200)]
        public IEnumerable<Hist> RentingByUser(string user_id)
        {
            return _histService.CurrRentedByUser(user_id);
        }


        /// <summary>
        /// Returns previously rented cars by user
        /// </summary>
        [HttpGet("Rented/{user_id}")]
        [ProducesResponseType(200)]
        public IEnumerable<Hist> RentedByUser(string user_id)
        {
            return _histService.PrevRentedByUser(user_id);
        }
    }
}
