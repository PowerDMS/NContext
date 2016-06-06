namespace NContext.Common.Tests.Specs.BindAsync
{
    using System;
    using System.Threading.Tasks;

    using Common;

    using Machine.Specifications;

    public class when_using_BindAsync<T, T2> : when_using_a_ServiceResponse<T>
    {
        Because of = async () => ResultResponse = await ServiceResponse.BindAsync(BindAsyncFunc).Await().AsTask;
        
        protected static Func<T, Task<IServiceResponse<T2>>> BindAsyncFunc;

        protected static IServiceResponse<T2> ResultResponse;
    }
}