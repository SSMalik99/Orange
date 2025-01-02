﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Orange.Services.ProductAPI.Data;

#nullable disable

namespace Orange.Services.ProductAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("Orange.Services.ProductAPI.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Orange"
                        },
                        new
                        {
                            Id = 2,
                            Name = "New"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Starter"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Main"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Appetizer"
                        });
                });

            modelBuilder.Entity("Orange.Services.ProductAPI.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 5,
                            Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                            ImageUrl = "https://placehold.co/603x403",
                            Name = "Samosa",
                            Price = 15m
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 2,
                            Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                            ImageUrl = "https://placehold.co/602x402",
                            Name = "Paneer Tikka",
                            Price = 13m
                        },
                        new
                        {
                            Id = 3,
                            CategoryId = 3,
                            Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                            ImageUrl = "https://placehold.co/601x401",
                            Name = "Sweet Pie",
                            Price = 11m
                        },
                        new
                        {
                            Id = 4,
                            CategoryId = 4,
                            Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                            ImageUrl = "https://placehold.co/600x400",
                            Name = "Pav Bhaji",
                            Price = 15m
                        });
                });

            modelBuilder.Entity("Orange.Services.ProductAPI.Models.Product", b =>
                {
                    b.HasOne("Orange.Services.ProductAPI.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Orange.Services.ProductAPI.Models.Category", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}