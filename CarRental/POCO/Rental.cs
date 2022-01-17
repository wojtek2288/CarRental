using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.POCO
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

        public bool Returned { get; set; }
        public string ImageName { get; set; }
        public string DocumentName { get; set; }
        public string Note { get; set; }

        public static explicit operator Rental(Models.Rental rental)
        {
            return new Rental()
            {
                Id = rental.Id,
                From = rental.From,
                To = rental.To,
                CarId = rental.CarId,
                UserId = rental.UserId,
                Returned = rental.Returned,
                ImageName = rental.ImageName,
                DocumentName = rental.DocumentName,
                Note = rental.Note
            };
        }
    }
}
