using System;

namespace CarRental.Models
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
    }
}
