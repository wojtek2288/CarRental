using System;

namespace CarRental.POCO
{
    public class Quota
    {
        public Guid Id { get; set; }
        public int Price { get; set; }
        public string Currency { get; set; }
        public DateTime ExpiredAt { get; set; }
        public Guid UserId { get; set; }
        public Guid CarId { get; set; }
        public int RentDuration { get; set; }

        public static explicit operator Quota(Models.Quota quota)
        {
            return new Quota()
            {
                Id = quota.Id,
                Price = quota.Price,
                Currency = quota.Currency,
                ExpiredAt = quota.ExpiredAt,
                CarId = quota.CarId,
                UserId = quota.UserId,
                RentDuration = quota.RentDuration,
            };
        }
    }
}
