namespace NContext.Common.Tests.Specs
{
    using System;

    using Machine.Specifications;

    public abstract class when_creating_a_ServiceResponse_with_data_of_type<T>
    {
        static Lazy<IServiceResponse<T>> _ServiceResponse;

        Establish context =
            () =>
            _ServiceResponse =
                new Lazy<IServiceResponse<T>>(() => new DataResponse<T>(Data));

        protected static void CreateDataResponse()
        {
            var x = _ServiceResponse.Value;
        }

        protected static void CreateServiceResponse(IServiceResponse<T> response)
        {
            _ServiceResponse = new Lazy<IServiceResponse<T>>(() => response);
            var x = _ServiceResponse.Value;
        }

        public static IServiceResponse<T> ServiceResponse
        {
            get { return _ServiceResponse.Value; }
        } 

        public static T Data { get; set; } 
    }
}