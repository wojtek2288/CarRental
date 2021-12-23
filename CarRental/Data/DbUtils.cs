using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using CarRental.ForeignAPI;

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

        public Models.Car FindCarModel(Guid Id)
        {
            foreach(var car in context.Cars)
            {
                if (car.Id == Id)
                    return car;
            }
            return null;
        }

        public Models.User FindUserModel(Guid Id)
        {
            foreach (var user in context.Users)
            {
                if (user.Id == Id)
                    return user;
            }
            return null;
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
            foreach(POCO.Car car in APIUtils.GetCars())
            {
                yield return car;
            }
        }

        public POCO.Car FindCar(Guid id)
        {
            foreach(var car in context.Cars)
            {
                if (car.Id == id)
                    return (POCO.Car)car;
            }
            return null;
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
            foreach (Models.User _user in context.Users)
            {
                if (user.AuthID == _user.AuthID || user.Email == _user.Email)
                    return true;
            }
            return false;
        }

        public POCO.User FindUser(string AuthID)
        {
            foreach (Models.User user in context.Users)
            {
                if (user.AuthID == AuthID) return (POCO.User)user;
            }

            return null;
        }

        public POCO.Quota FindQuota(Guid id)
        {
            Models.Quota found = context.Quotas.Find(id);
            if(found == null) return null;
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
            if(context.SaveChanges() != 1) return null;
            return (POCO.Quota)entry.Entity;
        }

        public DbUtils(DatabaseContext context)
        {
            this.context = context;
        }
    }
}
