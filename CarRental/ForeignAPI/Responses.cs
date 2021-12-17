using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.ForeignAPI
{
    public class Responses
    {
        public class GetPriceResponse
        {
            public double price { get; set; }
            public string currency { get; set; }
            public DateTime generatedAt { get; set; }
            public DateTime expiredAt { get; set; }
            public string quotaId { get; set; }
        }

        public class RentResponse
        {
            public string quoteId { get; set; }
            public string rentId { get; set; }
            public DateTime rentAt { get; set; }
            public DateTime startDate { get; set; }
            public DateTime endDate { get; set; }
        }
    }
}
