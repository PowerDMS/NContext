namespace NContext.Common
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    
    public static class IServiceResponseAwaitExtensions
    {
        /// <summary>
        /// Awaits the specified <paramref name="serviceResponseFuture"/>.
        /// If the awaited <paramref name="serviceResponseFuture"/> isLeft, returns a new <see cref="IServiceResponse{T2}" /> instance with the current
        /// <seealso cref="IServiceResponse{T}.Error" />. Else, binds the <seealso cref="IServiceResponse{T}.Data" /> into the 
        /// specified <paramref name="bindFunc" />.
        /// </summary>
        /// <typeparam name="T">The type of the current <see cref="IServiceResponse{T}"/>.</typeparam>
        /// <typeparam name="T2">The type of the next <see cref="IServiceResponse{T2}" /> to return.</typeparam>
        /// <param name="serviceResponseFuture">The service response task.</param>
        /// <param name="bindFunc">The binding function.</param>
        /// <returns>Instance of <see cref="IServiceResponse{T2}" />.</returns>
        public static async Task<IServiceResponse<T2>> AwaitBindAsync<T, T2>(
            this Task<IServiceResponse<T>> serviceResponseFuture, 
            Func<T, IServiceResponse<T2>> bindFunc)
        {
            var serviceResponse = await serviceResponseFuture;
            return serviceResponse.Bind(bindFunc);
        }

        /// <summary>
        /// Awaits the specified <paramref name="serviceResponseFuture"/>. 
        /// If the awaited <paramref name="serviceResponseFuture"/> isLeft, returns a new <see cref="IServiceResponse{T2}" /> instance 
        /// with the current <seealso cref="IServiceResponse{T}.Error" />. Else, binds the <seealso cref="IServiceResponse{T}.Data" /> into the 
        /// specified <paramref name="bindFunc" /> and awaits it.
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
            return await serviceResponse.BindAsync(bindFunc);
        }

        public static async Task<IServiceResponse<IEnumerable<T2>>> AwaitBindManyAsync<T, T2>(
            this Task<IServiceResponse<IEnumerable<T>>> serviceResponseFuture, 
            Func<T, Task<IServiceResponse<T2>>> bindFunc)
        {
            var serviceResponse = await serviceResponseFuture;
            return await serviceResponse.BindManyAsync(bindFunc);
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
            return await serviceResponse.LetAsync(letFunc);
        }

        /// <summary>
        /// Awaits the specified <paramref name="serviceResponseFuture"/>. Invokes the specified 
        /// function if the future result <see cref="IServiceResponse{T}.Error" /> is null.
        /// Returns the current <see cref="IServiceResponse{T}" /> instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResponseFuture">The future service response.</param>
        /// <param name="letAction"></param>
        /// <returns>Task&lt;IServiceResponse&lt;T&gt;&gt;.</returns>
        public static async Task<IServiceResponse<T>> AwaitLetAsync<T>(
            this Task<IServiceResponse<T>> serviceResponseFuture,
            Action<T> letAction)
        {
            var serviceResponse = await serviceResponseFuture;
            return await serviceResponse.LetAsync(letAction);
        }

        /// <summary>
        /// Awaits the specified <paramref name="serviceResponseFuture"/>.
        /// If the awaited <paramref name="serviceResponseFuture"/>.IsLeft, returns a new <see cref="IServiceResponse{T2}" /> instance with the current
        /// <paramref name="serviceResponseFuture"/>.Error. Otherwise, it binds the <paramref name="serviceResponseFuture"/>.Data into 
        /// the specified <paramref name="fmapFunc" />.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2">The type of the t2.</typeparam>
        /// <param name="serviceResponseFuture">The service response future.</param>
        /// <param name="fmapFunc">The fmap function.</param>
        /// <returns>Task&lt;IServiceResponse&lt;T2&gt;&gt;.</returns>
        public static async Task<IServiceResponse<T2>> AwaitFmapAsync<T, T2>(
            this Task<IServiceResponse<T>> serviceResponseFuture, 
            Func<T, T2> fmapFunc)
        {
            var serviceResponse = await serviceResponseFuture;
            return serviceResponse.Fmap(fmapFunc);
        }

        /// <summary>
        /// Awaits the specified <paramref name="serviceResponseFuture"/>.
        /// Invokes the specified <paramref name="catchFunc"/> function if the awaited <paramref name="serviceResponseFuture"/>.IsLeft. 
        /// Returns the awaited <paramref name="serviceResponseFuture"/> unless the <paramref name="catchFunc"/> returns a faulted task.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResponseFuture">The service response task.</param>
        /// <param name="catchFunc">Async function to invoke.</param>
        /// <returns>The awaited <paramref name="serviceResponseFuture"/> unless the <paramref name="catchFunc"/> returns a faulted task.</returns>
        public static async Task<IServiceResponse<T>> AwaitCatchAsync<T>(
            this Task<IServiceResponse<T>> serviceResponseFuture, 
            Func<Error, Task> catchFunc)
        {
            var serviceResponse = await serviceResponseFuture;
            return await serviceResponse.CatchAsync(catchFunc);
        }

        /// <summary>
        /// Awaits the specified <paramref name="serviceResponseFuture"/>.
        /// Invokes the specified <paramref name="continueWithFunction"/> function if the awaited <paramref name="serviceResponseFuture"/>.IsLeft. 
        /// Allows you to re-direct control flow with a new <typeparamref name="T" /> value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResponseFuture">The service response task.</param>
        /// <param name="continueWithFunction">The continue with function.</param>
        /// <returns>If <paramref name="serviceResponseFuture"/>.IsLeft, then the instance of <see cref="IServiceResponse{T}" /> 
        /// returned by <paramref name="continueWithFunction" />, else returns current <paramref name="serviceResponseFuture"/>.</returns>
        public static async Task<IServiceResponse<T>> AwaitCatchAndContinueAsync<T>(
            this Task<IServiceResponse<T>> serviceResponseFuture,
            Func<Error, IServiceResponse<T>> continueWithFunction)
        {
            var serviceResponse = await serviceResponseFuture;
            return serviceResponse.CatchAndContinue(continueWithFunction);
        }

        /// <summary>
        /// Awaits the specified <paramref name="serviceResponseFuture"/>.
        /// Invokes the specified <paramref name="continueWithFunction"/> function if the awaited <paramref name="serviceResponseFuture"/>.IsLeft. 
        /// Allows you to re-direct control flow with a new <typeparamref name="T" /> value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResponseFuture">The service response task.</param>
        /// <param name="continueWithFunction">The continue with function.</param>
        /// <returns>If <paramref name="serviceResponseFuture"/>.IsLeft, then the instance of <see cref="IServiceResponse{T}" /> 
        /// returned by <paramref name="continueWithFunction" />, else returns current <paramref name="serviceResponseFuture"/>.</returns>
        public static async Task<IServiceResponse<T>> AwaitCatchAndContinueAsync<T>(
            this Task<IServiceResponse<T>> serviceResponseFuture,
            Func<Error, Task<IServiceResponse<T>>> continueWithFunction)
        {
            var serviceResponse = await serviceResponseFuture;
            return await serviceResponse.CatchAndContinueAsync(continueWithFunction);
        }
    }
}