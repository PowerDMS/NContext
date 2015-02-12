namespace NContext.Common.Tests.Specs.Fmap
{
    using Machine.Specifications;

    using Ploeh.AutoFixture;

    public class with_primitive_data : when_using_fmap<DummyData, int>
    {
        Establish context = () =>
        {
            ResultData = 5;
            CreateServiceResponse(new DataResponse<DummyData>(new Fixture().Create<DummyData>()));
        };

        It should_bind_the_result_data = () => ResultResponse.Data.ShouldEqual(ResultData);
    }
}