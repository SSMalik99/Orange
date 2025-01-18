namespace Orange.Services.EmailAPI.ServiceBusMessages;

public class RewardMessage
{
    public string UserId { get; set; } = string.Empty;
    public int RewardPoints { get; set; }
    public string OrderId { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
}

//OrderCreatedRewardsUpdate