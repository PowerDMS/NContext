namespace NContext.Common.Tests.Specs
{
    using System.Linq;

    using Machine.Specifications;

    using Ploeh.AutoFixture;

    public class with_data_of_type_Array : when_creating_a_ServiceResponse_with_enumerable_data
    {
        Establish context = () =>
            {
                var fixture = new Fixture();
                Data = fixture.CreateMany<DummyData>().ToArray();
            };

        Because of = () => CreateServiceResponse();

        It should_have_the_same_underlying_data_type = () => ServiceResponse.Data.GetType().ShouldEqual(Data.GetType());
    }
}