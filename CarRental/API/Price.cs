using CarRental.ForeignAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API
{
    public static class Price
    {
        public static Responses.GetPriceResponse CalculatePrice(Requests.GetPriceRequest request, POCO.Car car)
        {
            Responses.GetPriceResponse response = new Responses.GetPriceResponse();

            response.currency = "PLN";
            response.price = 450*request.rentDuration;
            response.generatedAt = System.DateTime.Now;
            response.expiredAt = System.DateTime.Now.AddDays(1);

            return response;
        }
    }
}
