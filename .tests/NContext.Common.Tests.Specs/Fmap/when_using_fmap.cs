namespace NContext.Common.Tests.Specs.Fmap
{
    using Machine.Specifications;

    using NContext.Common;

    public abstract class when_using_fmap<T, T2> : when_creating_a_ServiceResponse_with_data_of_type<T>
    {
        Because of = () => ResultResponse = ServiceResponse.Fmap(_ => ResultData);

        protected static T2 ResultData;

        protected static IServiceResponse<T2> ResultResponse;
    }
}