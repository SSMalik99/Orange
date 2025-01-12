using Orange.Web.Models.Product;

namespace Orange.Web.Models.Order;

public class OrderDetailDto
{
    public Guid OrderId { get; set; }
    public string OrderHeaderId { get; set; }
    public int ProductId { get; set; }
    public ProductDto? Product { get; set; }
    public int Quantity { get; set; }
    public string ProductName { get; set; }
    public double Price { get; set; }
}