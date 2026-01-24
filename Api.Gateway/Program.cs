using Api.Gateway.Extensions;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Load Ocelot routes from the JSON file
//builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Add JWT authentication & Ocelot
//builder.Services.AddAuthentication("Bearer")
//    .AddJwtBearer(options =>
//    {
//        options.RequireHttpsMetadata = false;
//        options.TokenValidationParameters = new()
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = builder.Configuration["ApiSettings:Issuer"],
//            ValidAudience = builder.Configuration["ApiSettings:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(
//                Encoding.UTF8.GetBytes(builder.Configuration["ApiSettings:Secret"]))
//        };
//    });

builder.AddAuthentication();
builder.Services.AddOcelot();

var app = builder.Build();

//app.Use(async (context, next) =>
//{
//    Console.WriteLine(" Incoming request to Gateway");
//    Console.WriteLine($"  Path: {context.Request.Path}");
//    Console.WriteLine($"  Method: {context.Request.Method}");

//    if (context.Request.Headers.ContainsKey("Authorization"))
//        Console.WriteLine($"Authorization: {context.Request.Headers["Authorization"]}");
//    else
//        Console.WriteLine("No Authorization header found");

//    await next.Invoke(); 
//});

//// Ocelot must come last
//app.UseAuthentication();
//app.UseAuthorization();
app.UseOcelot();

app.Run();
