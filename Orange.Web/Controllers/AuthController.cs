using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Orange.Web.Services;
using Orange.Web.Models.Auth;
using Orange.Web.Services.IService;
using Orange.Web.Utility;

namespace Orange.Web.Controllers;

public class AuthController:Controller
{
    private readonly IAuthService _authService;
    private readonly ITokenProvider _tokenProvider;
    
    
    public AuthController(IAuthService authService, ITokenProvider tokenProvider)
    {
        _authService = authService;
        _tokenProvider = tokenProvider;
    }

    private List<SelectListItem> RoleList()
    {
        return
        [
            new SelectListItem { Text = SharedDetail.RoleAdmin, Value = SharedDetail.RoleAdmin },
            new SelectListItem { Text = SharedDetail.RoleCustomer, Value = SharedDetail.RoleCustomer }
        ];
    }
    
    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Register()
    {
        ViewBag.RoleList = RoleList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegistrationDto model, string? role)
    {
        if (model.Password != model.ConfirmPassword)
        {
            ModelState.AddModelError("ConfirmPassword", "Passwords don't match");
            TempData["Error"] = "Passwords don't match";
        }
        
        
        if (ModelState.IsValid)
        {
            var roleToAdd = SharedDetail.RoleCustomer;
            if (role is not null)
            {
                roleToAdd = role.ToUpper();
            }
            
            var result = await _authService.RegisterAsync(model);
            Console.WriteLine(result.Message);
            if (result.IsSuccess)
            {
                var assignRole = await _authService.AssignRoleAsync(new AssignRoleReqDto()
                {
                    Email = model.Email,
                    RoleName = roleToAdd
                });

                if (assignRole.IsSuccess)
                {
                    TempData[NotificationType.Success] = "Registration Successful";
                    return RedirectToAction("Login");
                }
                
                TempData[NotificationType.Error] = "Registration Failed";
            }
            else
            {
                TempData[NotificationType.Error] = result.Message;    
            }
            
            
        }
        
        ViewBag.RoleList = RoleList();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginRequestDto model)
    {
        if (ModelState.IsValid)
        {
            var apiResponse = await _authService.LoginAsync(model);
            
            if (apiResponse.IsSuccess)
            {
                var loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(apiResponse.Data));
                TempData[NotificationType.Success] = "Login successful";
                await AuthenticateUser(loginResponse);
                _tokenProvider.SetToken(loginResponse.Token);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("CustomeError", apiResponse.Message);
            }
            
        }
        
        return View(model);
    }
    public IActionResult Logout()
    {
        return View();
    }

    public async Task AuthenticateUser(LoginResponseDto model)
    {
        var handler = new  JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(model.Token);
        var claims = jwt.Claims;
        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        
        
        identity.AddClaim(new Claim(
                JwtRegisteredClaimNames.Sid, 
                claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sid).Value
            )
        );
        identity.AddClaim(new Claim(
                JwtRegisteredClaimNames.Email, 
                claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Email).Value
            )
        );
        identity.AddClaim(new Claim(
                JwtRegisteredClaimNames.Name, 
                claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Name).Value
            )
        );
        
        
        
        identity.AddClaim(new Claim(
                ClaimTypes.Name, 
                claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Email).Value
            )
        );
        
        
        var principle = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle);
    }
    
    
}