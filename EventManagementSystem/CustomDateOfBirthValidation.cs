namespace EventManagementSystem
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CustomDateOfBirthValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                if (date > DateTime.Now.AddYears(-18))
                {
                    return new ValidationResult(ErrorMessage ?? "You must be at least 18 years old.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
