
using System.Net;
using System.Text;

using Newtonsoft.Json;
using Orange.Web.Models;
using Orange.Web.Services.IService;
using static Orange.Web.Utility.SharedDetail;

namespace Orange.Web.Services;

public class BaseService(IHttpClientFactory httpClientFactory) : IBaseService
{
    public async Task<ResponseDto> SendAsync(RequestDto requestDto)
    {
        HttpClient httpClient = httpClientFactory.CreateClient("OrangeAPI");
        HttpRequestMessage request = new();
        request.Headers.Add("Accept","application/json");
        request.RequestUri = new Uri(requestDto.Url);

        if (requestDto.Body is not null)
        {
            request.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Body), Encoding.UTF8, "application/json");
        }

        request.Method = requestDto.ApiType switch
        {
            ApiType.Post => HttpMethod.Post,
            ApiType.Put => HttpMethod.Put,
            ApiType.Delete => HttpMethod.Put,
            _ => HttpMethod.Get
        };
        Console.WriteLine(requestDto.Url);
        var response = await httpClient.SendAsync(request);

        try
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    return new ResponseDto {IsSuccess = false, Message = "Not Found"};
                case HttpStatusCode.Unauthorized:
                    return new ResponseDto {IsSuccess = false, Message = "Unauthorized"};
                case HttpStatusCode.BadRequest:
                    return new ResponseDto {IsSuccess = false, Message = "Bad Request"};
                case HttpStatusCode.InternalServerError:
                    return new ResponseDto {IsSuccess = false, Message = "Internal Server Error"};
                case HttpStatusCode.ServiceUnavailable:
                    return new ResponseDto {IsSuccess = false, Message = "Service Unavailable"};
                case HttpStatusCode.Forbidden:
                    return new ResponseDto {IsSuccess = false, Message = "Access Denied"};
                default:
                    var apiContent = await response.Content.ReadAsStringAsync();
                    var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                    return apiResponseDto;
            }

        }
        catch (Exception e)
        {
            return new ResponseDto {IsSuccess = false, Message = e.Message};
        }
        
        
    }
}