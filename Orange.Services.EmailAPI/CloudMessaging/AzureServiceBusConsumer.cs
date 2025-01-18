using System.Text;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Orange.Services.EmailAPI.Models.Dto;
using Orange.Services.EmailAPI.ServiceBusMessages;
using Orange.Services.EmailAPI.Services;
using Orange.Services.EmailAPI.Utility;

namespace Orange.Services.EmailAPI.CloudMessaging;

public class AzureServiceBusConsumer : IAzureServiceBusConsumer
{
    private readonly ServiceBusProcessor _emailCartProcessor;
    private readonly ServiceBusProcessor _userRegistrationProcessor;
    private readonly ServiceBusProcessor _rewardProcessor;
    private readonly EmailService _emailService;
    
    public AzureServiceBusConsumer( EmailService emailService )
    {
        var client = new ServiceBusClient(StaticData.AzureQueueConnectionString);
        _emailCartProcessor = client.CreateProcessor(StaticData.AzureEmailCartQueueName);
        _userRegistrationProcessor = client.CreateProcessor(StaticData.AzureRegisterQueueName);
        
        _rewardProcessor = client.CreateProcessor(StaticData.AzureOrderCreatedTopicName,
            StaticData.AzureOrderCreatedEmailSubscription);
        
        _emailService = emailService;
        
    }

    public async Task StartConsumingAsync()
    {
        _emailCartProcessor.ProcessMessageAsync += OnEmailCartReceived;
        _emailCartProcessor.ProcessErrorAsync += OnErrorOccured;
        await _emailCartProcessor.StartProcessingAsync();
        
        _userRegistrationProcessor.ProcessMessageAsync += OnUserRegistrationReceived;
        _userRegistrationProcessor.ProcessErrorAsync += OnErrorOccured;
        await _userRegistrationProcessor.StartProcessingAsync();

        _rewardProcessor.ProcessMessageAsync += OnRewardUpdateReceived;
        _rewardProcessor.ProcessErrorAsync += OnErrorOccured;
        await _rewardProcessor.StartProcessingAsync();
    }

    private async Task OnRewardUpdateReceived(ProcessMessageEventArgs arg)
    {
        var message = arg.Message;
        try
        {
            var body = Encoding.UTF8.GetString(message.Body);
            var rewardMessage = JsonConvert.DeserializeObject<RewardMessage>(body);
            if (rewardMessage != null) await _emailService.SendOrderCreatedEmailAsync(rewardMessage);
            await arg.CompleteMessageAsync( message);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await arg.AbandonMessageAsync( message);
        }
        
    }

    private Task OnErrorOccured(ProcessErrorEventArgs arg)
    {
        Console.WriteLine(arg.Exception);
        // Send Email to admin - TODO
        return Task.CompletedTask;
    }

    private async Task OnEmailCartReceived(ProcessMessageEventArgs arg)
    {
        var message = arg.Message;
        try
        {
            var body = Encoding.UTF8.GetString(message.Body);
            var cartDto = JsonConvert.DeserializeObject<CartDto>(body);
            if (cartDto != null) await _emailService.SendCartEmail(cartDto);
            await arg.CompleteMessageAsync( message);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await arg.AbandonMessageAsync( message);
        }
        
    }
    
    private async Task OnUserRegistrationReceived(ProcessMessageEventArgs arg)
    {
        var message = arg.Message;
        try
        {
            var body = Encoding.UTF8.GetString(message.Body);
            var registerUserDto = JsonConvert.DeserializeObject<RegisterUserDto>(body);
            if (registerUserDto != null) await _emailService.SendRegisterUserEmail(registerUserDto);
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
        await _emailCartProcessor.StopProcessingAsync();
        await _emailCartProcessor.DisposeAsync();
        
        await _userRegistrationProcessor.StopProcessingAsync();
        await _userRegistrationProcessor.DisposeAsync();
        
        await _rewardProcessor.StopProcessingAsync();
        await _rewardProcessor.DisposeAsync();
        
    }
}