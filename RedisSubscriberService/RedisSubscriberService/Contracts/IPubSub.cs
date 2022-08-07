namespace RedisSubscriberService.Contracts;
public interface IPubSub
{
    void Subscribe(string topic);

    void Unsubscribe(string topic);

    Task Publish(string topic, string message);
}
