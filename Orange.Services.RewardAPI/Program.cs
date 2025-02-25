using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Orange.Services.RewardAPI;
using Orange.Services.RewardAPI.CloudMessaging;
using Orange.Services.RewardAPI.Data;
using Orange.Services.RewardAPI.Extensions;
using Orange.Services.RewardAPI.Services;
using Orange.Services.RewardAPI.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

StaticData.ProductApiBase = builder.Configuration["ServiceUrls:ProductAPI"] ?? throw new InvalidOperationException();
StaticData.CouponApiBase = builder.Configuration["ServiceUrls:CouponAPI"] ?? throw new InvalidOperationException();

builder.Configuration.AddJsonFile("Azure.secret.json", optional: false, reloadOnChange: false);

StaticData.AzureQueueConnectionString = builder.Configuration["serviceBusConnectionString"] ?? throw new InvalidOperationException();
StaticData.AzureOrderCreatedTopicName = builder.Configuration["TopicAndQueueName:OrderCreatedTopicName"] ?? throw new InvalidOperationException();
StaticData.AzureOrderCreatedRewardsUpdateSubscription = builder.Configuration["TopicAndQueueName:OrderCreatedRewardsUpdateSubscription"] ?? throw new InvalidOperationException();

// add automapper
builder.Services.AddSingleton(MappingConfig.RegisterMappings().CreateMapper());
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.AddSwaggerConfig();


// Add DB
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnectionDb"));
});

var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionBuilder.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnectionDb"));

builder.Services.AddSingleton<RewardService>(provider => new RewardService(optionBuilder.Options));

builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();



// Authentication and Authorization
builder.AddAppAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    // Add Swagger
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}


app.UseAzureServiceBusConsumer();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
ApplyMigrations();

app.Run();
return;

void ApplyMigrations()
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (db.Database.GetPendingMigrations().Any())
        {
            db.Database.Migrate();
        }    
    }
    
}