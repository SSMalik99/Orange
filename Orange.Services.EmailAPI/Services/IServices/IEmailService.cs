using Orange.Services.EmailAPI.Models.Dto;

namespace Orange.Services.EmailAPI.Services.IServices;

public interface IEmailService
{
    Task<bool> SendEmail(string to, string subject, string body);
    Task SendCartEmail(CartDto cart);
    Task SendRegisterUserEmail(RegisterUserDto registerUserDto);

}