using System;
using System.ComponentModel.DataAnnotations;
using Resources;

namespace Insurance.Domain
{
    public class Rental
    {
        //only problem is: 2 times the same not trackable, seen as unique
        [Display (ResourceType = typeof(PropertyResources), Name = "Price")]
        public double Price { get; set; }
        [Display (ResourceType = typeof(PropertyResources), Name = "StartDate")]
        public DateTime StartDate { get; set; }
        [Display (ResourceType = typeof(PropertyResources), Name = "EndDate")]
        public DateTime EndDate { get; set; }
        [Display (ResourceType = typeof(PropertyResources), Name = "Car")]
        public Car Car { get; set; }
        [Display (ResourceType = typeof(PropertyResources), Name = "Driver")]
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