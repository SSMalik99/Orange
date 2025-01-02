using Microsoft.Build.Framework;

namespace Orange.Web.Models.Auth;

public class LoginRequestDto
{
    [Required]
    public required string Username { get; set; }
    [Required]
    public required string Password { get; set; }
}