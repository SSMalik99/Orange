using System.Text;
using System.Text.Unicode;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Orange.Services.EmailAPI.Models.Dto;
using Orange.Services.EmailAPI.Utility;

namespace Orange.Services.EmailAPI.CloudMessaging;

public class AzureServiceBusConsumer : IAzureServiceBusConsumer
{
    private ServiceBusProcessor _emailCartProcessor;
    private readonly ILogger<AzureServiceBusConsumer> _logger;
    
    public AzureServiceBusConsumer( ILogger<AzureServiceBusConsumer> logger )
    {
        var client = new ServiceBusClient(StaticData.AzureQueueConnectionString);
        _emailCartProcessor = client.CreateProcessor(StaticData.AzureEmailCartQueueName);
        _logger = logger;
        
    }

    public async Task StartConsumingAsync()
    {
        _emailCartProcessor.ProcessMessageAsync += OnEmailCartReceived;
        _emailCartProcessor.ProcessErrorAsync += OnErrorOccured;
        await _emailCartProcessor.StartProcessingAsync();
    }

    private Task OnErrorOccured(ProcessErrorEventArgs arg)
    {
        _logger.LogError( arg.Exception, "Error occured");
        // Send Email to admin - TODO
        return Task.CompletedTask;
    }

    private async Task OnEmailCartReceived(ProcessMessageEventArgs arg)
    {
        var message = arg.Message;
        _logger.LogInformation( $"Received message: {message}");
        var body = Encoding.UTF8.GetString(message.Body);
        
        var cartDto = JsonConvert.DeserializeObject<CartDto>(body);
        try
        {
            _logger.LogInformation( $"Processing cart: {JsonConvert.SerializeObject(cartDto)}");
            
            // Send Email - TODO
            
            await arg.CompleteMessageAsync( message);

        }
        catch (Exception e)
        {
            _logger.LogError( e, "Error occured");
            await arg.AbandonMessageAsync( message);
        }
        
    }

    public async Task StopConsumingAsync()
    {
        await _emailCartProcessor.StopProcessingAsync();
        await _emailCartProcessor.DisposeAsync();
        
    }
}