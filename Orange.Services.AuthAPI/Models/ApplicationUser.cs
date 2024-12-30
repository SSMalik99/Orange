using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Orange.Services.AuthAPI.Models;

public class ApplicationUser : IdentityUser
{
    [Required]public required string FirstName { get; set; }
    public string? LastName { get; set; }
    
}