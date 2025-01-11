using Orange.Services.EmailAPI.Models.Dto;

namespace Orange.Services.EmailAPI.Services.IServices;

public interface IEmailService
{
    Task SendEmail(string to, string subject, string body);
    Task SendCartEmail(CartDto cart);
    
}