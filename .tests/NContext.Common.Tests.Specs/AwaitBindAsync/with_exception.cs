namespace NContext.Common.Tests.Specs.AwaitBindAsync
{
    using System;
    using System.Threading.Tasks;

    using Machine.Specifications;

    public class with_exception : when_using_AwaitBindAsync<int, int>
    {
        Establish context = () =>
        {
            FutureServiceResponse = Task.Run<IServiceResponse<int>>(() => new DataResponse<int>(0));
            BindAsyncFunc = source => Task.Run<IServiceResponse<int>>(() => { throw new Exception(); return (IServiceResponse<int>)null; });
        };

        It should_return_a_left_response = () => ResultResponse.IsLeft.ShouldBeTrue();
    }
}