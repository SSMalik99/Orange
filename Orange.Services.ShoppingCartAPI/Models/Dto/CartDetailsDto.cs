namespace Orange.Services.ShoppingCartAPI.Models.Dto;

public class CartDetailsDto
{
    
    public Guid CartId { get; set; }
    public Guid CartHeaderId { get; set; }
    public CartHeaderDto? CartHeader { get; set; }
    public int ProductId { get; set; }
    public ProductDto? Product { get; set; }
    public int Quantity { get; set; }
}