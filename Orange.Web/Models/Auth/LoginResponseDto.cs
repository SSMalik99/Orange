namespace Orange.Web.Models.Auth;

public class LoginResponseDto
{
    public UserDto? User { get; set; }
    public required string Token { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? Expires { get; set; }
    public string? Message { get; set; }
}