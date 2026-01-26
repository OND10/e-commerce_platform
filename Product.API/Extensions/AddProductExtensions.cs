using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Product.API.Features.Products.Repository.Implementation;
using Product.API.Features.Products.Repository.Interface;
using System.Reflection;
using System.Text;

namespace Product.API.Extensions
{
    public static class AddProductExtensions
    {
        public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
        {

            //Adding Authentication to the Application Pipeline
            var jwtSecret = builder.Configuration["ApiSettings:Secret"];
            var jwtIssuer = builder.Configuration["ApiSettings:Issuer"];
            var jwtAudience = builder.Configuration["ApiSettings:Audience"];

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
                };
            });


            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IUnitofWork, UnitofWork>();
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
