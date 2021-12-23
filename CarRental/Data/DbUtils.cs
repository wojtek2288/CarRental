using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using CarRental.ForeignAPI;
using System.Linq;

namespace CarRental.Data
{
    public class DbUtils
    {
        private DatabaseContext context;

        public bool AddUser(POCO.User user)
        {
            if (UserExists(user)) return false;

            context.Users.Add(new Models.User()
            {
                Id = user.Id,
                DriversLicenseDate = user.DriversLicenseDate,
                DateOfBirth = user.DateOfBirth,
                Location = user.Location,
                AuthID = user.AuthID,
                Email = user.Email,
                Role = (Models.User.UserRole)user.Role
            });
            return context.SaveChanges() == 1;
        }

        public bool AddCar(POCO.Car car)
        {
            context.Cars.Add(new Models.Car()
            {
                Id = car.Id,
                Brand = car.Brand,
                Description = car.Description,
                Horsepower = car.Horsepower,
                Model = car.Model,
                YearOfProduction = car.YearOfProduction,
                TimeAdded = DateTime.Now
            });
            return context.SaveChanges() == 1;
        }

        public bool AddRental(POCO.Rental rental)
        {
            Models.Rental newRental = new Models.Rental()
            {
                Id = rental.Id,
                From = rental.From,
                To = rental.To,
                Price = rental.Price,
                Currency = rental.Currency,
                UserId = rental.UserId,
                CarId = rental.CarId
            };

            context.Rentals.Add(newRental);
            return context.SaveChanges() == 1;
        }

        public POCO.Rental FindRental(Guid Id)
        {
            Models.Rental found = context.Rentals.Find(Id);
            if (found == null) return null;
            return (POCO.Rental)found;
        }

        public IEnumerable<POCO.Car> GetCars()
        {
            foreach (Models.Car car in context.Cars)
            {
                yield return (POCO.Car)car;
            }
            foreach (POCO.Car car in APIUtils.GetCars())
            {
                yield return car;
            }
        }

        public POCO.Car FindCar(Guid id)
        {
            Models.Car found = context.Cars.Find(id);

            if (found == null) return null;
            return (POCO.Car)found;
        }

        public IEnumerable<POCO.User> GetUsers()
        {
            foreach (Models.User user in context.Users)
            {
                yield return (POCO.User)user;
            }
        }

        public bool UserExists(POCO.User user)
        {
            var matchingUsers =
                from u in context.Users
                where u.AuthID == user.AuthID || u.Email == user.Email
                select u;
            return matchingUsers.Any();
        }

        public POCO.User FindUser(string AuthID)
        {
            return (from user in context.Users
                    where user.AuthID == AuthID
                    select (POCO.User)user).FirstOrDefault();
        }

        public POCO.Quota FindQuota(Guid id)
        {
            Models.Quota found = context.Quotas.Find(id);
            if (found == null) return null;
            return (POCO.Quota)found;
        }

        public POCO.Quota AddQuota(POCO.Quota quota)
        {
            var entry = context.Quotas.Add(new Models.Quota()
            {
                Id = quota.Id,
                Currency = quota.Currency,
                ExpiredAt = quota.ExpiredAt,
                Price = quota.Price,
                CarId = quota.CarId,
                UserId = quota.UserId,
                RentDuration = quota.RentDuration,
            });
            if (context.SaveChanges() != 1) return null;
            return (POCO.Quota)entry.Entity;
        }

        public DbUtils(DatabaseContext context)
        {
            this.context = context;
        }
    }
}
