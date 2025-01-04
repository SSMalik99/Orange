namespace Orange.Services.ShoppingCartAPI.Models.Dto;

public class CouponDto
{
    public int Id { get; set; }
    public string CouponCode { get; set; }
    public double CouponAmount { get; set; }
    public int MinAmount { get; set; }
}