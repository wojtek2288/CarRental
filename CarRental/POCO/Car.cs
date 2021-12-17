using System;

namespace CarRental.POCO
{
    public class Car
    {
        public Guid Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Horsepower { get; set; }
        public int YearOfProduction { get; set; }
        public string Description { get; set; }
        public DateTime TimeAdded { get; set; }

        public static explicit operator Car(Models.Car car)
        {
            return new Car()
            {
                Id = car.Id,
                Brand = car.Brand,
                Description = car.Description,
                Horsepower = car.Horsepower,
                Model = car.Model,
                YearOfProduction = car.YearOfProduction,
                TimeAdded = car.TimeAdded
            };
        }
    }
}
