namespace NContext.Common.Tests.Specs.BindAsync
{
    using System.Threading.Tasks;

    using Machine.Specifications;

    public class with_data : when_using_BindAsync<int, int>
    {
        Establish context = () =>
        {
            ServiceResponse = new DataResponse<int>(0);
            BindAsyncFunc = source => Task.FromResult<IServiceResponse<int>>(new DataResponse<int>(10));
        };

        It should_bind_the_result_data = () => ResultResponse.Data.ShouldEqual(10);
    }
}