using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Orange.Web.Models.Product;

namespace Orange.Web.Models.Cart;

public class CartDetailsDto
{
    
    public Guid CartId { get; set; }
    public Guid CartHeaderId { get; set; }
    [JsonIgnore]public CartHeaderDto? CartHeader { get; set; }
    public int ProductId { get; set; }
    public ProductDto? Product { get; set; }
    public int Quantity { get; set; }
}