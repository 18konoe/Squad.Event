using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SquadEvent.Shared.Validations
{
    public class BsonIdHexStringsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string id && Regex.IsMatch(id, @"^[0-9a-fA-F]{24}$"))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Required 24 digits hex strings.");
        }
    }
}