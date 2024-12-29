namespace Orange.Services.CouponAPI.Dto;

public class ResponseDto
{
    public object? Data { get; set; }
    public int StatusCode { get; set; } = 200;
    public string? Message { get; set; } = "";
    public bool IsSuccess => StatusCode is >= 200 and <= 299;
    
}

public class ResponseHelper
{
    public static ResponseDto NotFoundResponseDto(string message = "Resource not found")
    {
        return new ResponseDto
        {
            StatusCode = 404,
            Message = message,
            Data = null
        };
    }
}

