using System;

namespace CarRental.POCO
{
    public class User
    {
        public enum UserRole
        {
            CLIENT,
            ADMINISTRATOR
        }
        public int Id { get; set; }
        public DateTime DriversLicenseDate { get; set; } 
        public DateTime DateOfBirth { get; set; }
        public string Location { get; set; }
        public string AuthID { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }

        public static explicit operator User(Models.User user)
        {
            return new User()
            {
                Id = user.Id,
                DriversLicenseDate = user.DriversLicenseDate,
                DateOfBirth = user.DateOfBirth,
                Location = user.Location,
                AuthID = user.AuthID,
                Email = user.Email,
                Role = (UserRole)user.Role
            };
        }
    }
}
