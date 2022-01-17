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
            public Guid id { get; set; }
            public string brand { get; set; }
            public string model { get; set; }
            public int year { get; set; }
            public int month { get; set; }
            public int day { get; set; }

            public bool returned { get; set; }
            public string imagename { get; set; }
            public string documentname { get; set; }
            public string note { get; set; }

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

        [HttpGet("details")]
        [ProducesResponseType(200)]
        public IEnumerable<DetailedRental> GetDetailed()
        {
            return _rentalsService.GetDetailed();
        }

        [HttpGet("hist")]
        [ProducesResponseType(200)]
        public IEnumerable<DetailedRental> GetDetPrev()
        {
            return _rentalsService.GetDetPrev();
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

        [HttpPatch("return/{rental_id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(405)]
        public IActionResult ReturnCar([FromBody] DetailedRental rental)
        {
            _rentalsService.ReturnCar(rental.id, rental.imagename, rental.documentname, rental.note);
            return Ok();
        }
    }
}
