using CarRental.POCO;
using System;
using System.Collections.Generic;

namespace CarRental.ForeignAPI
{
    public interface IAPIUtils
    {
        public abstract IEnumerable<Car> GetCars();
        public abstract Quota GetPrice(Guid carId, User user, TimeSpan rentDuration);
        public abstract Guid RentCar(DateTime startDate, Guid quotaId);
        public abstract bool ReturnCar(Guid rentId);
    }
}
