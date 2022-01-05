using CarRental.API;
using CarRental.Controllers;
using CarRental.Data;
using CarRental.Email;
using CarRental.Exceptions;
using CarRental.ForeignAPI;
using CarRental.POCO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarRental.Services
{
    public interface ICarsService
    {
        bool AddCar(Car car);
        IEnumerable<Car> GetCars();
        Quota GetPrice(CarsController.Dates dates, string user_id, string car_id);
        void RentCar(DateTime startDate, string quotaId);
    }

    public class CarsService : ICarsService
    {
        private DbUtils dbUtils;

        public CarsService(DatabaseContext context)
        {
            dbUtils = new DbUtils(context);
        }

        public bool AddCar(Car car)
        {
            return dbUtils.AddCar(car);
        }

        public Quota GetPrice(CarsController.Dates dates, string user_id, string car_id)
        {
            User user = dbUtils.FindUserByAuthID(user_id);
            if (user == null) throw new NotFoundException("User not found");

            Guid carId = Guid.Parse(car_id);
            Car car = dbUtils.FindCar(carId);

            TimeSpan rentDuration = dates.to - dates.from;

            if (car == null)
            {
                Quota quota = APIUtils.GetPrice(carId, user, rentDuration);
                if (quota == null) throw new InternalServerErrorException("Internal Server error");
                quota = dbUtils.AddQuota(quota);
                return quota;
            }
            else
            {
                (int price, string currency) = Price.CalculatePrice(user, rentDuration, car);

                Quota quota = new Quota()
                {
                    Currency = currency,
                    Price = price,
                    ExpiredAt = DateTime.Now + TimeSpan.FromDays(1),
                    UserId = user.Id,
                    CarId = car.Id,
                    RentDuration = (int)rentDuration.TotalDays
                };

                quota = dbUtils.AddQuota(quota);

                if (quota == null) throw new InternalServerErrorException("Internal Server error");

                return quota;
            }
        }

        public void RentCar(DateTime startDate, string quotaId)
        {
            Guid Id = Guid.Parse(quotaId);
            Quota quota = dbUtils.FindQuota(Id);
            Rental rental;

            if (dbUtils.FindCar(quota.CarId) != null)
            {
                rental = new Rental()
                {
                    CarId = quota.CarId,
                    UserId = quota.UserId,
                    Currency = quota.Currency,
                    Price = quota.Price,
                    From = startDate,
                    To = startDate.AddDays(quota.RentDuration),
                };
            }
            else
            {
                Guid rentalId = APIUtils.RentCar(startDate, Id);
                if (rentalId == Guid.Empty) throw new InternalServerErrorException("Internal Server error");
                rental = new Rental()
                {
                    Id = rentalId,
                    CarId = quota.CarId,
                    UserId = quota.UserId,
                    Currency = quota.Currency,
                    From = startDate,
                    To = startDate.AddDays(quota.RentDuration),
                    Price = quota.Price
                };
            }

            if (dbUtils.VerifyRental(rental))
            {
                if (dbUtils.AddRental(rental))
                {
                    new EmailSender(dbUtils).SendRentalEmail(rental);
                    return;
                }
                else
                {
                    throw new InternalServerErrorException("Internal Server error");
                }
            }
            else
            {
                throw new BadRequestException("Rental is not possible");
            }
        }

        public IEnumerable<Car> GetCars()
        {
            return dbUtils.GetCars().ToArray();
        }
    }
}

