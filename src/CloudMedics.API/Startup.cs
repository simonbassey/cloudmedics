using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudMedics.Data;
using CloudMedics.Data.Repositories;
using CloudMedics.Domain.Models;
using CouldMedics.Services.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAll");
            app.UseMvc();
        }
    }
}
