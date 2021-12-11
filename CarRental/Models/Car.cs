using System;

namespace CarRental.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Horsepower { get; set; }
        public int YearOfProduction { get; set; }
        public string Description { get; set; }
        public DateTime TimeAdded { get; set; }
    }
}
