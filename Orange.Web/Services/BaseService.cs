
using System.Net;
using System.Text;

using Newtonsoft.Json;
using Orange.Web.Models;
using Orange.Web.Services.IService;

using static Orange.Web.Utility.SharedDetail;

namespace Orange.Web.Services;

public class BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider) : IBaseService
{
    public async Task<ResponseDto> SendAsync(RequestDto requestDto, bool withAuth = true)
    {
        
        HttpClient httpClient = httpClientFactory.CreateClient("OrangeAPI");
        HttpRequestMessage request = new();


        request.Headers.Add("Accept",
            requestDto.ContentType == ContactType.MultipartFormData ? "*/*" : "application/json");

        
        
        if (withAuth)
        {
            request.Headers.Add("Authorization", $"Bearer {tokenProvider.GetToken()}");
        }
        
        request.RequestUri = new Uri(requestDto.Url);
        
        if (requestDto.ContentType == ContactType.MultipartFormData)
        {
            var content = new MultipartFormDataContent();
           
            foreach (var prop in requestDto.Body.GetType().GetProperties())
            {
                var value = prop.GetValue(requestDto.Body);
                
                if (value is FormFile file)
                {
                    content.Add(new StreamContent(file.OpenReadStream()), prop.Name,file.FileName);
                    
                }
                else
                {
                    content.Add(new StringContent((value==null ? "" : value.ToString()) ?? string.Empty), prop.Name);
                }
                
                
            }
            
            request.Content = content;
            
        }else if (requestDto.Body is not null)
        {
            request.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Body), Encoding.UTF8, "application/json");
        }

        request.Method = requestDto.ApiType switch
        {
            ApiType.Post => HttpMethod.Post,
            ApiType.Put => HttpMethod.Put,
            ApiType.Delete => HttpMethod.Delete,
            _ => HttpMethod.Get
        };
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
                    var badResponse = await response.Content.ReadAsStringAsync();
                    var badResponseDto = JsonConvert.DeserializeObject<ResponseDto>(badResponse);
                    return new ResponseDto {IsSuccess = false, Message = badResponseDto?.Message ?? "Bad Request"};
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