namespace NContext.Common.Tests.Specs
{
    using System;
    using System.Collections.Generic;

    using Machine.Specifications;

    using Ploeh.AutoFixture;

    public class with_data_of_type_Dictionary : when_creating_a_ServiceResponse_with_data_of_type<IDictionary<Object, Object>>
    {
        Establish context = () =>
            {
                var fixture = new Fixture();
                Data = fixture.Create<IDictionary<Object, Object>>();
            };

        Because of = () => CreateServiceResponse();

        It should_have_the_same_underlying_data_type = () => ServiceResponse.Data.GetType().ShouldEqual(Data.GetType());
    }
}