

using Microsoft.EntityFrameworkCore;
using Orange.Services.RewardAPI.Data;
using Orange.Services.RewardAPI.Models;
using Orange.Services.RewardAPI.ServiceBusMessages;
using Orange.Services.RewardAPI.Services.IServices;

namespace Orange.Services.RewardAPI.Services;

public class RewardService : IRewardService
{
    
    private DbContextOptions<AppDbContext> _dbOptions;
    

    public RewardService(DbContextOptions<AppDbContext> dbOptions)
    {
        _dbOptions = dbOptions;
    }

    public async Task UpdateRewards(RewardMessage message)
    {
        try
        {
            await using var db = new AppDbContext(_dbOptions);
            var reward = new Rewards()
            {
                OrderId = message.OrderId,
                UserId = message.UserId,
                RewardPoints = message.RewardPoints,
                RewardDate = DateTime.Now,
            };
            
            await db.Rewards.AddAsync(reward);
            await db.SaveChangesAsync();
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }   

        
        
    }
}