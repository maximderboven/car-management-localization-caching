using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Insurance.Domain;

namespace UI.MVC.Models
{
    public class CarDTO
    {
        public string Brand { get; set; }
        public int NumberPlate { get; set; }
        public string Fuel { get; set; }
        public short Seats { get; set; }
        public double Mileage { get; set; }
        public long? Purchaseprice { get; set; }
    }
}