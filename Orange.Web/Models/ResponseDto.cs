namespace Orange.Web.Models;


public class ResponseDto
{
    public object? Data { get; set; }
    public string? Message { get; set; } = "";
    public bool IsSuccess { get; set; } = true;

}