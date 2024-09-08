using eCommerceWebMVC.Models.DTOs.ProductDTOs.Request;
using eCommerceWebMVC.Services.AuthServices.Implementation;
using eCommerceWebMVC.Services.AuthServices.Interface;
using eCommerceWebMVC.Services.CartServices.Implementation;
using eCommerceWebMVC.Services.CartServices.Interface;
using eCommerceWebMVC.Services.OrderServices.Implementation;
using eCommerceWebMVC.Services.OrderServices.Interface;
using eCommerceWebMVC.Services.ProductServices.Implementaion;
using eCommerceWebMVC.Services.ProductServices.Interface;
using eCommerceWebMVC.Services.SignalServices;
using eCommerceWebMVC.Services.TokenServices.Implementation;
using eCommerceWebMVC.Services.TokenServices.Interface;
using MicroserviceMVC.Common.Enum;
using MicroserviceMVC.Service.CouponServices.Implementaion;
using MicroserviceMVC.Service.CouponServices.Interface;
using MicroserviceMVC.Service.WebServices.Implementation;
using MicroserviceMVC.Service.WebServices.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

//SignalR DI
builder.Services.AddSignalR();

//Service coming from the Microservices
builder.Services.AddHttpClient<ICouponService,CouponService>();
builder.Services.AddScoped<IBaseService,BaseService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IImageUploadService<ProductRequestDto>, ImageUploadService<ProductRequestDto>>();
builder.Services.AddScoped<ITokenProviderService, TokenProviderService>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Auth/Login"; // Path to your login action
                options.AccessDeniedPath = "/Auth/AccessDenied"; // Path to your access denied action
                options.ExpireTimeSpan = TimeSpan.FromHours(10);
            });

//Services to make a call to the microservices
HttpMethodType.CouponAPIBase = builder.Configuration["ServiceUrls:CouponAPI"];
HttpMethodType.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"];
HttpMethodType.ProductAPIBase = builder.Configuration["ServiceUrls:ProductAPI"];
HttpMethodType.CartAPIBase = builder.Configuration["ServiceUrls:ShoppingCartAPI"];
HttpMethodType.OrderAPIBase = builder.Configuration["ServiceUrls:OrderAPI"];


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ProductHub>("/productHub");

app.Run();
