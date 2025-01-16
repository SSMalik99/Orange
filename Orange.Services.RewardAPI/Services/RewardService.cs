

using Microsoft.EntityFrameworkCore;
using Orange.Services.RewardAPI.Data;
using Orange.Services.RewardAPI.Models;
using Orange.Services.RewardAPI.ServiceBusMessages;
using Orange.Services.RewardAPI.Services.IServices;

namespace Orange.Services.RewardAPI.Services;

public class RewardService : IRewardService
{
    
    private readonly DbContextOptions _dbOptions;
    private readonly IRewardService _rewardService;
    

    public RewardService(DbContextOptions<AppDbContext> dbOptions, IRewardService rewardService)
    {
        _dbOptions = dbOptions;
        _rewardService = rewardService;
    }

    public Task UpdateRewards(RewardMessage message)
    {
        throw new NotImplementedException();
        // var reward = new Rewards()
        // {
        //     OrderId = message.OrderId,
        //     UserId = message.UserId,
        //     RewardPoints = message.RewardPoints,
        //     RewardDate = DateTime.Now,
        // };

        //await using var db = new AppDbContext(_dbOptions);
        
    }
}