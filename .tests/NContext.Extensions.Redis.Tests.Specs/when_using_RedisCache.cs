namespace NContext.Extensions.Redis.Tests.Specs
{
    using System;
    using System.Runtime.Caching;

    using Machine.Specifications;

    using StackExchange.Redis;
    
    [Subject(typeof(RedisCache))]
    public class when_using_RedisCache
    {
        Establish context = () =>
        {
            Cache = new RedisCache(
                () => new ConfigurationOptions
                {
                    EndPoints =
                    {
                        { "192.168.121.2", 6379 }
                    }
                });

            CacheKey = Guid.NewGuid().ToString();
        };

        Cleanup cleanup = () => Cache.Remove(CacheKey);
        
        protected static ObjectCache Cache;

        protected static String CacheKey;
    }
}