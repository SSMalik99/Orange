
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Orange.Services.CouponAPI;
using Orange.Services.CouponAPI.Data;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddOpenApi();

// add automapper
builder.Services.AddSingleton(MappingConfig.RegisterMappings().CreateMapper());
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(name:"v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Orange.Services.CouponAPI",
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnectionDb"));
});
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


