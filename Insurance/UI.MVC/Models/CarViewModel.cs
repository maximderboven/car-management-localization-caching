using System.ComponentModel.DataAnnotations;
using Insurance.Domain;

namespace UI.MVC.Models
{
    public class CarViewModel
    {
        //required nrml niet nodig aangezien door string length
        [Required(ErrorMessage = "Brand is required")]
        [StringLength(20, ErrorMessage = "Error: Brand min. 2 CHAR & max. 20 CHAR ", MinimumLength = 2)]
        public string Brand { get; set; }
        
        
        [Required(ErrorMessage = "Fuel is required")]
        public Fuel Fuel { get; set; }

        
        //value type dus normaal niet required maar ik wil een error message
        [Required(ErrorMessage = "Seats amount is required")]
        [Range(1, 7, ErrorMessage = "Seats need to be between 1 and 7")]
        public short Seats { get; set; }

        
        [Required(ErrorMessage = "Mileage is required")]
        // Replaced Range with regex because it is now a string, however the error
        // message stays as the only way this regex can fail is if the number
        // entered is too large or smaller than 0.
        [RegularExpression(@"^[0-9]{1,7}[,\.][0-9]{0,2}$", ErrorMessage = "Thats too much, not safe anymore")]
        public string Mileage { get; set; }

        
        public int Garage { get; set; }
        
        
        [Range(1,10000000 , ErrorMessage = "Verdomd dure auto amai")]
        public long? PurchasePrice { get; set; }
    }
}
