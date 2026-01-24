using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Api.Gateway.Extensions
{
    public static class ApiGatewayExtension
    {
        public static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder builder)
        {

            var settingsSection = builder.Configuration.GetSection("ApiSettings");
            //Adding Authentication to the Application Pipeline
            var secrect = settingsSection.GetValue<string>("Secret");
            var issuer = settingsSection.GetValue<string>("Issuer");
            var audience = settingsSection.GetValue<string>("Audience");

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



            return builder;
        }

    }
}
