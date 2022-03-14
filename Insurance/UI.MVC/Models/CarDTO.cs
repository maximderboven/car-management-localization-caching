using System.ComponentModel.DataAnnotations;
using Resources;

namespace UI.MVC.Models
{
    public class CarDTO
    {
        [Display(ResourceType = typeof(PropertyResources), Name = "Brand")]
        public string Brand { get; set; }
        [Display(ResourceType = typeof(PropertyResources), Name = "NumberPlate")]
        public int NumberPlate { get; set; }
        [Display(ResourceType = typeof(PropertyResources), Name = "Fuel")]
        public string Fuel { get; set; }
        [Display(ResourceType = typeof(PropertyResources), Name = "Seats")]
        public short Seats { get; set; }
        [Display(ResourceType = typeof(PropertyResources), Name = "Mileage")]
        public double Mileage { get; set; }
        [Display(ResourceType = typeof(PropertyResources), Name = "Purchaseprice")]
        public long? Purchaseprice { get; set; }
    }
}