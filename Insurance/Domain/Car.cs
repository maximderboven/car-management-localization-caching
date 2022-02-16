using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Insurance.Domain
{
    public class Car : IValidatableObject
    {
        [Required(ErrorMessage = "Error: Brand is required")]
        [StringLength(20, ErrorMessage = "Error: Brand min. 2 CHAR & max. 20 CHAR ", MinimumLength = 2)]
        public string Brand { get; set; }

        [Key] public int NumberPlate { get; set; }
        public Fuel Fuel { get; set; }

        [Range(1, 7, ErrorMessage = "Error: Seats need to be between 1 and 7")]
        public short Seats { get; set; }

        [Required(ErrorMessage = "Error: Milage is required")]
        public double Mileage { get; set; }

        public Garage Garage { get; set; }
        public long? PurchasePrice { get; set; }

        public ICollection<Rental> Rentals;

        public Car()
        {
            Rentals = new List<Rental>();
        }

        public Car(long? purchasePrice, string brand, Fuel fuel, short seats, double mileage, Garage garage)
        {
            Rentals = new List<Rental>();
            PurchasePrice = purchasePrice;
            Brand = brand;
            Fuel = fuel;
            Seats = seats;
            Mileage = mileage;
            Garage = garage;
        }

        //public INumerableValidationresult Validate(context context)
        //if (Enum.getvalues.Fuel().contains(this.Fuel());
        //Enum.isdefined
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new Collection<ValidationResult>();
            if (!Enum.IsDefined(Fuel))
            {
                errors.Add(new ValidationResult("Error: Fuel not supported"));
            }
            return errors;
        }
    }
}