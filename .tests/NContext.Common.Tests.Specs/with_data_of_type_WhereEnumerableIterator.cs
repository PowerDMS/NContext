namespace NContext.Common.Tests.Specs
{
    using System.Collections.Generic;
    using System.Linq;

    using Machine.Specifications;

    using Ploeh.AutoFixture;

    public class with_data_of_type_WhereEnumerableIterator : when_creating_a_ServiceResponse_with_enumerable_data
    {
        Establish context = () =>
            {
                var fixture = new Fixture();
                Data = fixture.CreateMany<DummyData>().Where(dd => dd.Id > 5);
            };

        Because of = () => CreateServiceResponse();

        It should_have_the_same_underlying_data_type = () => ServiceResponse.Data.GetType().ShouldEqual(Data.GetType());
    }
}