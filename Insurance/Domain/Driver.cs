using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Insurance.Domain
{
    public class Driver : IValidatableObject
    {
        [Required(ErrorMessage = "Error: Firstname is required")]
        [StringLength(40,ErrorMessage = "Error: Firstname min. 3 CHAR",MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Error: Lastname is required")]
        [StringLength(40, ErrorMessage = "Error: Lastname min. 3 CHAR",MinimumLength = 3)]
        public string LastName { get; set; }

        [Key] public int SocialNumber { get; set; }
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
                errors.Add(new ValidationResult("Error: Date must be in the past"));
            }
            if (DateOfBirth == DateTime.MinValue)
            {
                errors.Add(new ValidationResult("Error: Date can not be empty"));
            }
            return errors;
        }
    }
}