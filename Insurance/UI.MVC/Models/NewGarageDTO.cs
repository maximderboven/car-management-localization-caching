using Resources;
using System.ComponentModel.DataAnnotations;

namespace UI.MVC.Models
{
    public class NewGarageDTO
    {
        [Required(ErrorMessageResourceType = typeof(ValidationResources),
            ErrorMessageResourceName = "Required")] 
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceType = typeof(ValidationResources),
            ErrorMessageResourceName = "StringLength")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display (Name = "Address")]
        public string Adress { get; set; }
        [Display (Name = "TelNr")]
        public string Telnr { get; set; }
        public int Id { get; set; }
    }
}