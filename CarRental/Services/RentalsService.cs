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
        void ReturnCar(Guid rentId, string image, string document, string note);
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
                id = rental.Id,
                brand = car.Brand,
                model = car.Model,
                year = date.Year,
                month = date.Month,
                day = date.Day
            };
            return result;
        }

        public static RentalsController.DetailedRental WithDateParts(DateTime date){
            return new RentalsController.DetailedRental()
            {
                year = date.Year,
                month = date.Month,
                day = date.Day
            };
        }

        public IEnumerable<RentalsController.DetailedRental> FilteredDisp(bool now, string user_id)
        {
            var foundUser = (user_id != "" ? dbUtils.FindUserByAuthID(user_id) : null);
            var cars = dbUtils.GetCars();

            var rentals = dbUtils.GetRentals().ToList().Where(r => r.From <= DateTime.Now && r.Returned==false);
            if (!now) rentals = dbUtils.GetRentals().ToList().Where(r => r.Returned);

            var query = from car in cars
                        join rental in rentals on car.Id equals rental.CarId
                        select new { car, rental };
            if (foundUser!=null) query = from car in cars
                                         join rental in rentals on car.Id equals rental.CarId
                                         where rental.UserId == foundUser.Id
                                         select new { car, rental };

            var result = new List<RentalsController.DetailedRental>();
            foreach (var el in query)
            {
                var hist = WithDateParts(el.rental.To);
                hist.id = el.rental.Id;
                hist.brand = el.car.Brand;
                hist.model = el.car.Model;
                hist.returned = el.rental.Returned;
                hist.imagename = el.rental.ImageName;
                hist.documentname = el.rental.DocumentName;
                hist.note = el.rental.Note;
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

        public void ReturnCar(Guid Id, string image, string document, string note)
        {
            //Guid Id = Guid.Parse(rent_id);
            var rental = dbUtils.FindRental(Id);
            
            if (rental == null) throw new BadRequestException("Wrong rental id");
            if (rental.Returned) throw new BadRequestException("Already returned");

            rental.ImageName = image;
            rental.DocumentName = document;
            rental.Note = note;
            rental.Returned = true;

            if (!dbUtils.UpdateRental(Id, rental))
            {
                if (!APIUtils.ReturnCar(Id)) throw new BadRequestException("Return not possible");
            }
        }
    }
}

