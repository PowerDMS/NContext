namespace NContext.Common.Tests.Specs.AwaitLetAsync
{
    using System;
    using System.Threading.Tasks;

    using Machine.Specifications;

    public class with_exception : when_using_AwaitLetAsync<int>
    {
        Establish context = () =>
        {
            FutureServiceResponse = Task.Run<IServiceResponse<int>>(() => new DataResponse<int>(5));
            LetAsyncFunc = source => Task.Run(() => { throw new Exception("error"); });
        };

        It should_return_a_left_response = () => ResultResponse.IsLeft.ShouldBeTrue();
    }
}