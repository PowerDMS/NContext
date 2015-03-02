namespace NContext.Common
{
    /// <summary>
    /// Defines a data-transfer-object used for functional composition.
    /// </summary>
    /// <typeparam name="T">Type of data to return.</typeparam>
    public interface IServiceResponse<out T> : IEither<Error, T>
    {
        /// <summary>
        /// Gets the error.
        /// </summary>
        Error Error { get; }

        /// <summary>
        /// Gets the data.
        /// </summary>
        T Data { get; }
    }
}