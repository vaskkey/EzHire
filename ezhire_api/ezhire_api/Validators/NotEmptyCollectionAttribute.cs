using System.ComponentModel.DataAnnotations;

namespace ezhire_api.Validators;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class NotEmptyCollectionAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // Check for null
        if (value == null) return new ValidationResult($"{validationContext.DisplayName} is required.");

        // Try to treat the value as a collection of strings
        if (value is IEnumerable<string> collection)
        {
            var list = collection.ToList();
            if (!list.Any())
                return new ValidationResult($"{validationContext.DisplayName} must not be an empty collection.");

            // Check each string in the collection
            if (list.Any(s => string.IsNullOrWhiteSpace(s)))
                return new ValidationResult(
                    $"{validationContext.DisplayName} must not contain empty or whitespace strings.");

            return ValidationResult.Success;
        }

        return new ValidationResult($"{validationContext.DisplayName} must be a collection of strings.");
    }
}