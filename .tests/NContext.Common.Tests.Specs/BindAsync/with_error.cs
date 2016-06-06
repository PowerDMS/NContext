namespace NContext.Common.Tests.Specs.BindAsync
{
    using System;
    using System.Threading.Tasks;

    using Machine.Specifications;

    using NContext.Extensions;

    public class with_error : when_using_BindAsync<int, int>
    {
        Establish context = () =>
        {
            ServiceResponse = new DataResponse<int>(0);
            BindAsyncFunc = source => Task.Run<IServiceResponse<int>>(() => new ErrorResponse<int>(new Exception().ToError()));
        };

        It should_return_a_left_response = () => ResultResponse.IsLeft.ShouldBeTrue();
    }
}