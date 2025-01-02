using Microsoft.Build.Framework;

namespace Orange.Web.Models.Auth;

public class RegistrationDto
{
    [Required]public required string Email { get; set; }
    [Required]public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Username { get; set; }
    public string? PhoneNumber { get; set; }
    [Required]public required string Password { get; set; }
    [Required]public required string ConfirmPassword { get; set; }
    
}