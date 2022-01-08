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
    public interface IRentalsService
    {
        IEnumerable<Rental> GetRentals();
        IEnumerable<RentalsController.DetailedRental> GetDetailed();
        IEnumerable<RentalsController.DetailedRental> GetDetPrev();
        IEnumerable<RentalsController.DetailedRental> GetDetCurr();
        IEnumerable<RentalsController.DetailedRental> GetUserPrev(string user_id);
        IEnumerable<RentalsController.DetailedRental> GetUserCurr(string user_id);
    }

    public class RentalsService : IRentalsService
    {
        private DbUtils dbUtils;
        public RentalsService(DatabaseContext context)
        {
            dbUtils = new DbUtils(context);
        }

        public RentalsController.DetailedRental ForDisplay(Rental rental)
        {
            var date = rental.To;
            var car = dbUtils.FindCar(rental.CarId);
            RentalsController.DetailedRental result = new RentalsController.DetailedRental()
            {
                Id = rental.Id,
                Brand = car.Brand,
                Model = car.Model,
                Year = date.Year,
                Month = date.Month,
                Day = date.Day,
                
                Note = "Overall State:\nGood\n\nDescription:\nCar smells nice :)",
                //ImageFile = "samochodzik.jpg",
                //DocumentFile = "umowa.pdf",
            };
            return result;
        }

        public static RentalsController.DetailedRental WithDateParts(DateTime date){
            return new RentalsController.DetailedRental()
            {
                Year = date.Year,
                Month = date.Month,
                Day = date.Day
            };
        }

        public IEnumerable<RentalsController.DetailedRental> FilteredDisp(bool now, string user_id)
        {
            var foundUser = (user_id != "" ? dbUtils.FindUserByAuthID(user_id) : null);
            var cars = dbUtils.GetCars();
            // poza now jeszcze spr czy aktywny rental
            var rentals = dbUtils.GetRentals().ToList().Where(r => now ? r.To >= DateTime.Now : r.To < DateTime.Now);
            var res = from car in cars
                      join rental in rentals on car.Id equals rental.CarId
                      where user_id=="" || (foundUser!=null && rental.UserId==foundUser.Id)
                      select new { car, rental };

            var result = new List<RentalsController.DetailedRental>();
            foreach (var el in res)
            {
                var hist = WithDateParts(el.rental.To);
                hist.Id = el.rental.Id;
                hist.Brand = el.car.Brand;
                hist.Model = el.car.Model;
                hist.Note = "A note from Worker";
                result.Add(hist);
            }

            return result;
        }

        public IEnumerable<Rental> GetRentals()
        {
            return dbUtils.GetRentals();
        }

        public IEnumerable<RentalsController.DetailedRental> GetDetailed()
        {
            var rentals = dbUtils.GetRentals();
            var result = new List<RentalsController.DetailedRental>();
            foreach (var r in rentals) result.Add( ForDisplay(r));
            return result;
        }

        public IEnumerable<RentalsController.DetailedRental> GetDetPrev()
        {
            return FilteredDisp(false, "");
        }

        public IEnumerable<RentalsController.DetailedRental> GetDetCurr()
        {
            return FilteredDisp(true, "");
        }

        public IEnumerable<RentalsController.DetailedRental> GetUserPrev(string user_id)
        {
            return FilteredDisp(false, user_id);
        }

        public IEnumerable<RentalsController.DetailedRental> GetUserCurr(string user_id)
        {
            return FilteredDisp(true, user_id);
        }
    }
}

