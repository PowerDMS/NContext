namespace NContext.Common.Tests.Specs
{
    using System.Threading.Tasks;

    public class when_using_a_future_ServiceResponse<T>
    {
        protected static Task<IServiceResponse<T>> FutureServiceResponse;
    }
}