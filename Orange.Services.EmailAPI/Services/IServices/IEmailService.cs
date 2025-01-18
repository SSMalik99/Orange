using Orange.Services.EmailAPI.Models.Dto;
using Orange.Services.EmailAPI.ServiceBusMessages;

namespace Orange.Services.EmailAPI.Services.IServices;

public interface IEmailService
{
    Task<bool> SendEmail(string to, string subject, string body);
    Task SendCartEmail(CartDto cart);
    Task SendRegisterUserEmail(RegisterUserDto registerUserDto);
    
    Task SendOrderCreatedEmailAsync(RewardMessage rewardMessage);

}