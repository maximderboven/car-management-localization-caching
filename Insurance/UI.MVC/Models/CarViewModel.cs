using System.ComponentModel.DataAnnotations;
using Insurance.Domain;

namespace UI.MVC.Models
{
    public class CarViewModel
    {
        //required nrml niet nodig aangezien door string length
        [Required(ErrorMessageResourceType = typeof(Resources.ValidationResources),
            ErrorMessageResourceName = "Required")]
        [Display(Name = "Brand")]
        [StringLength(20, ErrorMessage = "Error: Brand min. 2 CHAR & max. 20 CHAR ", MinimumLength = 2)]
        public string Brand { get; set; }
        
        
        [Required(ErrorMessageResourceType = typeof(Resources.ValidationResources),
        ErrorMessageResourceName = "Required")]
        [Display(Name = "Fuel")]
        public Fuel Fuel { get; set; }

        
        //value type dus normaal niet required maar ik wil een error message
        [Required(ErrorMessageResourceType = typeof(Resources.ValidationResources),
        ErrorMessageResourceName = "Required")]
        [Range(1, 7, ErrorMessageResourceType = typeof(Resources.ValidationResources),
        ErrorMessageResourceName = "Required")]
        [Display(Name = "Seats")]
        public short Seats { get; set; }

        
        [Required(ErrorMessageResourceType = typeof(Resources.ValidationResources),
        ErrorMessageResourceName = "Required")]
        // Replaced Range with regex because it is now a string, however the error
        // message stays as the only way this regex can fail is if the number
        // entered is too large or smaller than 0.
        [RegularExpression(@"^[0-9]{1,7}[,\.][0-9]{0,2}$",
        ErrorMessageResourceType = typeof(Resources.ValidationResources),
        ErrorMessage = "TooLarge")]
        [Display(Name = "Mileage")]
        public string Mileage { get; set; }

        [Display(Name = "Garage")]
        public int Garage { get; set; }
        
        
        [Range(0, 10000000 , ErrorMessageResourceType = typeof(Resources.ValidationResources),
            ErrorMessageResourceName = "Range")]
        [Display(Name = "PurchasePrice")]
        public long? PurchasePrice { get; set; }
    }
}
