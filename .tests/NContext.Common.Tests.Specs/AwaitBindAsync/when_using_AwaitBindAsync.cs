namespace NContext.Common.Tests.Specs.AwaitBindAsync
{
    using System;
    using System.Threading.Tasks;

    using Common;
    
    using Machine.Specifications;

    public class when_using_AwaitBindAsync<T, T2> : when_using_a_future_ServiceResponse<T>
    {
        Because of = async () => ResultResponse = await FutureServiceResponse.AwaitBindAsync(BindAsyncFunc).Await().AsTask;
        
        protected static Func<T, Task<IServiceResponse<T2>>> BindAsyncFunc;

        protected static IServiceResponse<T2> ResultResponse;
    }
}