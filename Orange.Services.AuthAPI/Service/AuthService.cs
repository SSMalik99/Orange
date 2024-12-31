using Microsoft.AspNetCore.Identity;
using Orange.Services.AuthAPI.Data;
using Orange.Services.AuthAPI.Models;
using Orange.Services.AuthAPI.Models.Dto;
using Orange.Services.AuthAPI.Service.IService;

namespace Orange.Services.AuthAPI.Service;

public class AuthService : IAuthService
{
    
    private readonly AppDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    
    public AuthService(
        AppDbContext dbContext,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IJwtTokenGenerator jwtTokenGenerator
        )
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<string> Register(RegistrationDto registrationDto)
    {
        var existingUser = await _userManager.FindByEmailAsync(registrationDto.Email);
        if (existingUser != null) return "A user with "+registrationDto.Email+" already registered!";
        
        ApplicationUser user = new()
        {
            UserName = registrationDto.Username ?? registrationDto.Email,
            Email = registrationDto.Email,
            FirstName = registrationDto.FirstName,
            LastName = registrationDto.LastName,
            NormalizedEmail = registrationDto.Email.ToUpper(),
            PhoneNumber = registrationDto.PhoneNumber,
            
            
        };

        try
        {
            var result = await _userManager.CreateAsync(user, registrationDto.Password);
            
            return result.Succeeded ? "" : (result.Errors.FirstOrDefault()?.Description ?? "Unknown Error");
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
    {
        var user = _dbContext.ApplicationUsers.FirstOrDefault(u => (u.Email == loginRequestDto.Username || u.UserName == loginRequestDto.Username));

        if (user == null) return new LoginResponseDto(){ User = null, Token = "", Message = "Invalid username"};
        
        var isValidPassword = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
        if (!isValidPassword) return new LoginResponseDto(){ User = null, Token = "", Message = "Invalid password"};

        // Generate token
        var token = _jwtTokenGenerator.GenerateJwtToken(user);
        
        UserDto userDto = new()
        {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber
        };
        
        
        return new LoginResponseDto(){ User = userDto, Token = token, Message = "Logged in Successfully" };


    }

    public async Task<bool> AssignRole(string email, string role)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return false;
        
        var roleExists = _roleManager.RoleExistsAsync(role).Result;
        
        if (!roleExists)
        {
            var newRole = _roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
        }
        
        await _userManager.AddToRoleAsync(user, role);
        return true;
        
    }
    
}