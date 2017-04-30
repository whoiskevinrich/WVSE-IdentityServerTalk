using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Sample.Web
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
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // direct application to issue it's own cookie and use cookie-based authentication
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "cookie"
            });

            // Turn off JWT claim type mapping to allow well-known claims ('sub'/'idp') through unmolested 
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            // direct applicaition to use an OpenIDConnect Provider (our Identity Server)
            app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
            {
                // the client registered with our IdentityServer
                ClientId = "oidcClient",
                SaveTokens = true,

                // Set endpoint for provider
                Authority = "http://localhost:5000",
                RequireHttpsMetadata = false,

                // authentication type to log in with
                SignInScheme = "cookie"
            });

            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();
        }
    }
}
