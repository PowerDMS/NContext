namespace NContext.Common.Tests.Specs.Fmap
{
    using Machine.Specifications;

    using NContext.ErrorHandling;

    public class with_an_error : when_using_fmap<DummyParent, DummyData>
    {
        Establish context = () =>
        {
            CreateServiceResponse(new ErrorResponse<DummyParent>(ErrorBase.NullObject()));
        };

        It should_bind_the_result_data = () => ResultResponse.Error.ShouldNotBeNull();
    }
}