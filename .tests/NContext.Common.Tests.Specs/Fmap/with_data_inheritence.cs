namespace NContext.Common.Tests.Specs.Fmap
{
    using Machine.Specifications;

    using Ploeh.AutoFixture;

    public class with_data_inheritence : when_using_fmap<DummyParent, DummyData>
    {
        Establish context = () =>
        {
            ResultData = new Fixture().Create<DummyData>();
            CreateServiceResponse(new DataResponse<DummyChild>(new DummyChild()));
        };

        It should_bind_the_result_data = () => ResultResponse.Data.ShouldEqual(ResultData);
    }
}