using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Linq;
using ZLMediaKit.CSharp.Models;
using ZLMediaKit.CSharp.ZLMediaKit;

namespace ZLMediaKit.CSharp.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {

            CreateHostBuilder(args).Build().Run();
        }
        public static IConfigurationRoot config = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                  .Build();
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .UseSystemd()
                .UseContentRoot(AppContext.BaseDirectory)
                .ConfigureHostConfiguration(configure =>
                {
                    configure.AddConfiguration(config);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(hostContext.Configuration)
                    .Enrich.FromLogContext()
#if DEBUG
                    .WriteTo.Console()
#endif
                    .WriteTo.File("logs/log_", rollingInterval: RollingInterval.Day)
                    .CreateLogger();
                    services.AddLogging(builder =>
                    {
                        builder.AddSerilog();
                    });
                    services.AddMediaKitCSharp(config);
                });
        }

    }
}
