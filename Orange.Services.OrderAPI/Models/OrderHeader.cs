using System.ComponentModel.DataAnnotations;
using Orange.Services.OrderAPI.Models.Enum;

namespace Orange.Services.OrderAPI.Models;

public class OrderHeader
{
    [Key]
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string? CouponCode { get; set; }
    public double Discount { get; set; }
    public double OrderTotal { get; set; }
    
    public string? FirstName { get; set; } 
    public string? LastName { get; set; } 
    public string? Email { get; set; } 
    public string? PhoneNumber { get; set; } 
    
    public DateTime OrderTime { get; set; }
    public string Status { get; set; }
    
    public string? PaymentIntentId { get; set; }
    public string? PaymentMethod { get; set; }
    public string? StripeSessionId { get; set; }
    
    public IEnumerable<OrderDetails>? OrderDetails { get; set; }
}

