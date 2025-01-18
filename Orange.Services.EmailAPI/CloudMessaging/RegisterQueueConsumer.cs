using Azure.Messaging.ServiceBus;
using Orange.Services.EmailAPI.Services;
using Orange.Services.EmailAPI.Utility;

namespace Orange.Services.EmailAPI.CloudMessaging;

public class RegisterQueueConsumer : IRegisterQueueConsumer
{
    private readonly ServiceBusProcessor _emailCartProcessor;
    private readonly EmailService _emailService;
    private readonly ServiceBusProcessor _rewardProcessor;
    
    public RegisterQueueConsumer( EmailService emailService )
    {
        var client = new ServiceBusClient(StaticData.AzureQueueConnectionString);
        _emailCartProcessor = client.CreateProcessor(StaticData.AzureRegisterQueueName);
        
        
        _emailService = emailService;
        
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