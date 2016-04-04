namespace NContext.Extensions
{
    using System;
    using System.Threading.Tasks;

    using NContext.Common;
    using NContext.EventHandling;

    /// <summary>
    /// Defines event handling extension methods for <see cref="IServiceResponse{T}"/>.
    /// </summary>
    public static class IServiceResponseEventExtensions
    {
        /// <summary>
        /// Raises the specified event if <paramref name="onRightOnly"/> is false or the <paramref name="serviceResponse"/>.IsRight. 
        /// Event handlers may be executed in parallel. All handlers are run even if one throws an exception. 
        /// If an exception is thrown, this method returns a new <see cref="ServiceResponse{T}"/> with an error representing the thrown exceptions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TEvent">The type of the T event.</typeparam>
        /// <param name="serviceResponse">The service response.</param>
        /// <param name="event">The event.</param>
        /// <param name="onRightOnly">Determines whether to raise the event in the case the serviceResponse is left.</param>
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
                    : serviceResponse)
                .Result;
        }

        /// <summary>
        /// Raises the specified event if <paramref name="onRightOnly"/> is false or the <paramref name="serviceResponse"/>.IsRight. 
        /// Event handlers may be executed in parallel. All handlers are run even if one throws an exception. 
        /// If an exception is thrown, this method returns a new <see cref="ServiceResponse{T}"/> with an error representing the thrown exceptions.
        /// </summary>
        /// <typeparam name="T"/><typeparam name="TEvent">The type of the T event.</typeparam>
        /// <param name="serviceResponse">The service response.</param>
        /// <param name="eventFactory">The function to create the event.</param>
        /// <param name="onRightOnly">Determines whether to raise the event in the case the serviceResponse is left.</param>
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
                    : serviceResponse)
                .Result;
        }


        /// <summary>
        /// Raises the specified event if <paramref name="onRightOnly"/> is false or the <paramref name="serviceResponse"/>.IsRight. 
        /// Event handlers may be executed in parallel. All handlers are run even if one throws an exception. 
        /// If an exception is thrown, this method returns a new <see cref="ServiceResponse{T}"/> with an error representing the thrown exceptions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TEvent">The type of the T event.</typeparam>
        /// <param name="serviceResponse">The service response.</param>
        /// <param name="event">The event.</param>
        /// <param name="onRightOnly">Determines whether to raise the event in the case the serviceResponse is left.</param>
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

            return eventManager.Raise(@event)
                .ContinueWith(task => task.IsFaulted
                    ? new ErrorResponse<T>(task.Exception.ToError())
                    : serviceResponse);
        }

        /// <summary>
        /// Raises the specified event if <paramref name="onRightOnly"/> is false or the <paramref name="serviceResponse"/>.IsRight. 
        /// Event handlers may be executed in parallel. All handlers are run even if one throws an exception. 
        /// If an exception is thrown, this method returns a new <see cref="ServiceResponse{T}"/> with an error representing the thrown exceptions.
        /// </summary>
        /// <typeparam name="T"/><typeparam name="TEvent">The type of the T event.</typeparam>
        /// <param name="serviceResponse">The service response.</param>
        /// <param name="eventFactory">The function to create the event.</param>
        /// <param name="onRightOnly">Determines whether to raise the event in the case the serviceResponse is left.</param>
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

            return eventManager.Raise(eventFactory(serviceResponse.Data))
                .ContinueWith(task => task.IsFaulted
                    ? new ErrorResponse<T>(task.Exception.ToError())
                    : serviceResponse);
        }
    }
}