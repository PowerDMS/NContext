namespace NContext.Common.Tests.Specs.AwaitLetAsync
{
    using System.Threading.Tasks;

    using Machine.Specifications;

    public class with_data : when_using_AwaitLetAsync<int>
    {
        Establish context = () =>
        {
            FutureServiceResponse = Task.Run<IServiceResponse<int>>(() => new DataResponse<int>(5));
            LetAsyncFunc = source => Task.FromResult<object>(0);
        };

        It should_bind_the_result_data = () => ResultResponse.Data.ShouldEqual(5);
    }
}