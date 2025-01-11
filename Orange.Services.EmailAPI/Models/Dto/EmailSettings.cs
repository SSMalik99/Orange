namespace Orange.Services.EmailAPI.Models.Dto;

public class EmailSettings
{
    public string FromAddress { get; set; }
    public string Host  { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int Port { get; set; }
    public bool EnableSsl { get; set; } = true;

}