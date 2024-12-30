namespace Orange.Services.AuthAPI.Models.Dto;

public class LoginResponseDto
{
    public required UserDto User { get; set; }
    public required string Token { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? Expires { get; set; }
}