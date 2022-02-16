using System.ComponentModel.DataAnnotations;

namespace UI.MVC.Models
{
    public class NewGarageDTO
    {
        [Required(ErrorMessage = "Garage name is required")] 
        [StringLength(50, ErrorMessage = "Error: Name min. 2 CHAR & max. 2 CHAR ", MinimumLength = 2)]
        public string Name { get; set; }
        public string Adress { get; set; }
        public string Telnr { get; set; }
        public int Id { get; set; }
    }
}