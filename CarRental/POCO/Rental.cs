using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.POCO
{
    public class Rental
    {
        public Guid Id { get; set; }
        public DateTime from { get; set; }
        public DateTime to { get; set; }
        public bool isConfirmed { get; set; }
        public double price { get; set; }
        public string currency { get; set; }
        public POCO.Car car { get; set; }
        public POCO.User user { get; set; }

        public static explicit operator Rental(Models.Rental rental)
        {
            return new Rental()
            {
                Id = rental.Id,
                from = rental.from,
                to = rental.to,
                car = (Car)rental.car,
                user = (User)rental.user
            };
        }
    }
}
