using Orange.Web.Utility;

namespace Orange.Web.Models;

public class RequestDto
{
    public SharedDetail.ApiType ApiType { get; set; } = SharedDetail.ApiType.Get;
    public string Url { get; set; } = "";
    public object? Body { get; set; } = "";
    public string? AccessToken { get; set; }
    
    public SharedDetail.ContactType ContentType { get; set; } = SharedDetail.ContactType.Json;
    
}