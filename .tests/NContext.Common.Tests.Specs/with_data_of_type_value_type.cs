namespace NContext.Common.Tests.Specs
{
    using System;

    using Machine.Specifications;

    public class with_data_of_type_Int32 : when_creating_a_ServiceResponse_with_data_of_type<Int32>
    {
        Establish context = () =>
            {
                Data = 5;
            };

        Because of = () => CreateDataResponse();

        It should_have_the_same_underlying_data_type = () => ServiceResponse.Data.GetType().ShouldEqual(Data.GetType());
    }
}