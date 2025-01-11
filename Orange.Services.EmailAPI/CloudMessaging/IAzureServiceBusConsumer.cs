namespace Orange.Services.EmailAPI.CloudMessaging;

public interface IAzureServiceBusConsumer
{
    Task StartConsumingAsync();
    Task StopConsumingAsync();
}