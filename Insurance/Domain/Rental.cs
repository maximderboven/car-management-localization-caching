using System;
using System.ComponentModel.DataAnnotations;

namespace Insurance.Domain
{
    public class Rental
    {
        //only problem is: 2 times the same not trackable, seen as unique
        public double Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Car Car { get; set; }
        public Driver Driver { get; set; }

        public Rental(double price, DateTime startDate, DateTime endDate, Car car, Driver driver)
        {
            Price = price;
            StartDate = startDate;
            EndDate = endDate;
            Car = car;
            Driver = driver;
        }

        public Rental()
        {
        }
    }
}