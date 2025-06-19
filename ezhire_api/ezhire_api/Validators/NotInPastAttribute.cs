using System.ComponentModel.DataAnnotations;

namespace ezhire_api.Validators;

public class NotInPastAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null) return true; // Covered by [Required]

        DateTime date;
        if (value is DateTime)
            date = (DateTime)value;
        else if (value is DateTime?)
            date = ((DateTime?)value).Value;
        else
            return false;

        // Compare with today's date, ignore time part
        return date.Date >= DateTime.Today;
    }

    public override string FormatErrorMessage(string name)
        => $"{name} must be today or in the future.";
}
