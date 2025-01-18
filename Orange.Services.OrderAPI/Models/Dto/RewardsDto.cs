namespace Orange.Services.OrderAPI.Models.Dto;

public class RewardsDto
{
    public string UserId { get; set; }
    public int RewardPoints { get; set; }
    public string OrderId { get; set; }
    
    public string? Email { get; set; } = string.Empty;
}