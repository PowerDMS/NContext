namespace NContext.Extensions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using NContext.Common;
    using NContext.EventHandling;

    /// <summary>
    /// Defines event handling extension methods for <see cref="IServiceResponse{T}"/>.
    /// </summary>
    public static class IServiceResponseEventExtensions
    {
        /// <summary>
        /// Raises the specified event if <paramref name="onRightOnly" /> is false or the <paramref name="serviceResponse" />.IsRight.
        /// Event handlers may be executed in parallel. All handlers are run even if one throws an exception.
        /// If an exception is thrown, this method returns a new <see cref="ServiceResponse{T}" /> with an error representing the thrown exceptions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TEvent">The type of the T event.</typeparam>
        /// <param name="serviceResponse">The service response.</param>
        /// <param name="eventManager">The event manager.</param>
        /// <param name="event">The event.</param>
        /// <param name="onRightOnly">Determines whether to raise the event in the case the serviceResponse is left.</param>
        /// <returns>IServiceResponse&lt;T&gt;.</returns>
        public static IServiceResponse<T> Raise<T, TEvent>(
            this IServiceResponse<T> serviceResponse,
            IManageEvents eventManager,
            TEvent @event, 
            Boolean onRightOnly = true)
        {
            if (onRightOnly && serviceResponse.IsLeft)
            {
                return serviceResponse;
            }

            return eventManager.Raise(@event)
                .ContinueWith(task => task.IsFaulted
                    ? new ErrorResponse<T>(task.Exception.ToError())
                    : serviceResponse,
                    CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Default)
                .Result;
        }

        /// <summary>
        /// Raises the specified event if <paramref name="onRightOnly" /> is false or the <paramref name="serviceResponse" />.IsRight.
        /// Event handlers may be executed in parallel. All handlers are run even if one throws an exception.
        /// If an exception is thrown, this method returns a new <see cref="ServiceResponse{T}" /> with an error representing the thrown exceptions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TEvent">The type of the T event.</typeparam>
        /// <param name="serviceResponse">The service response.</param>
        /// <param name="eventManager">The event manager.</param>
        /// <param name="eventFactory">The function to create the event.</param>
        /// <param name="onRightOnly">Determines whether to raise the event in the case the serviceResponse is left.</param>
        /// <returns>IServiceResponse&lt;T&gt;.</returns>
        public static IServiceResponse<T> Raise<T, TEvent>(
            this IServiceResponse<T> serviceResponse, 
            IManageEvents eventManager,
            Func<T, TEvent> eventFactory, 
            Boolean onRightOnly = true)
        {
            if (onRightOnly && serviceResponse.IsLeft)
            {
                return serviceResponse;
            }

            return eventManager.Raise(eventFactory(serviceResponse.Data))
                .ContinueWith(task => task.IsFaulted
                    ? new ErrorResponse<T>(task.Exception.ToError())
                    : serviceResponse,
                    CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Default)
                .Result;
        }


        /// <summary>
        /// Raises the specified event if <paramref name="onRightOnly" /> is false or the <paramref name="serviceResponse" />.IsRight.
        /// Event handlers may be executed in parallel. All handlers are run even if one throws an exception.
        /// If an exception is thrown, this method returns a new <see cref="ServiceResponse{T}" /> with an error representing the thrown exceptions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TEvent">The type of the T event.</typeparam>
        /// <param name="serviceResponse">The service response.</param>
        /// <param name="eventManager">The event manager.</param>
        /// <param name="event">The event.</param>
        /// <param name="onRightOnly">Determines whether to raise the event in the case the serviceResponse is left.</param>
        /// <returns>Task&lt;IServiceResponse&lt;T&gt;&gt;.</returns>
        public static Task<IServiceResponse<T>> RaiseAsync<T, TEvent>(
            this IServiceResponse<T> serviceResponse,
            IManageEvents eventManager,
            TEvent @event,
            Boolean onRightOnly = true)
        {
            if (onRightOnly && serviceResponse.IsLeft)
            {
                return Task.FromResult(serviceResponse);
            }

            return serviceResponse.RaiseAsync(eventManager, _ => @event, onRightOnly);
        }

        /// <summary>
        /// Raises the specified event if <paramref name="onRightOnly" /> is false or the <paramref name="serviceResponse" />.IsRight.
        /// Event handlers may be executed in parallel. All handlers are run even if one throws an exception.
        /// If an exception is thrown, this method returns a new <see cref="ServiceResponse{T}" /> with an error representing the thrown exceptions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TEvent">The type of the T event.</typeparam>
        /// <param name="serviceResponse">The service response.</param>
        /// <param name="eventManager">The event manager.</param>
        /// <param name="eventFactory">The function to create the event.</param>
        /// <param name="onRightOnly">Determines whether to raise the event in the case the serviceResponse is left.</param>
        /// <returns>Task&lt;IServiceResponse&lt;T&gt;&gt;.</returns>
        public static Task<IServiceResponse<T>> RaiseAsync<T, TEvent>(
            this IServiceResponse<T> serviceResponse,
            IManageEvents eventManager,
            Func<T, TEvent> eventFactory,
            Boolean onRightOnly = true)
        {
            if (onRightOnly && serviceResponse.IsLeft)
            {
                return Task.FromResult(serviceResponse);
            }

            var eventParam = serviceResponse.IsRight
                ? serviceResponse.GetRight()
                : default(T);

            return eventManager.Raise(eventFactory(eventParam))
                .ContinueWith(task => task.IsFaulted
                    ? new ErrorResponse<T>(task.Exception.ToError())
                    : serviceResponse,
                    CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Default);
        }

        public static async Task<IServiceResponse<T>> AwaitRaiseAsync<T, TEvent>(
           this Task<IServiceResponse<T>> serviceResponseFuture, 
           IManageEvents eventManager, 
           TEvent @event,
           bool onRightOnly = true)
        {
            var serviceResponse = await serviceResponseFuture;
            return await serviceResponse.RaiseAsync(eventManager, @event, onRightOnly);
        }

        public static async Task<IServiceResponse<T>> AwaitRaiseAsync<T, TEvent>(
            this Task<IServiceResponse<T>> serviceResponseFuture, 
            IManageEvents eventManager, 
            Func<T, TEvent> eventFactory,
            bool onRightOnly = true)
        {
            var serviceResponse = await serviceResponseFuture;
            return await serviceResponse.RaiseAsync(eventManager, eventFactory, onRightOnly);
        }
    }
}