namespace NContext.Extensions
{
    using Microsoft.FSharp.Core;

    using NContext.Common;
    using NContext.ErrorHandling;

    /// <summary>
    /// Defines extension methods for <see cref="ErrorBase"/>.
    /// </summary>
    public static class ErrorBaseExtensions
    {
        /// <summary>
        /// Returns a new <see cref="IServiceResponse{Unit}"/> with the specified <paramref name="error"/>.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns>IServiceResponse{Unit}.</returns>
        public static IServiceResponse<Unit> ToServiceResponse(this ErrorBase error)
        {
            return new ErrorResponse<Unit>(error);
        }

        /// <summary>
        /// Returns a new <see cref="IServiceResponse{T}"/> with the specified <paramref name="error"/>.
        /// </summary>
        /// <typeparam name="T">Type of IServiceResponse.</typeparam>
        /// <param name="error">The error.</param>
        /// <returns>IServiceResponse{T}.</returns>
        public static IServiceResponse<T> ToServiceResponse<T>(this ErrorBase error)
        {
            return new ErrorResponse<T>(error);
        }
    }
}