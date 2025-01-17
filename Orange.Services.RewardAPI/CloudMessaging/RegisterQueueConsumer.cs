using Azure.Messaging.ServiceBus;
using Orange.Services.RewardAPI.Services;
using Orange.Services.RewardAPI.Utility;

namespace Orange.Services.RewardAPI.CloudMessaging;

public class RegisterQueueConsumer : IRegisterQueueConsumer
{
    private readonly ServiceBusProcessor _emailCartProcessor;
    private readonly RewardService _rewardService;
    
    public RegisterQueueConsumer( RewardService rewardService )
    {
        var client = new ServiceBusClient(StaticData.AzureQueueConnectionString);
        _emailCartProcessor = client.CreateProcessor(StaticData.AzureRegisterQueueName);
        
        _rewardService = rewardService;
        
    }

    public Task StartConsumingAsync()
    {
        throw new NotImplementedException();
    }

    public Task StopConsumingAsync()
    {
        throw new NotImplementedException();
    }
}