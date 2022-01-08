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
    public class RentalsController : ControllerBase
    {
        private readonly ILogger<RentalsController> _logger;
        private IRentalsService _rentalsService;
        public class DetailedRental
        {
            public Guid Id { get; set; }
            public string Brand { get; set; }
            public string Model { get; set; }
            public int Year { get; set; }
            public int Month { get; set; }
            public int Day { get; set; }

            /* te pola będą razem z bool Active w klasie Rental, 
             * te będzie zmieniał admin*/

            /* TODO note ma zawierać złączone(albo rozbić na kilka) teksty z formularza Admina */
            public string Note { get; set; }

            /* TODO nazwy plików, będą się wyświetlać Userowi w archiwum */
            //public string ImageFile, DocumentFile;

        }

        public RentalsController(ILogger<RentalsController> logger, IRentalsService histService)
        {
            _logger = logger;
            _rentalsService = histService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public IEnumerable<Rental> Get()
        {
            return _rentalsService.GetRentals();
        }

        [HttpGet("hist")]
        [ProducesResponseType(200)]
        public IEnumerable<DetailedRental> GetDetPrev()
        {
            return _rentalsService.GetDetPrev();
        }

        [HttpGet("details")]
        [ProducesResponseType(200)]
        public IEnumerable<DetailedRental> GetDetailed()
        {
            return _rentalsService.GetDetailed();
        }

        [HttpGet("curr")]
        [ProducesResponseType(200)]
        public IEnumerable<DetailedRental> GetDetCurr()
        {
            return _rentalsService.GetDetCurr();
        }
        
        [HttpGet("hist/{user_id}")]
        [ProducesResponseType(200)]
        public IEnumerable<DetailedRental> GetUserPrev(string user_id)
        {
            return _rentalsService.GetUserPrev(user_id);
        }

        [HttpGet("curr/{user_id}")]
        [ProducesResponseType(200)]
        public IEnumerable<DetailedRental> GetUserCurr(string user_id)
        {
            return _rentalsService.GetUserCurr(user_id);
        }
    }
}
