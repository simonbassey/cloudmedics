using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudMedics.Data;
using CloudMedics.Data.Repositories;
using CloudMedics.Domain.Models;
using CouldMedics.Services.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace CloudMedics.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectString = Configuration.GetConnectionString("cloudmedicsDbConnection");
            services.AddDbContext<CloudMedicDbContext>(options => options.UseMySql(connectString));
            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<CloudMedicDbContext>();
            services.Configure<IdentityOptions>(
                (IdentityOptions options) =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 6;

                    options.User.RequireUniqueEmail = true;
                    options.SignIn.RequireConfirmedEmail = true;
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(jwtBearerOptions =>
                    {
                            jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidIssuer = Configuration["Token:Issuer"],
                                ValidAudience = Configuration["Token:Audience"],
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:key"])),
                                ValidateLifetime = true,
                                ValidateAudience = true
                            };
                        jwtBearerOptions.Audience = Configuration["Token:Audience"];
                        jwtBearerOptions.Authority = Configuration["Token:Issuer"];
            
                    });
            services.AddCors((CorsOptions corsOptions) => 
                                     corsOptions.AddPolicy(
                                        "AllowAll", corsPolicyBuilder =>
                                                        corsPolicyBuilder.AllowAnyHeader()
                                                        .AllowAnyMethod()
                                                        .AllowAnyOrigin())
                            );
            //register framework services
            services.AddMvc();

            //register dependent service : Emails, SMS, Infrastructure services

            //register application services
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<AppDbInitializer>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, AppDbInitializer dbInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseMvc();
            dbInitializer.Seed().Wait();
        }
    }
}
