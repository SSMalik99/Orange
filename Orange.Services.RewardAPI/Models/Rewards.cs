using System.ComponentModel.DataAnnotations;

namespace Orange.Services.RewardAPI.Models;

public class Rewards
{
    
    [Key]
    public int Id { get; set; }
    
    public string UserId { get; set; }
    
    public DateTime RewardDate { get; set; }
    public int RewardPoints { get; set; }
    public string OrderId { get; set; }
}