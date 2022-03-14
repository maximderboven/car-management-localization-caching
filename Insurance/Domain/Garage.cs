using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Resources;

namespace Insurance.Domain
{
    public class Garage
    {
        [Required(ErrorMessageResourceType = typeof (ValidationResources),
            ErrorMessageResourceName = "Required")] public string Name { get; set; }
        [Display (ResourceType = typeof(PropertyResources), Name = "Adress")]
        public string Adress { get; set; }
        [Display (ResourceType = typeof(PropertyResources), Name = "Telnr")]
        public string Telnr { get; set; }
        [Key] public int Id { get; set; }
        
        public ICollection<Car> Cars { get; set; }

        public Garage()
        {
            Cars = new List<Car>();
        }

        public Garage(string name, string adress, string telnr)
        {
            Name = name;
            Adress = adress;
            Telnr = telnr;
            Cars = new List<Car>();
        }
        public Garage(int id,string name, string adress, string telnr)
        {
            Id = id;
            Name = name;
            Adress = adress;
            Telnr = telnr;
            Cars = new List<Car>();
        }
    }
}