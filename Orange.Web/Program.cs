using Microsoft.AspNetCore.Authentication.Cookies;
using Orange.Web.Services;
using Orange.Web.Services.IService;
using Orange.Web.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

// register the http client for each services
builder.Services.AddHttpClient<ICouponService, CouponService>();
builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddHttpClient<ICartService, CartService>();
builder.Services.AddHttpClient<IOrderService, OrderService>();



// bind the interface to the implementation
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/AccessDenied";
});



SharedDetail.CouponApiBase = builder.Configuration["ServiceUrls:CouponAPI"] ?? throw new NullReferenceException();
SharedDetail.AuthApiBase = builder.Configuration["ServiceUrls:AuthAPI"] ?? throw new NullReferenceException();
SharedDetail.ProductApiBase = builder.Configuration["ServiceUrls:ProductAPI"] ?? throw new NullReferenceException();
SharedDetail.CartApiBase = builder.Configuration["ServiceUrls:CartAPI"] ?? throw new NullReferenceException();
SharedDetail.OrderApiBase = builder.Configuration["ServiceUrls:OrderAPI"] ?? throw new NullReferenceException();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();


app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();