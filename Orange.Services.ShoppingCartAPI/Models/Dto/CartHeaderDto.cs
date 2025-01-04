namespace Orange.Services.ShoppingCartAPI.Models.Dto;

public class CartHeaderDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string CouponCode { get; set; }
    public double Discount { get; set; }
    public double CartTotal { get; set; }
}