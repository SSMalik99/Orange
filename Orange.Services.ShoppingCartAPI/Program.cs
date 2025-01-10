using Microsoft.EntityFrameworkCore;
using Orange.MessageBus;
using Orange.Services.ShoppingCartAPI;
using Orange.Services.ShoppingCartAPI.Data;
using Orange.Services.ShoppingCartAPI.Extensions;
using Orange.Services.ShoppingCartAPI.Services;
using Orange.Services.ShoppingCartAPI.Services.IServices;
using Orange.Services.ShoppingCartAPI.Utility;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddOpenApi();

StaticData.ProductApiBase = builder.Configuration["ServiceUrls:ProductAPI"] ?? throw new InvalidOperationException();
StaticData.CouponApiBase = builder.Configuration["ServiceUrls:CouponAPI"] ?? throw new InvalidOperationException();

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ApiAuthHttpClientHandler>();
//builder.Services.AddHttpClient<IProductService, ProductService>().AddHttpMessageHandler<ApiAuthHttpClientHandler>();
//builder.Services.AddHttpClient<ICouponService, CouponService>().AddHttpMessageHandler<ApiAuthHttpClientHandler>();


builder.Services.AddHttpClient("ProductAPI", client => 
    client.BaseAddress = new Uri(StaticData.ProductApiBase)
).AddHttpMessageHandler<ApiAuthHttpClientHandler>();

builder.Services.AddHttpClient("CouponAPI", client => 
    client.BaseAddress = new Uri(StaticData.CouponApiBase)
).AddHttpMessageHandler<ApiAuthHttpClientHandler>();

// add automapper
builder.Services.AddSingleton(MappingConfig.RegisterMappings().CreateMapper());
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICouponService, CouponService>();

builder.Services.AddScoped<IMessageBus, MessageBus>();
// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.AddSwaggerConfig();

// Add DB
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnectionDb"));
});



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