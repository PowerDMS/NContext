namespace NContext.Extensions.Redis.Tests.Specs
{
    using Machine.Specifications;

    [Tags("functional")]
    public class when_calling_AddOrGetExisting_and_the_key_already_exists : when_using_RedisCache
    {
        Because of = () => Cache.Set(CacheKey, 5, null);

        It should_return_the_existing_value = () => Cache.AddOrGetExisting(CacheKey, 10, null).ShouldEqual(5);
    }
}