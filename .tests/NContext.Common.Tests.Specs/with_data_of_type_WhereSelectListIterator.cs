namespace NContext.Common.Tests.Specs
{
    using System.Collections.Generic;
    using System.Linq;

    using Machine.Specifications;

    using Ploeh.AutoFixture;

    public class with_data_of_type_WhereSelectListIterator : when_creating_a_ServiceResponse_with_enumerable_data
    {
        Establish context = () =>
            {
                var fixture = new Fixture();
                Data = fixture.CreateMany<DummyData>().Select(dd => dd);
            };

        Because of = () => CreateServiceResponse();

        It should_materialize_the_data_to_a_concrete_list = () => ServiceResponse.Data.GetType().GetGenericTypeDefinition().ShouldEqual(typeof(List<>));
    }
}