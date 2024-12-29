using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Orange.Services.CouponAPI.Models;

namespace Orange.Services.CouponAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options){}
    
    public DbSet<Coupon> Coupons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        SeedCouponData(modelBuilder);
        
    }

    private void SeedCouponData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coupon>().HasData(new Coupon
        {
            Id = 1,
            CouponCode = "A123",
            CouponAmount = 12.40,
            MinAmount = 30
        });
        modelBuilder.Entity<Coupon>().HasData(new Coupon
        {
            Id = 2,
            CouponCode = "B123",
            CouponAmount = 5.00,
            MinAmount = 20
        });
    }
}