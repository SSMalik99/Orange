using Microsoft.AspNetCore.Mvc;

namespace Orange.Services.OrderAPI.Models.Dto;

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

    public static ResponseDto GenerateErrorResponse(string message)
    {
        return new ResponseDto
        {
            IsSuccess = false,
            Message = message ,
            Data = null
        };
        
    }
}

