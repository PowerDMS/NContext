namespace NContext.Extensions.Redis
{
    using StackExchange.Redis;

    public interface IRedisCache
    {
        IDatabase Database { get; }

        IDatabaseAsync DatabaseAsync { get; }

        ConnectionMultiplexer Multiplexer { get; }
    }
}