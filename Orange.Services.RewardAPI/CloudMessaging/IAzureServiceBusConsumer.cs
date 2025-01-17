namespace Orange.Services.RewardAPI.CloudMessaging;

public interface IAzureServiceBusConsumer
{
    Task StartConsumingAsync();
    Task StopConsumingAsync();
}