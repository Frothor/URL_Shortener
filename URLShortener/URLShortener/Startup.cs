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
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
