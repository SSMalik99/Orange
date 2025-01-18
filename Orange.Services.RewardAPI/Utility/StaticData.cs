
namespace Orange.Services.RewardAPI.Utility;

public static class StaticData
{
    public static string ProductApiBase { get; set; } = string.Empty;
    public static string CouponApiBase { get; set; } = string.Empty;

    public static string AzureQueueConnectionString { get; set; } = string.Empty;
    // public static string AzureEmailCartQueueName { get; set; } = string.Empty;
    // public static string AzureRegisterQueueName { get; set; } = string.Empty;
    
    public static string AzureOrderCreatedTopicName { get; set; } = string.Empty;
    public static string AzureOrderCreatedRewardsUpdateSubscription { get; set; } = string.Empty;
}
