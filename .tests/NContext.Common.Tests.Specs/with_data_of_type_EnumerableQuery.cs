﻿namespace NContext.Common.Tests.Specs
{
    using System.Collections.Generic;
    using System.Linq;

    using Machine.Specifications;

    using Ploeh.AutoFixture;

    public class with_data_of_type_EnumerableQuery : when_creating_a_ServiceResponse_with_enumerable_data
    {
        Establish context = () =>
            {
                var fixture = new Fixture();
                Data = fixture.CreateMany<DummyData>().AsQueryable().Where(dd => dd.Id > 5);
            };

        Because of = () => CreateDataResponse();

        It should_materialize_to_List = () => ServiceResponse.Data.GetType().GetGenericTypeDefinition().ShouldEqual(typeof(List<>));
    }
}