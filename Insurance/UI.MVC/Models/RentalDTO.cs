using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Resources;

namespace UI.MVC.Models
{
    public class RentalDTO : IValidatableObject
    {
        [Required(ErrorMessageResourceType = typeof(ValidationResources),
            ErrorMessageResourceName = "Required")]
        [Range(10,10000, ErrorMessageResourceType = typeof (ValidationResources),
            ErrorMessageResourceName = "Range")]
        [Display(ResourceType = typeof(PropertyResources), Name = "Price")]
        public double Price { get; set; }
        [Display(ResourceType = typeof(PropertyResources), Name = "StartDate")]
        public DateTime StartDate { get; set; }
        [Display(ResourceType = typeof(PropertyResources), Name = "EndDate")]
        public DateTime EndDate { get; set; }
        [Display(ResourceType = typeof(PropertyResources), Name = "NumberPlate")]
        public int NumberPlate { get; set; }
        [Display(ResourceType = typeof(PropertyResources), Name = "Socialnumber")]
        public int Socialnumber { get; set; }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new Collection<ValidationResult>();
            if (StartDate < DateTime.Now)
            {
                errors.Add(new ValidationResult(string.Format (ValidationResources.Before, StartDate, EndDate)));
            }
            if (EndDate < StartDate)
            {
                errors.Add(new ValidationResult(string.Format (ValidationResources.After, EndDate, StartDate)));
            }
            return errors;
        }
    }
}