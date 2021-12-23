using System;

namespace CarRental.Models
{
    public class Quota
    {
        public Guid Id { get; set; }
        public int Price { get; set; }
        public string Currency { get; set; }
        public DateTime ExpiredAt { get; set; }
        public Guid CarId { get; set; }
        public Guid UserId { get; set; }
        public int RentDuration { get; set; }
    }
}
