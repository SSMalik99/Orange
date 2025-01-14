
namespace Orange.Web.Models.Order;

public class OrderHeaderDto
{
    public Guid OrderHeaderId { get; set; }
    public string UserId { get; set; }
    public string? CouponCode { get; set; }
    public double Discount { get; set; }
    public double OrderTotal { get; set; }
    
    public string? FirstName { get; set; } 
    public string? LastName { get; set; } 
    public string? Email { get; set; } 
    public string? PhoneNumber { get; set; } 
    
    public DateTime OrderTime { get; set; }
    public string Status { get; set; } = OrderStatus.Pending;
    
    public string? PaymentIntentId { get; set; }
    public string? PaymentMethod { get; set; }
    public string? StripeSessionId { get; set; }
    
    public IEnumerable<OrderDetailDto>? OrderDetails { get; set; }
}