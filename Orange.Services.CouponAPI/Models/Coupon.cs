using System.ComponentModel.DataAnnotations;


namespace Orange.Services.CouponAPI.Models;

public class Coupon
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string CouponCode { get; set; }
    [Required]
    public double CouponAmount { get; set; }
    
    public int? MinAmount { get; set; }
    
    
    public DateTime? CreatedAt { get; set; }
    
    
}