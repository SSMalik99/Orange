

using Orange.Services.RewardAPI.ServiceBusMessages;

namespace Orange.Services.RewardAPI.Services.IServices;

public interface IRewardService
{
    Task UpdateRewards(RewardMessage message);
    
}