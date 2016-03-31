namespace NContext.Extensions
{
    using System;
    using System.Reflection;

    using NContext.Common;
    using NContext.ErrorHandling;

    /// <summary>
    /// Defines extension methods for <see cref="Error"/>.
    /// </summary>
    public static class ErrorExtensions
    {
        /// <summary>
        /// Returns the <paramref name="error"/> as a <typeparamref name="TException"/>.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="error">The error to convert.</param>
        /// <returns><typeparamref name="TException"/> instance.</returns>
        /// <exception cref="TargetInvocationException">
        /// Thrown when <typeparamref name="TException"/> does not have a constructor which takes in an exception message <see cref="String"/>.
        /// </exception>
        public static TException ToException<TException>(this ErrorBase error)
            where TException : Exception
        {
            return (TException)Activator.CreateInstance(typeof(TException), error.Message);
        }

        /// <summary>
        /// Returns the <paramref name="error" /> as a <typeparamref name="TException" />.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="error">The error to convert.</param>
        /// <param name="exceptionFactory">The exception factory.</param>
        /// <returns><typeparamref name="TException" /> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="exceptionFactory" /> is null.</exception>
        public static TException ToException<TException>(this ErrorBase error, Func<ErrorBase, TException> exceptionFactory)
            where TException : Exception
        {
            if (exceptionFactory == null)
            {
                throw new ArgumentNullException("exceptionFactory");
            }

            return exceptionFactory.Invoke(error);
        }
    }
}