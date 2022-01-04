using CarRental.API;
using CarRental.Controllers;
using CarRental.Data;
using CarRental.Email;
using CarRental.ForeignAPI;
using CarRental.POCO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarRental.Services
{
    public class CarsService
    {
        private DbUtils dbUtils;
        private CarsController carsController;

        public CarsService(DatabaseContext context, CarsController carsController)
        {
            dbUtils = new DbUtils(context);
            this.carsController = carsController;
        }

        public bool AddCar(Car car)
        {
            return dbUtils.AddCar(car);
        }

        public ActionResult GetPrice(CarsController.Dates dates, string user_id, string car_id)
        {
            User user = dbUtils.FindUserByAuthID(user_id);
            if (user == null) return carsController.StatusCode(404);

            Guid carId = Guid.Parse(car_id);
            Car car = dbUtils.FindCar(carId);

            TimeSpan rentDuration = dates.to - dates.from;

            if (car == null)
            {
                Quota quota = APIUtils.GetPrice(carId, user, rentDuration);
                if (quota == null) return carsController.StatusCode(500);
                quota = dbUtils.AddQuota(quota);
                return carsController.Ok(quota);
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

                if (quota == null) return carsController.StatusCode(500);

                return carsController.Ok(quota);
            }
        }

        public IActionResult RentCar(DateTime startDate, string quotaId)
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
                if (rentalId == Guid.Empty) return carsController.StatusCode(500);
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
                    return carsController.Ok();
                }
                else
                {
                    return carsController.StatusCode(500);
                }
            }
            else
            {
                return carsController.BadRequest();
            }
        }

        public IEnumerable<Car> GetCars()
        {
            return dbUtils.GetCars().ToArray();
        }
    }
}

