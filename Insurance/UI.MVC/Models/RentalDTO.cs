using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Insurance.Domain;

namespace UI.MVC.Models
{
    public class RentalDTO : IValidatableObject
    {
        [Required(ErrorMessage = "Error: Brand is required")]
        [Range(10,10000,ErrorMessage = "Error: Price need to be between 10 and 10.000")]
        public double Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberPlate { get; set; }
        public int Socialnumber { get; set; }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new Collection<ValidationResult>();
            if (StartDate < DateTime.Now)
            {
                errors.Add(new ValidationResult("Start date must be in the future"));
            }
            if (EndDate < StartDate)
            {
                errors.Add(new ValidationResult("EndDate must be greater or the same as the startdate"));
            }
            return errors;
        }
    }
}