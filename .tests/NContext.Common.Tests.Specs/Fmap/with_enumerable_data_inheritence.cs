namespace NContext.Common.Tests.Specs.Fmap
{
    using System.Collections.Generic;

    using Machine.Specifications;

    using Ploeh.AutoFixture;

    public class with_enumerable_data_inheritence : when_using_fmap<IEnumerable<DummyParent>, DummyData>
    {
        Establish context = () =>
        {
            ResultData = new Fixture().Create<DummyData>();
            CreateServiceResponse(new DataResponse<IEnumerable<DummyChild>>(new[] { new DummyChild() }));
        };

        It should_bind_the_result_data = () => ResultResponse.Data.ShouldEqual(ResultData);
    }
}