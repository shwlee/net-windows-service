using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RedisSubscriberService.Contracts;
using StackExchange.Redis;

namespace RedisSubscriberService.Services;
public class RedisPubSub : IPubSub
{
    private ConnectionMultiplexer _connection;

    private readonly ILogger<RedisPubSub> _logger;

    private readonly IHostApplicationLifetime _lifeTime;

    public RedisPubSub(ILogger<RedisPubSub> logger, IHostApplicationLifetime lifeTime)
    {
        _connection = ConnectionMultiplexer.Connect(new ConfigurationOptions { EndPoints = { "localhost:7002" } });
        _logger = logger;
        _lifeTime = lifeTime;
    }

    public void Subscribe(string topic)
    {
        var pubsub = _connection.GetSubscriber();
        pubsub.Subscribe(topic, ReceivedMessage);
    }

    public Task Publish(string topic, string message)
    {
        var pubsub = _connection.GetSubscriber();
        return pubsub.PublishAsync(topic, message);
    }

    public void Unsubscribe(string topic)
    {
        var pubsub = _connection.GetSubscriber();
        pubsub.Unsubscribe(topic, ReceivedMessage);
    }

    private void ReceivedMessage(RedisChannel channel, RedisValue message)
    {
        var logMessage = "channel:{channel}, message:{message}";
        _logger.LogInformation(logMessage, channel, message);

        if (message == "q")
        {
            _lifeTime.StopApplication();
        }
    }
}
