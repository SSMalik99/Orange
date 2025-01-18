using System.Text;
using Microsoft.EntityFrameworkCore;
using Orange.Services.EmailAPI.Data;
using Orange.Services.EmailAPI.Models;
using Orange.Services.EmailAPI.Models.Dto;
using Orange.Services.EmailAPI.Services.IServices;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Orange.Services.EmailAPI.ServiceBusMessages;

namespace Orange.Services.EmailAPI.Services;

public class EmailService : IEmailService
{
    
    private DbContextOptions<AppDbContext> _dbOptions;
    private readonly EmailSettings _emailSettings;

    public EmailService(DbContextOptions<AppDbContext> dbOptions, IOptions< EmailSettings > emailSettings)
    {
        _dbOptions = dbOptions;
        _emailSettings = emailSettings.Value;
    }
    
    public async Task<bool> SendEmail(string to, string subject, string body)
    {
        try
        {
            await using var db = new AppDbContext(_dbOptions);
            
            // Log email
            var emailLogger = new EmailLogger()
            {
                Email = to,
                Message = body,
                CreatedAt = DateTime.Now,
            };
            
            

            // Send email 
            using (var smtpClient = new SmtpClient(_emailSettings.Host, _emailSettings.Port))
            {
                smtpClient.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
                smtpClient.EnableSsl = _emailSettings.EnableSsl;

                var mailMessage = new MailMessage(_emailSettings.FromAddress, to, subject, body)
                {
                    IsBodyHtml = true,
                };
                await smtpClient.SendMailAsync(mailMessage);
            }

            // mark email sent 
            emailLogger.SentAt = DateTime.Now;
            db.EmailLoggers.Add(emailLogger);
            await db.SaveChangesAsync();
            return true;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task SendCartEmail(CartDto cartDto)
    {
        if (cartDto.CartHeader.Email == null)
        {
            return;
        }
        StringBuilder message = new StringBuilder();

        message.AppendLine("<br/>Cart Information from Orange.");
        message.AppendLine("<br/>Total " + cartDto.CartHeader.CartTotal);
        message.Append("<br/>");
        message.Append("<ul>");
        
        foreach (var item in cartDto.CartDetails)
        {
            if (item.Product == null) continue;
            message.Append("<li>");
            message.Append(item.Product?.Name + " x " + item.Quantity + " x " + item.Product?.Price);
            message.Append("</li>");
        }
        message.Append("</ul>");

        
        await SendEmail(
            to:cartDto.CartHeader.Email,
            body: message.ToString(), 
            subject: "Cart Details From Orange");

    }

    public async Task SendRegisterUserEmail(RegisterUserDto registerUserDto)
    {
        if (registerUserDto?.Email == null)
        {
            return;
        }
        StringBuilder message = new StringBuilder();

        message.AppendLine("<br/><h3>User Registration Information from Orange.</h3>");
        message.AppendLine("<div style='font-weight: bold;'><br/>First Name : " + registerUserDto.FirstName);
        message.AppendLine("<br/>Last Name : " + registerUserDto.LastName);
        message.AppendLine("<br/>Email :" + registerUserDto.Email);
        message.Append("<br/></div>");
        
        await SendEmail(to:registerUserDto.Email, body: message.ToString(), subject: "Registration Successful");
    }

    public async  Task SendOrderCreatedEmailAsync(RewardMessage rewardMessage)
    {
        if (rewardMessage.Email == null)
        {
            return;
        }
        
        var message = new StringBuilder();

        message.AppendLine("<br/><h3>Order is placed from Orange.</h3>");
        message.AppendLine("<div><br/>Order ID -" + rewardMessage.OrderId);
        message.AppendLine("<br/> Email - "+rewardMessage.Email);
        message.Append("<br/></div>");
        await SendEmail(to:rewardMessage.Email, body: message.ToString(), subject: "Order Placed Successfully");
    }
}