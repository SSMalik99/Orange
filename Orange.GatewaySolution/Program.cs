

using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Orange.GatewaySolution.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOcelot();

builder.AddAppAuthentication();



var app = builder.Build();
await app.UseOcelot();
app.MapGet("/", () => "Hello World!");

app.Run();