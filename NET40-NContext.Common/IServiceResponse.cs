namespace NContext.Common
{
    public interface IServiceResponse<out T> : IEither<Error, T>
    {
        Error Error { get; }

        T Data { get; }
    }
}