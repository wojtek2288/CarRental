using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Models
{
    public class Rental
    {
        public Guid Id { get; set; }
        public DateTime from { get; set; }
        public DateTime to { get; set; }
        public double price { get; set; }
        public string currency { get; set; }
        public bool isConfirmed { get; set; }
        public Guid carId { get; set; }
        public Car car { get; set; }
        public Guid userId { get; set; } 
        public User user { get; set; }
    }
}
