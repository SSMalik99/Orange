namespace Orange.Web.Models.Auth;

public class RegistrationDto
{
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Username { get; set; }
    public string? PhoneNumber { get; set; }
    public required string Password { get; set; }
    public required string ConfirmPassword { get; set; }
    
}