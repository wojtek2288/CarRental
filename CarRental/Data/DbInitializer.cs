using System.Linq;

namespace CarRental.Data
{
    public class DbInitializer
    {
        public static void Initialize(DatabaseContext context)
        {
            context.Database.EnsureCreated();
            if (context.Users.Any()) return;

            context.Users.Add(new Models.User
            {
                AuthID = "googleid",
                DateOfBirth = System.DateTime.Today,
                DriversLicenseDate = System.DateTime.MinValue,
                Email = "user@website.com",
                Location = "Warsaw",
                Role = Models.User.UserRole.CLIENT
            });

            context.Users.Add(new Models.User
            {
                AuthID = "googleid2",
                DateOfBirth = System.DateTime.Today,
                DriversLicenseDate = System.DateTime.MinValue,
                Email = "user2@website.com",
                Location = "Warsaw",
                Role = Models.User.UserRole.ADMINISTRATOR
            });

            context.Cars.Add(new Models.Car
            {
                Brand="Lego",
                Model="Custom Model",
                Description="Taki fajny kolorowy i szybki",
                Horsepower=9999,
                YearOfProduction=2040
            });

            context.SaveChanges();
        }
    }
}
