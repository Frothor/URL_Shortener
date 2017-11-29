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
            services.AddScoped<SignInManager<IdentityUser>>();
            services.AddScoped<UserManager<IdentityUser>>();
            //integracja z Asp.Net Core Mvc
            services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<Context>();
            services.AddScoped<RoleManager<IdentityRole>>();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
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
