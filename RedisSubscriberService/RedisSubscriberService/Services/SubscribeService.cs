using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RedisSubscriberService.Contracts;

namespace RedisSubscriberService.Services;
public class SubscribeService : ISubscribeService, IHostedService, IDisposable
{
    private readonly IPubSub _pubsub;

    private readonly ILogger<SubscribeService> _logger;

    private string _topic = "WindowsServiceTest";

    public SubscribeService(IPubSub pubsub, ILogger<SubscribeService> logger)
    {
        _pubsub = pubsub;
        _logger = logger;
    }

    public Task Process()
    {
        _pubsub.Subscribe(_topic);
        _logger.LogInformation("Redis topic subscribed : {topic}", _topic);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _pubsub.Unsubscribe(_topic);

        _logger.LogInformation("~Redis topic unsubscribed : {topic}", _topic);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Process();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Dispose();

        return Task.CompletedTask;
    }
}
