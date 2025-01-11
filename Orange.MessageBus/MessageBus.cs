using System.Text;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Orange.MessageBus;

public class MessageBus : IMessageBus
{
    
    private readonly string _azureConnectionString;

    public MessageBus(string connectionString)
    {
       _azureConnectionString = connectionString;
        
    }
    
    
    public async Task PublishMessageAsync(object message, string topiOrQueueName)
    {
        await using var client = new ServiceBusClient(_azureConnectionString);
        var sender = client.CreateSender(topiOrQueueName);
        
        var jsonMessage = JsonConvert.SerializeObject(message);
        
        var finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
        {
            CorrelationId = Guid.NewGuid().ToString() 
        };
        
        await sender.SendMessageAsync(finalMessage);
        //await client.DisposeAsync();
    }

    
    
}



