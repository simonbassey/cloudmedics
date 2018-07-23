using CloudMedics.API.Helpers.Extensions;
using CloudMedics.Data;
using CloudMedics.Data.Repositories;
using CloudMedics.Domain.Models;
using CouldMedics.Services.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AutoMapper;
using CouldMedics.Services;
using ExpenseMgr.Data;
using System;
using CloudMedics.Data.Helpers;

namespace CloudMedics.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        //public IApplicationBuilder


        public void ConfigureServices(IServiceCollection services)
        {
            var connectString = Configuration.GetConnectionString("cloudmedicsDbConnection");
            services.AddDbContext<CloudMedicDbContext>();
            services.AddEntityFrameworkMySql();

            services.AddAutoMapper();
            services.AddIdentity<ApplicationUser, IdentityRole>()
                   .AddEntityFrameworkStores<CloudMedicDbContext>()
                   .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options => options.ConfigureIdentiyOptions());
            services.AddAuthentication(option =>
            {
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => options.ConfigureJWTBearerOptions(Configuration));

            services.AddCors(options => options.ConfigureCorsPolicy());
            services.Configure<IISOptions>(config =>
                                           config.ForwardClientCertificate = false);
            //register framework services
            services.AddMvc();
            //register application services

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IPatientUserRepository, PatientUserRepository>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IPatientUserService, PatientUserService>();
            services.AddTransient<AppDbInitializer>();

            ConfigSettingsHelper.Create(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, AppDbInitializer dbInitializer, CloudMedicDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseMvc();
            IServiceProvider serviceProvider = app.ApplicationServices.CreateScope().ServiceProvider;
            var dbContext = (CloudMedicDbContext)serviceProvider.GetRequiredService<CloudMedicDbContext>();
            context.Database.Migrate();
            dbInitializer.Seed().Wait();
        }


        #region privates
        #endregion
    }
}
