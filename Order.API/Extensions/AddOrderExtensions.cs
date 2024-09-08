using MessageBus.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OnMapper;
using Order.API.Features.Orders.Services.Implementation;
using Order.API.Features.Orders.Services.Interface;
using Order.API.Features.Products.Services;
using System.Reflection;
using System.Text;

namespace Order.API.Extensions
{
    public static class AddOrderExtensions
    {
        public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
        {

            //Adding Authentication to the Application Pipeline
            var secrect = builder.Configuration.GetValue<string>("ApiSettings:Secret");
            var issuer = builder.Configuration.GetValue<string>("ApiSettings:Issuer");
            var audience = builder.Configuration.GetValue<string>("ApiSettings:Audience");

            //Adding the key of the SymmetricSecurityKey
            var key = Encoding.ASCII.GetBytes(secrect);

            builder.Services.AddAuthentication(a =>
            {
                a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(a =>
            {
                a.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    ValidateAudience = true,
                };
            });

            builder.Services.AddScoped<OnMapping>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IOrderService, BasicOrderService>();
            builder.Services.AddScoped<IMessageBusService, MessageBusService>();
            builder.Services.AddAutoMapper(typeof(Program));
            var assembly = Assembly.GetExecutingAssembly();
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);
            });

            return builder;
        }
    }
}
