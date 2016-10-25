namespace NContext.Common.Tests.Specs.AwaitLetAsync
{
    using System;
    using System.Threading.Tasks;

    using Common;

    using Machine.Specifications;

    public class when_using_AwaitLetAsync<T> : when_using_a_future_ServiceResponse<T>
    {
        Because of = async () => ResultResponse = await FutureServiceResponse.AwaitLetAsync(LetAsyncFunc).Await().AsTask;
        
        protected static Func<T, Task> LetAsyncFunc;

        protected static IServiceResponse<T> ResultResponse;
    }
}