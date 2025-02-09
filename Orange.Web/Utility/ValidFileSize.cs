using System.ComponentModel.DataAnnotations;

namespace Orange.Web.Utility;

public class ValidFileSize(long maxFileSize) : ValidationAttribute
{
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not IFormFile file) return ValidationResult.Success;
        return 
            file.Length > (maxFileSize * 1024 * 1024) ?
                new ValidationResult($"File is too large, maximum file allowed size is {maxFileSize} MB.") : 
                ValidationResult.Success;
    }
}