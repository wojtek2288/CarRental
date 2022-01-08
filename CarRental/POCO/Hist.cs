using System;

namespace CarRental.POCO
{
    public class Hist
    {
        public Guid Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string Note { get; set; }
        public string ImageName { get; set; }
        public string DocumentName { get; set; }
    }
}
