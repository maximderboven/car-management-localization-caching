using System.ComponentModel.DataAnnotations;
using Resources;

namespace UI.MVC.Models
{
    public class CarDTO
    {
        [Display(ResourceType = typeof(PropertyResources), Name = "Brand")]
        public string Brand { get; set; }
        public int NumberPlate { get; set; }
        public string Fuel { get; set; }
        public short Seats { get; set; }
        public double Mileage { get; set; }
        public long? Purchaseprice { get; set; }
    }
}