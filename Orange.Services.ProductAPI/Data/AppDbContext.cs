using Microsoft.EntityFrameworkCore;
using Orange.Services.ProductAPI.Models;
using Orange.Services.ProductAPI.Models.Dto;

namespace Orange.Services.ProductAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options){}
    
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        SeedCategory(modelBuilder);
        SeedProducts(modelBuilder);
        
    }

    protected void SeedCategory(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(new Category
        {
            Id = 1,
            Name = "Orange",
        });
        
        modelBuilder.Entity<Category>().HasData(new Category
        {
            Id = 2,
            Name = "New",
        });
        modelBuilder.Entity<Category>().HasData(new Category
        {
            Id = 3,
            Name = "Starter",
        });
        
        modelBuilder.Entity<Category>().HasData(new Category
        {
            Id = 4,
            Name = "Main",
        });

        modelBuilder.Entity<Category>().HasData(new Category()
            {
                Id = 5,
                Name = "Appetizer",
            }
        );

    }

    protected void SeedProducts(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 1,
            Name = "Samosa",
            Price = 15,
            Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
            ImageUrl = "https://placehold.co/603x403",
            CategoryId = 5
        });
        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 2,
            Name = "Paneer Tikka",
            Price = 13,
            Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
            ImageUrl = "https://placehold.co/602x402",
            CategoryId = 2
        });
        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 3,
            Name = "Sweet Pie",
            Price = 11,
            Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
            ImageUrl = "https://placehold.co/601x401",
            CategoryId = 3
        });
        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 4,
            Name = "Pav Bhaji",
            Price = 15,
            Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
            ImageUrl = "https://placehold.co/600x400",
            CategoryId = 4
        });
    }
    
    
}