namespace Orange.Services.RewardAPI.ServiceBusMessages;

public class RewardMessage
{
    public string UserId { get; set; }
    public int RewardPoints { get; set; }
    public string OrderId { get; set; }
    public string? Email { get; set; } = string.Empty;
}

//OrderCreatedRewardsUpdate