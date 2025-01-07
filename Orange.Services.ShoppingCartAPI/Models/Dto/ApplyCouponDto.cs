namespace Orange.Services.ShoppingCartAPI.Models.Dto;

public class ApplyCouponDto
{
    public Guid UserId { get; set; }
    public string? CouponCode { get; set; }
    

}