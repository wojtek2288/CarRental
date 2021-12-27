using CarRental.ForeignAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.POCO;

namespace CarRental.API
{
    public static class Price
    {
        public static (int price, string currency) CalculatePrice(User user, TimeSpan rentDuration, Car car)
        {
            return (450 * (int)rentDuration.TotalDays, "PLN");
        }
    }
}
