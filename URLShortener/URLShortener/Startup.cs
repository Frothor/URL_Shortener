using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using URLShortener.Models;

namespace URLShortener
{
    public class Startup
    {
        protected IConfigurationRoot ConfigurationRoot;
        public Startup()
        {
            var configuratioBuilder = new ConfigurationBuilder();
            configuratioBuilder.AddXmlFile("Config.xml");
            ConfigurationRoot = configuratioBuilder.Build();

        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Context>(optionsBuilder =>
                optionsBuilder.UseSqlServer(
                    ConfigurationRoot["ConnectionString"]));
            services.AddScoped<SignInManager<User>>();
            services.AddScoped<UserManager<User>>();
            //integracja z Asp.Net Core Mvc
            services.AddIdentity<User, IdentityRole<int>>()
            .AddEntityFrameworkStores<Context>();
            services.AddScoped<RoleManager<IdentityRole<int>>>();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
        }
    }
}
