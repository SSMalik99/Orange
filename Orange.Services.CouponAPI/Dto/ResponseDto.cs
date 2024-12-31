namespace Orange.Services.CouponAPI.Dto;

public class ResponseDto
{
    public object? Data { get; set; }
    public string? Message { get; set; } = "";
    public bool IsSuccess { get; set; } = true;
    
}

public class ResponseHelper
{
    public static ResponseDto NotFoundResponseDto(string message = "Resource not found")
    {
        return new ResponseDto
        {
            IsSuccess = false,
            Message = message,
            Data = null
        };
    }
}

