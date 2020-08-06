using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SquadEvent.Shared.Validations
{
    public class LongNumberIdAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string id && Regex.IsMatch(id, @"^[0-9]{1,20}$"))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult($"Id value is required unsigned long type.");
        }
    }
}