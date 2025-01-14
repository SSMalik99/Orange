using System.Text.Json.Serialization;

namespace Orange.Services.OrderAPI.Models.Dto;

public class OrderDetailDto
{
    public Guid OrderId { get; set; }
    public string OrderHeaderId { get; set; }
    
    [JsonIgnore]public OrderHeaderDto? OrderHeaderDto { get; set; }
    
    public int ProductId { get; set; }
    
    [JsonIgnore]public ProductDto? Product { get; set; }
    public int Quantity { get; set; }
    public string ProductName { get; set; }
    public double Price { get; set; }
}