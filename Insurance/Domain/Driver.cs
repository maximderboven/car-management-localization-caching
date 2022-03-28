using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Resources;

namespace Insurance.Domain
{
    public class Driver : IValidatableObject
    {
        [Required(ErrorMessageResourceType = typeof (ValidationResources),
            ErrorMessageResourceName = "Required")]
        [StringLength(40,ErrorMessageResourceType = typeof (ValidationResources),
            ErrorMessageResourceName = "StringLength",MinimumLength = 3)]
        [Display (ResourceType = typeof(PropertyResources), Name = "FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof (ValidationResources),
            ErrorMessageResourceName = "Required")]
        [StringLength(40, ErrorMessageResourceType = typeof (ValidationResources),
            ErrorMessageResourceName = "StringLength",MinimumLength = 3)]
        [Display (ResourceType = typeof(PropertyResources), Name = "LastName")]
        public string LastName { get; set; }

        [Display (ResourceType = typeof(PropertyResources), Name = "SocialNumber")]
        [Key] public int SocialNumber { get; set; }
        [Display (ResourceType = typeof(PropertyResources), Name = "DateOfBirth")]
        public DateTime DateOfBirth { get; set; }
        public ICollection<Rental> Rentals { get; set; }

        public Driver()
        {
            Rentals = new List<Rental>();
        }

        public Driver(string firstName, string lastName, DateTime dateOfBirth)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Rentals = new List<Rental>();
        }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new Collection<ValidationResult>();
            if (DateOfBirth > DateTime.Now)
            {
                string errorMessage = ValidationResources.ResourceManager.GetString ("Date_In_The_Past") ?? string.Empty;
                errors.Add(new ValidationResult(errorMessage));
            }
            if (DateOfBirth == DateTime.MinValue)
            {
                string errorMessage = ValidationResources.ResourceManager.GetString ("Date_In_The_Future") ?? string.Empty;
                errors.Add(new ValidationResult(errorMessage));
            }
            return errors;
        }
    }
}