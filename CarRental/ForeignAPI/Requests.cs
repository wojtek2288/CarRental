using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.ForeignAPI
{
    public class Requests
    {
        public class GetPriceRequest
        {
            public int age { get; set; }
            public int yearsOfHavingDriverLicense { get; set; }
            public int rentDuration { get; set; }
            public string location { get; set; }
            public int currentlyRentedCount { get; set; }
            public int overallRentedCount { get; set; }
        }

        public class RentDates
        {
            public DateTime from { get; set; }
            public DateTime to { get; set; }
        }
    }
}
