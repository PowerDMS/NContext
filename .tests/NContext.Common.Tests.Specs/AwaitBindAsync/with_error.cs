namespace NContext.Common.Tests.Specs.AwaitBindAsync
{
    using System;
    using System.Threading.Tasks;

    using Machine.Specifications;

    using NContext.Extensions;

    public class with_error : when_using_AwaitBindAsync<int, int>
    {
        Establish context = () =>
        {
            FutureServiceResponse = Task.Run<IServiceResponse<int>>(() => new DataResponse<int>(0));
            BindAsyncFunc = source => Task.Run<IServiceResponse<int>>(() => new ErrorResponse<int>(new Exception().ToError()));
        };

        It should_bind_the_result_data = () => ResultResponse.IsLeft.ShouldBeTrue();
    }
}