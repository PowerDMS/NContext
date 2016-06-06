namespace NContext.Common.Extensions
{
    using System;
    using System.Threading.Tasks;

    public static class IServiceResponseAwaitExtensions
    {
        /// <summary>
        /// Awaits the specified <paramref name="serviceResponseFuture"/>. If <seealso cref="IServiceResponse{T}.Error" /> is not null, 
        /// returns a new <see cref="IServiceResponse{T2}" /> instance with the current <seealso cref="IServiceResponse{T}.Error" />. 
        /// Otherwise, it binds the <seealso cref="IServiceResponse{T}.Data" /> into the specified <paramref name="bindFunc" /> and awaits it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2">The type of the next <see cref="IServiceResponse{T2}" /> to return.</typeparam>
        /// <param name="serviceResponseFuture">The service response task.</param>
        /// <param name="bindFunc">The binding function.</param>
        /// <returns>Task&lt;IServiceResponse&lt;T2&gt;&gt;.</returns>
        public static async Task<IServiceResponse<T2>> AwaitBindAsync<T, T2>(
            this Task<IServiceResponse<T>> serviceResponseFuture,
            Func<T, Task<IServiceResponse<T2>>> bindFunc)
        {
            var serviceResponse = await serviceResponseFuture;
            if (serviceResponse.Error != null)
            {
                return serviceResponse.CreateGenericErrorResponse<T, T2>(serviceResponse.Error);
            }

            return await bindFunc(serviceResponse.Data);
        }

        /// <summary>
        /// Awaits the specified <paramref name="serviceResponseFuture"/>. Invokes the specified 
        /// function if the future result <see cref="IServiceResponse{T}.Error" /> is null.
        /// Returns the current <see cref="IServiceResponse{T}" /> instance unless the task 
        /// from <paramref name="letFunc"/> <see cref="Task.IsFaulted"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResponseFuture">The future service response.</param>
        /// <param name="letFunc">The let function.</param>
        /// <returns>Task&lt;IServiceResponse&lt;T&gt;&gt;.</returns>
        public static async Task<IServiceResponse<T>> AwaitLetAsync<T>(
            this Task<IServiceResponse<T>> serviceResponseFuture,
            Func<T, Task> letFunc)
        {
            var serviceResponse = await serviceResponseFuture;
            if (serviceResponse.Error != null)
            {
                return new ErrorResponse<T>(serviceResponse.Error);
            }
            
            return await letFunc(serviceResponse.Data)
                .ContinueWith<IServiceResponse<T>>(task =>
                {
                    if (task.IsFaulted)
                    {
                        return new ErrorResponse<T>(task.Exception.ToError());
                    }

                    return new DataResponse<T>(serviceResponse.Data);
                });
        }

        /// <summary>
        /// Invokes the specified action if <see cref="IServiceResponse{T}.Error" /> is null.
        /// Returns the current <see cref="IServiceResponse{T}" />.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResponseFuture">The future service response.</param>
        /// <param name="letAction">The let action.</param>
        /// <returns>Task&lt;IServiceResponse&lt;T&gt;&gt;.</returns>
        public static async Task<IServiceResponse<T>> AwaitLetAsync<T>(
            this Task<IServiceResponse<T>> serviceResponseFuture,
            Action<T> letAction)
        {
            var serviceResponse = await serviceResponseFuture;
            if (serviceResponse.Error != null)
            {
                return serviceResponse.CreateGenericErrorResponse(serviceResponse.Error);
            }

            letAction(serviceResponse.Data);

            return serviceResponse;
        }
    }
}