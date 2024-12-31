using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Orange.Web.Models.Auth;
using Orange.Web.Services.IService;
using Orange.Web.Utility;

namespace Orange.Web.Controllers;

public class AuthController:Controller
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
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

        var roleToAdd = SharedDetail.RoleCustomer;
        if (role is not null)
        {
            roleToAdd = role.ToUpper();
        }

        
        if (ModelState.IsValid)
        {
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
                    Console.WriteLine(assignRole.Message, result.Message);
                    
                    TempData[NotificationType.Success] = "Registration Successful";
                    return RedirectToAction("Login");
                }
                
                TempData[NotificationType.Error] = "Registration Failed";
            }
            
            TempData[NotificationType.Error] = result.Message;
        }
        
        ViewBag.RoleList = RoleList();
        return View(model);
    }

    public IActionResult Logout()
    {
        return View();
    }
    
    
}