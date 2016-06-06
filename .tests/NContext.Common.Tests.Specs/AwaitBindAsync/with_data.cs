namespace NContext.Common.Tests.Specs.AwaitBindAsync
{
    using System.Threading.Tasks;

    using Machine.Specifications;

    public class with_data : when_using_AwaitBindAsync<int, int>
    {
        Establish context = () =>
        {
            FutureServiceResponse = Task.Run<IServiceResponse<int>>(() => new DataResponse<int>(0));
            BindAsyncFunc = source => Task.FromResult<IServiceResponse<int>>(new DataResponse<int>(10));
        };

        It should_bind_the_result_data = () => ResultResponse.Data.ShouldEqual(10);
    }
}