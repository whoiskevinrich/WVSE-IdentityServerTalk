using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace Sample.Auth0.WebApi
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
            // 8. Add framework services.
            services.AddMvc(config =>
            {
                // Create policy to enforce Global Authentication
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                config.Filters.Add(new AuthorizeFilter(policy));
            });

            // 9. Add Authorization Policies
            services.AddAuthorization(options => {
                options.AddPolicy("Developer", policy => 
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "claims.read");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Prologue: Explain Controller

            // 1. Install Appropriate packages: 
            //      Install-Package Microsoft.AspNetCore.Authentication.JwtBearer

            // 2. Create Non-Interactive Client at Auth0:
            //      https://manage.auth0.com/#clients
            //      Name: LINQPad

            // 3. Select API (Add New)
            //      Name: Sample.Auth0.WebApi
            //      Identifier: https://sample.auth0.webapi/

            // 4. Add Scopes for "claims.read", "claims.write"

            // 5. Assign claims.read scope to LINQPad client

            // 6. Configure OAuth on Website for the client
            //      Client => Settings => Advanced => OAuth
            //      Change JsonWebTokenSignature to RS256

            // 7. Set up API Middleware via quickstart
            
            // 8-9 (above). Set up Access Policies

            // 10. Set up LINQPad consumer via quickstart

            //JWT Bearer Auth Middleware:
            //      Auth0 => Api => Quickstart
            var options = new JwtBearerOptions
            {
                Audience = "https://sample.auth0.webapi/",
                Authority = "https://whoiskevinrich.auth0.com/"
            };
            app.UseJwtBearerAuthentication(options);

            app.UseMvc();
        }
    }
}
