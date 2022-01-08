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
    public interface IHistService
    {
        IEnumerable<Hist> FindHistory(bool now, string user_id);
        IEnumerable<Hist> CurrRentedByUser(string user_id);
        IEnumerable<Hist> PrevRentedByUser(string user_id);
        IEnumerable<Hist> CurrRented();
        IEnumerable<Hist> PrevRented();
    }

    public class HistService : IHistService
    {
        private DbUtils dbUtils;
        public HistService(DatabaseContext context)
        {
            dbUtils = new DbUtils(context);
        }

        public static Hist WithDateParts(DateTime date){
            return new Hist()
            {
                Year = date.Year,
                Month = date.Month,
                Day = date.Day
            };
        }

        

        public IEnumerable<Hist> Example()
        {
            var res =  new List<Hist>{
                new Hist()
                {
                    Year = 2022,
                    Month = 1,
                    Day = 2,
                    Id = new Guid(),
                    Brand = "Mercedes-Benz",
                    Model = "AMG",
                    Note = "Heh"
                },
                new Hist()
                {
                    Year = 2022,
                    Month = 1,
                    Day = 2,
                    Id = new Guid(),
                    Brand = "Toyota",
                    Model = "Avensis",
                    Note = "Git"
                },
            };
            return res;
        }
        public IEnumerable<Hist> FindHistory(bool now, string user_id)
        {
            var foundUser = (user_id != "" ? dbUtils.FindUserByAuthID(user_id) : null);
            var cars = dbUtils.GetCars();
            var rentals = dbUtils.GetRentals().ToList().Where(r => now ? r.To > DateTime.Now : r.To <= DateTime.Now);
            var res = from car in cars
                      join rental in rentals on car.Id equals rental.CarId
                      where user_id=="" || (foundUser!=null && rental.UserId==foundUser.Id)
                      select new { car, rental };

            var result = new List<Hist>();
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

        public IEnumerable<Hist> CurrRented()
        {
            return FindHistory(true, "");
        }

        public IEnumerable<Hist> CurrRentedByUser(string user_id)
        {
            return FindHistory(true, user_id);
        }

        public IEnumerable<Hist> PrevRented()
        {
            return FindHistory(false, "");
        }

        public IEnumerable<Hist> PrevRentedByUser(string user_id)
        {
            return FindHistory(false, user_id);
        }
    }
}

