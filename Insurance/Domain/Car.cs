using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Resources;

namespace Insurance.Domain
{
    public class Car : IValidatableObject
    {
        [Required(ErrorMessageResourceType = typeof(ValidationResources),
            ErrorMessageResourceName = "Required")]
        [StringLength(20, ErrorMessageResourceType = typeof(ValidationResources),
            ErrorMessageResourceName = "Stringlength", MinimumLength = 2)]
        [Display(ResourceType = typeof(PropertyResources), Name = "Brand")]
        public string Brand { get; set; }

        [Display(ResourceType = typeof(PropertyResources), Name = "NumberPlate")]
        [Key]
        public int NumberPlate { get; set; }

        [Display(ResourceType = typeof(PropertyResources), Name = "Fuel")]
        public Fuel Fuel { get; set; }

        [Range(1, 7, ErrorMessageResourceType = typeof(ValidationResources),
            ErrorMessageResourceName = "Range")]
        [Display(ResourceType = typeof(PropertyResources), Name = "Seats")]
        public short Seats { get; set; }

        [Required(ErrorMessageResourceType = typeof (ValidationResources),
            ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(PropertyResources), Name = "Mileage")]
        public double Mileage { get; set; }

        [Display(ResourceType = typeof(PropertyResources), Name = "Garage")]
        public Garage Garage { get; set; }

        [Display(ResourceType = typeof(PropertyResources), Name = "PurchasePrice")]
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