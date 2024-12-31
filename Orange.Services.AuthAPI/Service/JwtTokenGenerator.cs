using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Orange.Services.AuthAPI.Models;
using Orange.Services.AuthAPI.Service.IService;

namespace Orange.Services.AuthAPI.Service;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    public readonly JwtOptions _jwtOptions;

    public JwtTokenGenerator(IOptions< JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }
    public string GenerateJwtToken(ApplicationUser applicationUser)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, applicationUser.UserName),
            new Claim(ClaimTypes.Email, applicationUser.Email),
            new Claim(ClaimTypes.NameIdentifier, applicationUser.Id),
            new Claim(ClaimTypes.Sid, applicationUser.Id)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Audience = _jwtOptions.Audience,
            Issuer = _jwtOptions.Issuer,
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        
        return jwtTokenHandler.WriteToken(token);

    }
}