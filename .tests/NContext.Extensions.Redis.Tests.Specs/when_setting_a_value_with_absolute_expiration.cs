namespace NContext.Extensions.Redis.Tests.Specs
{
    using System;
    using System.Reactive.Linq;
    using System.Threading;

    using Machine.Specifications;

    [Tags("integration")]
    public class when_setting_a_value_with_absolute_expiration : when_using_RedisCache
    {
        Because of = () =>
        {
            _Timer = Observable.Timer(TimeSpan.FromSeconds(_Seconds))
                .Subscribe(_ => _HasExpired = true);

            Cache.Set(CacheKey, 5, DateTimeOffset.UtcNow.Add(TimeSpan.FromSeconds(_Seconds)));
        };

        It should_be_set = () => Cache.Get(CacheKey).ShouldEqual(5);

        It should_expire = () =>
        {
            while (!_HasExpired)
            {
                Thread.Sleep(200);
            }

            Cache.Get(CacheKey).ShouldEqual(null);
        };

        Cleanup cleanup = () => _Timer.Dispose();

        private static Boolean _HasExpired;

        private static Double _Seconds = 1;

        private static IDisposable _Timer;
    }
}