using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CarRental.Data
{
    public class DbUtils
    {
        static DatabaseContext context;

        public static bool AddUser(POCO.User user)
        {
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

        public static bool AddCar(POCO.Car car)
        {
            context.Cars.Add(new Models.Car()
            {
                Id = car.Id,
                Brand = car.Brand,
                Description = car.Description,
                Horsepower = car.Horsepower,
                Model = car.Model,
                YearOfProduction = car.YearOfProduction
            });
            return context.SaveChanges() == 1;
        }

        public static IEnumerable<POCO.Car> GetCars()
        {
            foreach (Models.Car car in context.Cars)
            {
                yield return (POCO.Car)car;
            }
        }

        public static IEnumerable<POCO.User> GetUsers()
        {
            foreach (Models.User user in context.Users)
            {
                yield return (POCO.User) user;
            }
        }

        internal static void Init(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    context = services.GetRequiredService<DatabaseContext>();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred initialising DbUtils.");
                }
            }
        }
    }
}
