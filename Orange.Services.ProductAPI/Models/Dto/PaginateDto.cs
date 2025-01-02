namespace Orange.Services.ProductAPI.Models.Dto;

public class PaginateDto
{
    public object? Main { get; set; }
    public int CurrentPage { get; set; }
    public int Limit { get; set; } = 20;
}