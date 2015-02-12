namespace NContext.Common.Tests.Specs
{
    using Machine.Specifications;

    public class with_data_of_type_inherits_from_Collection : when_creating_a_ServiceResponse_with_data_of_type<DummyCollection>
    {
        Establish context = () =>
        {
            Data = new DummyCollection { 5 };
        };

        Because of = () => CreateDataResponse();

        It should_have_the_same_underlying_data_type = () => ServiceResponse.Data.GetType().ShouldEqual(Data.GetType());
    }
}