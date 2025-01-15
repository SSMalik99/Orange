


using Microsoft.EntityFrameworkCore;
using Orange.Services.RewardAPI.Models;

namespace Orange.Services.RewardAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options){}
    
    public DbSet<Rewards> Rewards { get; set; }
    
    
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     base.OnModelCreating(modelBuilder);
    //     
    // }
    
    
}