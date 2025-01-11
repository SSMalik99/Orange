namespace Orange.Services.EmailAPI;

public interface IAzureServiceBusConsumer
{
    Task StartConsumingAsync();
    Task StopConsumingAsync();
}