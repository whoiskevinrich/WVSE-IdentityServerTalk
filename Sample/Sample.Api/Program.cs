using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Sample.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Sample Consuming API";

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }
    }
}
