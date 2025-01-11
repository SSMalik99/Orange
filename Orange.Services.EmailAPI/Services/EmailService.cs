using System.Text;
using Microsoft.EntityFrameworkCore;
using Orange.Services.EmailAPI.Data;
using Orange.Services.EmailAPI.Models.Dto;
using Orange.Services.EmailAPI.Services.IServices;

namespace Orange.Services.EmailAPI.Services;

public class EmailService : IEmailService
{
    
    private DbContextOptions<AppDbContext> _dbOptions;

    public EmailService(DbContextOptions<AppDbContext> dbOptions)
    {
        _dbOptions = dbOptions;
    }
    
    public Task SendEmail(string to, string subject, string body)
    {
        throw new NotImplementedException();
    }

    public Task SendCartEmail(CartDto cartDto)
    {
        StringBuilder message = new StringBuilder();

        message.AppendLine("<br/>Cart Email Requested ");
        message.AppendLine("<br/>Total " + cartDto.CartHeader.CartTotal);
        message.Append("<br/>");
        message.Append("<ul>");
        foreach (var item in cartDto.CartDetails)
        {
            message.Append("<li>");
            message.Append(item.Product.Name + " x " + item.Quantity + " x " + item.Product.Price);
            message.Append("</li>");
        }
        message.Append("</ul>");
        
    }
}