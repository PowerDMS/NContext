namespace NContext.Extensions.Redis.Tests.Specs
{
    using System;

    using Machine.Specifications;

    [Tags("functional")]
    public class when_calling_AddOrGetExisting_and_the_key_doesnt_exist : when_using_RedisCache
    {
        Because of = () => _Value = Cache.AddOrGetExisting(CacheKey, 10, null);

        It should_return_the_specified_value = () => _Value.ShouldEqual(10);

        It should_add_the_specified_value_to_the_cache = () => Cache.Get(CacheKey).ShouldEqual(10);

        private static Object _Value;
    }
}