namespace NContext.Common
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines extension methods for <see cref="IServiceResponse{T}"/>.
    /// </summary>
    public static class IServiceResponseAsyncExtensions
    {
        /// <summary>
        /// If <seealso cref="IServiceResponse{T}.Error" /> is not null, returns a new <see cref="IServiceResponse{T2}" /> instance with the current
        /// <seealso cref="IServiceResponse{T}.Error" />. Else, binds the <seealso cref="IServiceResponse{T}.Data" /> into the specified <paramref name="bindFunc" />.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2">The type of the next <see cref="IServiceResponse{T2}" /> to return.</typeparam>
        /// <param name="serviceResponse">The service response.</param>
        /// <param name="bindFunc">The binding function.</param>
        /// <returns>Instance of <see cref="IServiceResponse{T2}" />.</returns>
        public static Task<IServiceResponse<T2>> BindAsync<T, T2>(
            this IServiceResponse<T> serviceResponse,
            Func<T, Task<IServiceResponse<T2>>> bindFunc)
        {
            if (serviceResponse.Error != null)
            {
                var tcs = new TaskCompletionSource<IServiceResponse<T2>>();
                tcs.SetResult(serviceResponse.CreateGenericErrorResponse<T, T2>(serviceResponse.Error));

                return tcs.Task;
            }

            return bindFunc(serviceResponse.Data);
        }

        /// <summary>
        /// Invokes the specified function if <see cref="IServiceResponse{T}.Error"/> is not null. Allows you to re-direct control flow with a new <typeparamref name="T" /> value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResponse">The service response.</param>
        /// <param name="continueWithFunction">The continue with function.</param>
        /// <returns>If errors exist, returns the instance of IServiceResponse{T} returned by <paramref name="continueWithFunction" />, else returns current instance.</returns>
        public static Task<IServiceResponse<T>> CatchAndContinueAsync<T>(
            this IServiceResponse<T> serviceResponse, 
            Func<Error, Task<IServiceResponse<T>>> continueWithFunction)
        {
            if (serviceResponse.Error != null)
            {
                return continueWithFunction.Invoke(serviceResponse.Error);
            }

            var tcs = new TaskCompletionSource<IServiceResponse<T>>();
            tcs.SetResult(serviceResponse);

            return tcs.Task;
        }

        /// <summary>
        /// Invokes the specified function if <see cref="IServiceResponse{T}.Error"/> is not null. Allows you to re-direct control flow with a new <typeparamref name="T" /> value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResponse">The service response.</param>
        /// <param name="continueWithFunction">The continue with function.</param>
        /// <returns>If errors exist, returns the instance of IServiceResponse{T} returned by <paramref name="continueWithFunction" />, else returns current instance.</returns>
        public static Task<IServiceResponse<T>> CatchAndContinueAsync<T>(
            this IServiceResponse<T> serviceResponse, 
            Func<Error, T> continueWithFunction)
        {
            var tcs = new TaskCompletionSource<IServiceResponse<T>>();
            if (serviceResponse.Error != null)
            {
                T result = continueWithFunction.Invoke(serviceResponse.Error);

                tcs.SetResult(serviceResponse.CreateGenericDataResponse<T>(result));
            }
            else
            {
                tcs.SetResult(serviceResponse);
            }

            return tcs.Task;
        }

        /// <summary>
        /// Invokes the specified action if <see cref="IServiceResponse{T}.Error" /> is null.
        /// Returns the current <see cref="IServiceResponse{T}" /> instance unless the task 
        /// from <paramref name="letFunc"/> <see cref="Task.IsFaulted"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResponse">The service response.</param>
        /// <param name="letFunc">The let function.</param>
        /// <returns>Task&lt;IServiceResponse&lt;T&gt;&gt;.</returns>
        public static Task<IServiceResponse<T>> LetAsync<T>(
            this IServiceResponse<T> serviceResponse,
            Func<T, Task> letFunc)
        {
            if (serviceResponse.Error != null)
            {
                return Task.FromResult(serviceResponse.CreateGenericErrorResponse(serviceResponse.Error));
            }

            return letFunc(serviceResponse.Data)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        return serviceResponse.CreateGenericErrorResponse(task.Exception.ToError());
                    }

                    return serviceResponse.CreateGenericDataResponse<T>(serviceResponse.Data);
                });
        }
    }
}