﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sample.IdentityServer.Configurations;

namespace Sample.IdentityServer
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // configure identity server with in-memory stores, keys, clients and resources
            services.AddIdentityServer()
                .AddInMemoryApiResources(Resources.GetApiResources())
                .AddInMemoryIdentityResources(Resources.GetIdentityResources())
                .AddTestUsers(Users.GetTestUsers())
                .AddInMemoryClients(Clients.GetClients())
                .AddTemporarySigningCredential();

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            {
                loggerFactory.AddConsole();

                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                // Add the identity server middleware
                app.UseIdentityServer();

                // Browse to http://localhost:5001/.well-known/openid-configuration

                app.UseMvcWithDefaultRoute();
            }
        }
    }
}
