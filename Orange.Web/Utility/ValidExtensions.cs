using System.ComponentModel.DataAnnotations;

namespace Orange.Web.Utility;

public class ValidExtensions(string[] extensions) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not IFormFile file) return ValidationResult.Success;
        
        var extension = Path.GetExtension(file.FileName);
        return !extensions.Contains(extension.ToLower()) ? new ValidationResult($"The file extension '{extension}' is not supported.") : ValidationResult.Success;
    }
}