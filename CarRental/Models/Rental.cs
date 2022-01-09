using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Models
{
    public class Rental
    {
        public Guid Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public double Price { get; set; }
        public string Currency { get; set; }
        public Guid CarId { get; set; }
        public Guid UserId { get; set; }
        public bool Active { get; set; }
        public string ImageName { get; set; }
        public string DocumentName { get; set; }
        public string Note { get; set; }
    }
}
