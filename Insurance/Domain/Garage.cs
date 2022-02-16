using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Insurance.Domain
{
    public class Garage
    {
        [Required(ErrorMessage = "Garage name is required")] public string Name { get; set; }
        public string Adress { get; set; }
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