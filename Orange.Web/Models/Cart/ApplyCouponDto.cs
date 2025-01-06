namespace Orange.Web.Models.Cart;

public class ApplyCouponDto
{
    public Guid UserId { get; set; }
    public string? CouponCode { get; set; }
}