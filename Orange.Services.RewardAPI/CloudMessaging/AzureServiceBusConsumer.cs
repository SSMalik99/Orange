using System.Text;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Orange.Services.RewardAPI.ServiceBusMessages;
using Orange.Services.RewardAPI.Services;
using Orange.Services.RewardAPI.Utility;

namespace Orange.Services.RewardAPI.CloudMessaging;

public class AzureServiceBusConsumer : IAzureServiceBusConsumer
{
    private readonly ServiceBusProcessor _rewardsProcessor;
    private readonly RewardService _rewardService;
    
    public AzureServiceBusConsumer( RewardService rewardService )
    {
        var client = new ServiceBusClient(StaticData.AzureQueueConnectionString);
        _rewardsProcessor = client.CreateProcessor(StaticData.AzureOrderCreatedTopicName, StaticData.AzureOrderCreatedRewardsUpdateSubscription);
        _rewardService = rewardService;
        
    }

    public async Task StartConsumingAsync()
    {
        _rewardsProcessor.ProcessMessageAsync += OnRewardsReceived;
        _rewardsProcessor.ProcessErrorAsync += OnErrorOccured;
        await _rewardsProcessor.StartProcessingAsync();
        
    }
    
    private Task OnErrorOccured(ProcessErrorEventArgs arg)
    {
        Console.WriteLine(arg.Exception);
        // Send Email to admin - TODO
        return Task.CompletedTask;
    }

    
    private async Task OnRewardsReceived(ProcessMessageEventArgs arg)
    {
        var message = arg.Message;
        try
        {
            var body = Encoding.UTF8.GetString(message.Body);
            var rewardMessage = JsonConvert.DeserializeObject<RewardMessage>(body);
            
            if (rewardMessage != null) await _rewardService.UpdateRewards(rewardMessage);

            await arg.CompleteMessageAsync( message);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await arg.AbandonMessageAsync( message);
        }
    }

    public async Task StopConsumingAsync()
    {
        await _rewardsProcessor.StopProcessingAsync();
        await _rewardsProcessor.DisposeAsync();
    }
}