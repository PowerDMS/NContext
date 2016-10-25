namespace NContext.Common
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines async extension methods for <see cref="IServiceResponse{T}"/>.
    /// </summary>
    public static class IServiceResponseAsyncExtensions
    {
        /// <summary>
        /// If <seealso cref="IServiceResponse{T}.IsLeft" />, returns a new <see cref="IServiceResponse{T2}" /> instance with the current
        /// <seealso cref="IServiceResponse{T}.Error" />. Else, binds the <seealso cref="IServiceResponse{T}.Data" /> into the 
        /// specified <paramref name="bindFunc" />.
        /// </summary>
        /// <typeparam name="T">The type of the current <see cref="IServiceResponse{T}"/>.</typeparam>
        /// <typeparam name="T2">The type of the next <see cref="IServiceResponse{T2}" /> to return.</typeparam>
        /// <param name="serviceResponse">The service response.</param>
        /// <param name="bindFunc">The binding function.</param>
        /// <returns>Instance of <see cref="IServiceResponse{T2}" />.</returns>
        public static Task<IServiceResponse<T2>> BindAsync<T, T2>(
            this IServiceResponse<T> serviceResponse,
            Func<T, Task<IServiceResponse<T2>>> bindFunc)
        {
            if (serviceResponse.IsLeft)
            {
                return Task.FromResult(serviceResponse.CreateGenericErrorResponse<T, T2>(serviceResponse.GetLeft()));
            }

            return bindFunc(serviceResponse.GetRight());
        }

        public static async Task<IServiceResponse<IEnumerable<T2>>> BindManyAsync<T, T2>(
            this IServiceResponse<IEnumerable<T>> serviceResponse,
            Func<T, Task<IServiceResponse<T2>>> bindFunc)
        {
            if (serviceResponse.IsLeft)
            {
                return serviceResponse.CreateGenericErrorResponse<IEnumerable<T>, IEnumerable<T2>>(serviceResponse.GetLeft());
            }

            var result = new List<T2>();
            foreach (var element in serviceResponse.GetRight())
            {
                var elementResponse = await bindFunc(element);
                if (elementResponse.IsLeft)
                {
                    return serviceResponse.CreateGenericErrorResponse<IEnumerable<T>, IEnumerable<T2>>(elementResponse.GetLeft());
                }

                result.Add(elementResponse.GetRight());
            }

            return serviceResponse.CreateGenericDataResponse(result);
        }

        /// <summary>
        /// Invokes the specified function if <see cref="IServiceResponse{T}.IsLeft"/>. Returns the current <paramref name="serviceResponse"/> 
        /// unless the <paramref name="catchFunc"/> returns a faulted task. <see cref="Task.IsFaulted"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResponse">The service response.</param>
        /// <param name="catchFunc">Async function to invoke.</param>
        /// <returns>The current <paramref name="serviceResponse"/> unless the <paramref name="catchFunc"/> returns a faulted task.</returns>
        public static Task<IServiceResponse<T>> CatchAsync<T>(
            this IServiceResponse<T> serviceResponse,
            Func<Error, Task> catchFunc)
        {
            if (serviceResponse.IsLeft)
            {
                return catchFunc.Invoke(serviceResponse.GetLeft())
                    .ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                            return serviceResponse.CreateGenericErrorResponse(task.Exception.ToError());
                        }

                        return serviceResponse;
                    });
            }

            return Task.FromResult(serviceResponse);
        }

        /// <summary>
        /// Invokes the specified <paramref name="continueWithFunction"/> function if <see cref="IServiceResponse{T}.IsLeft"/>. 
        /// Allows you to re-direct control flow with a new <typeparamref name="T" /> value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResponse">The service response.</param>
        /// <param name="continueWithFunction">The continue with function.</param>
        /// <returns>If <paramref name="serviceResponse"/>.IsLeft, then the instance of <see cref="IServiceResponse{T}" /> 
        /// returned by <paramref name="continueWithFunction" />, else returns current <paramref name="serviceResponse"/>.</returns>
        public static Task<IServiceResponse<T>> CatchAndContinueAsync<T>(
            this IServiceResponse<T> serviceResponse, 
            Func<Error, Task<IServiceResponse<T>>> continueWithFunction)
        {
            if (serviceResponse.IsLeft)
            {
                return continueWithFunction.Invoke(serviceResponse.GetLeft());
            }

            return Task.FromResult(serviceResponse);
        }

        /// <summary>
        /// Invokes the specified <paramref name="continueWithFunction"/> function if <see cref="IServiceResponse{T}.IsLeft"/>. 
        /// Allows you to re-direct control flow with a new <typeparamref name="T" /> value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResponse">The service response.</param>
        /// <param name="continueWithFunction">The continue with function.</param>
        /// <returns>If <paramref name="serviceResponse"/>.IsLeft, then the instance of <see cref="IServiceResponse{T}" /> 
        /// returned by <paramref name="continueWithFunction" />, else returns current <paramref name="serviceResponse"/>.</returns>
        public static Task<IServiceResponse<T>> CatchAndContinueAsync<T>(
            this IServiceResponse<T> serviceResponse, 
            Func<Error, T> continueWithFunction)
        {
            var tcs = new TaskCompletionSource<IServiceResponse<T>>();
            if (serviceResponse.IsLeft)
            {
                T result = continueWithFunction.Invoke(serviceResponse.GetLeft());

                tcs.SetResult(new DataResponse<T>(result));
            }
            else
            {
                tcs.SetResult(serviceResponse);
            }

            return tcs.Task;
        }

        /// <summary>
        /// Invokes the specified action if <see cref="IServiceResponse{T}.IsRight" />.
        /// Returns the current <paramref name="serviceResponse"/> instance unless <paramref name="letFunc"/> 
        /// returns a faulted task. <see cref="Task.IsFaulted"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResponse">The service response.</param>
        /// <param name="letFunc">The let function.</param>
        /// <returns>Returns the current <paramref name="serviceResponse"/> instance unless 
        /// <paramref name="letFunc"/> returns a faulted task. <see cref="Task.IsFaulted"/></returns>
        public static Task<IServiceResponse<T>> LetAsync<T>(
            this IServiceResponse<T> serviceResponse,
            Func<T, Task> letFunc)
        {
            if (serviceResponse.IsLeft)
            {
                return Task.FromResult(serviceResponse.CreateGenericErrorResponse(serviceResponse.GetLeft()));
            }

            return letFunc(serviceResponse.GetRight())
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        return new ErrorResponse<T>(task.Exception.ToError());
                    }

                    return serviceResponse;
                });
        }

        /// <summary>
        /// Invokes the specified action if <paramref name="serviceResponse"/>.<see cref="IServiceResponse{T}.IsRight" />.
        /// Returns the current <paramref name="serviceResponse"/> instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResponse">The service response.</param>
        /// <param name="letAction">The let function.</param>
        /// <returns>Returns the current <paramref name="serviceResponse"/> instance unless 
        /// <paramref name="letAction"/> returns a faulted task. <see cref="Task.IsFaulted"/></returns>
        public static Task<IServiceResponse<T>> LetAsync<T>(
            this IServiceResponse<T> serviceResponse,
            Action<T> letAction)
        {
            if (serviceResponse.IsLeft)
            {
                return Task.FromResult(serviceResponse);
            }

            // TODO: (DG) This can yield unexpected results of the action is an asyncVoid and throws an exception!
            letAction(serviceResponse.GetRight());

            return Task.FromResult(serviceResponse);
        }
    }
}