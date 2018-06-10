using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace CloudMedics.API.Configurations
{
    public static class StartupConfigExtensions
    {

        public static IdentityOptions ConfigureIdentiyOptions(this IdentityOptions options)
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireUppercase = false;
            options.Password.RequiredUniqueChars = 0;
            options.Password.RequireNonAlphanumeric = false;

            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;

            return options;
        }

        public static JwtBearerOptions ConfigureJWTBearerOptions(this JwtBearerOptions jwtBearerOptions, IConfiguration appConfig)
        {

            jwtBearerOptions.RequireHttpsMetadata = false;
            jwtBearerOptions.SaveToken = true;
            jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = appConfig["Token:Issuer"],
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = appConfig["Token:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appConfig["Token:Key"])),
                RequireSignedTokens = true,
                RequireExpirationTime = true,
                ValidateLifetime = true
            };
            jwtBearerOptions.Events = new JwtBearerEvents()
            {
                OnTokenValidated = (ctx) =>
                {
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = (ctx) =>
                {
                    ctx.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                }
            };

            return jwtBearerOptions;
        }

        public static CorsOptions ConfigureCorsPolicy(this CorsOptions corsOptions)
        {
            corsOptions.AddPolicy("AllowAll",
                                  corsPolicyBuilder => corsPolicyBuilder
                                  .AllowAnyHeader()
                                  .AllowAnyMethod()
                                  .AllowAnyOrigin()
                                 );
            return corsOptions;
        }
    }
}
