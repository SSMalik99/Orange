using System.Text;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Orange.MessageBus;

public class MessageBus : IMessageBus
{
    
    private AzureSecretModel _azureSecretModel;

    public MessageBus()
    {
        _azureSecretModel = FeedAzureSecrets() ?? throw new ArgumentNullException(nameof(FeedAzureSecrets));
        
    }
    
    
    public async Task PublishMessageAsync(object message, string topiOrQueueName)
    {
        await using var client = new ServiceBusClient(_azureSecretModel.ServiceBusConnectionString);
        var sender = client.CreateSender(topiOrQueueName);
        
        var jsonMessage = JsonConvert.SerializeObject(message);
        
        var finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
        {
            CorrelationId = Guid.NewGuid().ToString() 
        };
        
        await sender.SendMessageAsync(finalMessage);
        //await client.DisposeAsync();
    }

    static AzureSecretModel? FeedAzureSecrets()
    {
        using var r = new StreamReader("Azure.Secret.json");
        var secretDataString = r.ReadToEnd();
        Console.WriteLine(secretDataString);
        
        var secrets = JsonSerializer.Deserialize<AzureSecretModel>(secretDataString);
        return secrets;
    }
    
    
}



