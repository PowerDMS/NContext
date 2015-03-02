namespace NContext.Tests.Specs.Caching
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Caching;
    using Machine.Specifications;
    using NContext.Caching;

    public class when_using_CacheManager
    {
        private static CacheConfiguration _CacheConfig;

        Establish context = () =>
        {
            var tries = 0;
            _CacheConfig = new CacheConfiguration(
                new Dictionary<String,Func<ObjectCache>>
                {
                    {
                        "redis",
                        () =>
                        {
                            if (++tries <= 2) throw new Exception("First try throws exception");

                            return new MemoryCache("redis");
                        }
                    }
                });

            CacheManager = new CacheManager(_CacheConfig);
        };

        Because of = () =>
        {
            FirstTry = CacheManager.GetProvider("redis");
            SecondTry = CacheManager.GetProvider("redis");
            ThirdTry = CacheManager.GetProvider("redis");
        };

        It should_return_null_when_unabled_to_create = () => FirstTry.ShouldEqual(null);

        It should_create_the_provider = () => SecondTry.ShouldNotEqual(null);

        It should_return_the_created_instance = () => ThirdTry.ShouldEqual(SecondTry);

        public static ObjectCache ThirdTry { get; set; }

        public static ObjectCache SecondTry { get; set; }

        public static ObjectCache FirstTry { get; set; } 

        public static CacheManager CacheManager { get; set; }
    }
}