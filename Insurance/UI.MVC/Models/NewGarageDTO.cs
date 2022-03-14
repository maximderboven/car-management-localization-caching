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
        [Display(ResourceType = typeof(PropertyResources), Name = "Name")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(PropertyResources), Name = "Adress")]
        public string Adress { get; set; }
        [Display(ResourceType = typeof(PropertyResources), Name = "Telnr")]
        public string Telnr { get; set; }
        public int Id { get; set; }
    }
}