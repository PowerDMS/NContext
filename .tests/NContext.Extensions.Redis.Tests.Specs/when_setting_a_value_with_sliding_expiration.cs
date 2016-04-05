namespace NContext.Extensions.Redis.Tests.Specs
{
    using System;
    using System.Runtime.Caching;
    using System.Threading;

    using Machine.Specifications;
    
    [Tags("integration")]
    public class when_setting_a_value_with_sliding_expiration : when_using_RedisCache
    {
        Because of = () =>
        {
            Cache.Set(CacheKey, 5, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMilliseconds(_Milliseconds) });
        };

        It should_be_set = () => Cache.Get(CacheKey).ShouldEqual(5);

        It should_slide_expiration_after_each_key_access = () =>
        {
            Cache.Get(CacheKey).ShouldEqual(5);
            Thread.Sleep(1000);
            Cache.Get(CacheKey).ShouldEqual(null);
        };

        private static Double _Milliseconds = 500;
    }
}