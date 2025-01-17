namespace Orange.Services.RewardAPI.CloudMessaging;

public interface IRegisterQueueConsumer
{
    Task StartConsumingAsync();
    Task StopConsumingAsync();
}