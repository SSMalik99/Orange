using Microsoft.EntityFrameworkCore;
using Orange.Services.ShoppingCartAPI;
using Orange.Services.ShoppingCartAPI.Data;
using Orange.Services.ShoppingCartAPI.Extensions;
using Orange.Services.ShoppingCartAPI.Utility;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddOpenApi();

StaticData.ProductApiBase = builder.Configuration["ServiceUrls:ProductAPI"];

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