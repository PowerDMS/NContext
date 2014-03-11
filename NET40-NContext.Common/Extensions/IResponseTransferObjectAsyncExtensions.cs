namespace NContext.Common
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines extension methods for <see cref="IResponseTransferObject{T}"/>.
    /// </summary>
    public static class IResponseTransferObjectAsyncExtensions
    {
        /// <summary>
        /// If <seealso cref="IResponseTransferObject{T}.Error" /> is not null, returns a new <see cref="IResponseTransferObject{T2}" /> instance with the current
        /// <seealso cref="IResponseTransferObject{T}.Error" />. Else, binds the <seealso cref="IResponseTransferObject{T}.Data" /> into the specified <paramref name="bindFunc" />.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2">The type of the next <see cref="IResponseTransferObject{T2}" /> to return.</typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="bindFunc">The binding function.</param>
        /// <returns>Instance of <see cref="IResponseTransferObject{T2}" />.</returns>
        public static Task<IResponseTransferObject<T2>> BindAsync<T, T2>(
            this IResponseTransferObject<T> responseTransferObject,
            Func<T, Task<IResponseTransferObject<T2>>> bindFunc)
        {
            if (responseTransferObject.Error != null)
            {
                var tcs = new TaskCompletionSource<IResponseTransferObject<T2>>();
                tcs.SetResult(new ServiceResponse<T2>(responseTransferObject.Error));

                return tcs.Task;
            }

            return bindFunc(responseTransferObject.Data);
        }

        /// <summary>
        /// Invokes the specified function if <see cref="IResponseTransferObject{T}.Error"/> is not null. Allows you to re-direct control flow with a new <typeparamref name="T" /> value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="continueWithFunction">The continue with function.</param>
        /// <returns>If errors exist, returns the instance of IResponseTransferObject{T} returned by <paramref name="continueWithFunction" />, else returns current instance.</returns>
        public static Task<IResponseTransferObject<T>> CatchAndContinueAsync<T>(
            this IResponseTransferObject<T> responseTransferObject, 
            Func<Error, Task<IResponseTransferObject<T>>> continueWithFunction)
        {
            if (responseTransferObject.Error != null)
            {
                return continueWithFunction.Invoke(responseTransferObject.Error);
            }

            var tcs = new TaskCompletionSource<IResponseTransferObject<T>>();
            tcs.SetResult(responseTransferObject);

            return tcs.Task;
        }

        /// <summary>
        /// Invokes the specified function if <see cref="IResponseTransferObject{T}.Error"/> is not null. Allows you to re-direct control flow with a new <typeparamref name="T" /> value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="continueWithFunction">The continue with function.</param>
        /// <returns>If errors exist, returns the instance of IResponseTransferObject{T} returned by <paramref name="continueWithFunction" />, else returns current instance.</returns>
        public static Task<IResponseTransferObject<T>> CatchAndContinueAsync<T>(
            this IResponseTransferObject<T> responseTransferObject, 
            Func<Error, T> continueWithFunction)
        {
            var tcs = new TaskCompletionSource<IResponseTransferObject<T>>();
            if (responseTransferObject.Error != null)
            {
                T result = continueWithFunction.Invoke(responseTransferObject.Error);

                tcs.SetResult(IResponseTransferObjectExtensions.CreateGenericServiceResponse<T>(responseTransferObject, result));
            }
            else
            {
                tcs.SetResult(responseTransferObject);
            }

            return tcs.Task;
        }

        /// <summary>
        /// If <seealso cref="IResponseTransferObject{T}.Error" /> is not null, returns a new <see cref="IResponseTransferObject{T2}" /> instance with the current
        /// <seealso cref="IResponseTransferObject{T}.Error" />. Else, binds the <seealso cref="IResponseTransferObject{T}.Data" /> into the specified <paramref name="mappingFunction" />.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2">The type of the next <see cref="IResponseTransferObject{T2}" /> to return.</typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="mappingFunction">The mapping function.</param>
        /// <returns>Instance of <see cref="IResponseTransferObject{T2}" />.</returns>
        public static Task<IResponseTransferObject<T2>> FmapAsync<T, T2>(this IResponseTransferObject<T> responseTransferObject, Func<T, T2> mappingFunction)
        {
            var tcs = new TaskCompletionSource<IResponseTransferObject<T2>>();
            if (responseTransferObject.Error != null)
            {
                tcs.SetResult(IResponseTransferObjectExtensions.CreateGenericServiceResponse<T, T2>(responseTransferObject, responseTransferObject.Error));
            }
            else
            {
                T2 result = mappingFunction.Invoke(responseTransferObject.Data);
                tcs.SetResult(IResponseTransferObjectExtensions.CreateGenericServiceResponse(responseTransferObject, result));
            }

            return tcs.Task;
        }
    }
}