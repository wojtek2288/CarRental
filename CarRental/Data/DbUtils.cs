using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using CarRental.ForeignAPI;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Data
{
    public class DbUtils
    {
        private DatabaseContext context;

        public bool AddUser(POCO.User user)
        {
            if (UserExists(user)) return false;
            if (user.Id == Guid.Empty) user.Id = Guid.NewGuid();

            context.Users.Add(new Models.User()
            {
                Id = user.Id,
                DriversLicenseDate = user.DriversLicenseDate,
                DateOfBirth = user.DateOfBirth,
                Location = user.Location,
                AuthID = user.AuthID,
                Email = user.Email,
                Role = (Models.User.UserRole) user.Role
            });
            return context.SaveChanges() == 1;
        }

        public POCO.User FindUserByEmail(string email)
        {
            return (from user in context.Users
                where user.Email == email
                select (POCO.User) user).FirstOrDefault();
        }

        public bool RemoveUser(POCO.User user)
        {
            context.Remove(context.Users.Single(u => u.Id == user.Id));
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

        public bool RemoveCar(POCO.Car car)
        {
            context.RemoveRange(from quota in context.Quotas
                where quota.CarId == car.Id
                select quota);
            context.RemoveRange(from rental in context.Rentals
                where rental.CarId == car.Id
                select rental);
            context.Remove(context.Cars.Single(c => c.Id == car.Id));
            return context.SaveChanges() > 0;
        }

        public bool AddRental(POCO.Rental rental)
        {
            if (rental.Id == Guid.Empty) rental.Id = new Guid();
            Models.Rental newRental = new Models.Rental()
            {
                Id = rental.Id,
                From = rental.From,
                To = rental.To,
                Price = rental.Price,
                Currency = rental.Currency,
                UserId = rental.UserId,
                CarId = rental.CarId,
                Returned = rental.Returned,
                ImageName = rental.ImageName,
                DocumentName = rental.DocumentName,
                Note = rental.Note
            };

            context.Rentals.Add(newRental);
            return context.SaveChanges() == 1;
        }

        public IEnumerable<POCO.Rental> GetRentals()
        {
            foreach (Models.Rental rental in context.Rentals)
            {
                yield return (POCO.Rental) rental;
            }
        }

        public bool UpdateRental(Guid Id, POCO.Rental rental)
        {
            var found = context.Rentals.Find(Id);
            if (found != null)
            {
                found.Returned = rental.Returned;
                found.ImageName = rental.ImageName;
                found.DocumentName = rental.DocumentName;
                found.Note = rental.Note;
            }

            return context.SaveChanges() == 1;
        }

        public POCO.Rental FindRental(Guid Id)
        {
            Models.Rental found = context.Rentals.Find(Id);
            if (found == null) return null;
            return (POCO.Rental) found;
        }

        public bool VerifyRental(POCO.Rental rental)
        {
            var overlappingRentals =
                from r in context.Rentals
                where !r.Returned && rental.From.Date <= r.To.Date && rental.To.Date >= r.From.Date &&
                      rental.CarId == r.CarId
                select r;

            if (overlappingRentals.Any() || rental.From.Date < DateTime.Now.Date || rental.From.Date > rental.To.Date)
                return false;
            else
                return true;
        }

        public IEnumerable<POCO.Car> GetCars()
        {
            foreach (Models.Car car in context.Cars)
            {
                POCO.Car _car = (POCO.Car)car;
                _car.Company = "Team A";
                yield return _car;
            }

            foreach (POCO.Car car in APIUtils.GetCars())
            {
                yield return car;
            }
        }

        public POCO.Car FindCar(Guid id)
        {
            Models.Car found = context.Cars.Find(id);

            if (found == null)
            {
                foreach (POCO.Car car in APIUtils.GetCars())
                {
                    if (car.Id == id)
                    {
                        return car;
                    }
                }
            }
            POCO.Car _found = (POCO.Car)found;
            _found.Company = "Team A";
            return _found;
        }

        public POCO.Car FindCarInDatabase(Guid id)
        {
            Models.Car found = context.Cars.Find(id);

            if (found == null) return null;
            return (POCO.Car) found;
        }

        public IEnumerable<POCO.User> GetUsers()
        {
            foreach (Models.User user in context.Users)
            {
                yield return (POCO.User) user;
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

        public POCO.User FindUserByAuthID(string AuthID)
        {
            return (from user in context.Users
                where user.AuthID == AuthID
                select (POCO.User) user).FirstOrDefault();
        }

        public POCO.User FindUser(Guid id)
        {
            var found = context.Users.Find(id);
            if (found == null) return null;
            return (POCO.User) found;
        }

        public POCO.Quota FindQuota(Guid id)
        {
            Models.Quota found = context.Quotas.Find(id);
            if (found == null) return null;
            return (POCO.Quota) found;
        }

        public POCO.Quota AddQuota(POCO.Quota quota)
        {
            if (quota.Id == Guid.Empty) quota.Id = Guid.NewGuid();
            context.Quotas.Add(new Models.Quota()
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
            return quota;
        }

        public DbUtils(DatabaseContext context)
        {
            this.context = context;
        }

        public DbUtils(string appsettingsPath)
        {
            string appsettingsContent = File.ReadAllText(appsettingsPath);
            dynamic settings = JsonConvert.DeserializeObject(appsettingsContent);

            string connectionString = settings.ConnectionStrings.DefaultConnection;

            var contextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseSqlServer(connectionString)
                .Options;
            context = new DatabaseContext(contextOptions);
        }

        public IEnumerable<POCO.Car> GetNewCars()
        {
            return from car in context.Cars
                where car.TimeAdded > DateTime.Now.AddDays(-3)
                select (POCO.Car) car;
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}