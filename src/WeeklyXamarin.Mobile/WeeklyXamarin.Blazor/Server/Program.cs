using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WeeklyXamarin.Blazor.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static Version Version { get; set; } = typeof(Program).Assembly
                        .GetName().Version ?? new();
        public static string ProductVersion { get; set; } = typeof(Program).Assembly
                                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                                .InformationalVersion ?? "";

    }
}
