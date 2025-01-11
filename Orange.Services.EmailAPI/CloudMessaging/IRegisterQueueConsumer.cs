namespace Orange.Services.EmailAPI.CloudMessaging;

public interface IRegisterQueueConsumer
{
    Task StartConsumingAsync();
    Task StopConsumingAsync();
}