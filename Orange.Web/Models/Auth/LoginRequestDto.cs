namespace Orange.Web.Models.Auth;

public class LoginRequestDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}