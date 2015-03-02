using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using Ploeh.AutoFixture;

namespace NContext.Common.Tests.Specs
{
    public class with_data_that_implements_IQueryable : when_creating_a_ServiceResponse_with_enumerable_data
    {
        Establish context = () =>
        {
            var fixture = new Fixture();
            Data = fixture.CreateMany<DummyData>().AsQueryable();
        };

        Because of = () => CreateDataResponse();

        It should_materialize_the_data_to_a_concrete_list = () => ServiceResponse.Data.GetType().GetGenericTypeDefinition().ShouldEqual(typeof(List<>));
    }
}