using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using RedisSubscriberService.Contracts;
using RedisSubscriberService.Services;
using System.Diagnostics;

Debugger.Launch();

var host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "RedisSubscribeService";
    })
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration;
        services.AddLogging(builder =>
        {
            builder.ClearProviders();            

            var currentDir = $"{Directory.GetCurrentDirectory()}"; // {Path.DirectorySeparatorChar}
            var path = "configs";
            var logConfigFile = "nlog.config";
            var logConfigPath = Path.Combine(currentDir, path, logConfigFile);            

            builder.AddNLog(logConfigPath);
        });

        services.AddTransient<IPubSub, RedisPubSub>();
        //services.AddSingleton<ISubscribeService, SubscribeService>();
        services.AddHostedService<SubscribeService>();
    })
    .Build();

var logger = host.Services.GetService<ILogger<SubscribeService>>();

await host.RunAsync();
logger?.LogInformation("Service Started!");


logger?.LogInformation("Service Shutdown!");