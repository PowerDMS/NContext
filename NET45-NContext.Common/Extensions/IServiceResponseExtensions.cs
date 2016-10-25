namespace NContext.Common
{
    using System;

    /// <summary>
    /// Defines extension methods for <see cref="IServiceResponse{T}"/>.
    /// </summary>
    public static class IServiceResponseExtensions
    {
        /// <summary>
        /// If <paramref name="serviceResponse"/>.IsLeft, returns a new <see cref="IServiceResponse{T2}" /> instance with the current
        /// <paramref name="serviceResponse"/>.Error. Otherwise, it binds the <paramref name="serviceResponse"/>.Data into the specified 
        /// <paramref name="bindingFunction" />.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2">The type of the next <see cref="IServiceResponse{T2}" /> to return.</typeparam>
        /// <param name="serviceResponse">The service response.</param>
        /// <param name="bindingFunction">The binding function.</param>
        /// <returns>Instance of <see cref="IServiceResponse{T2}" />.</returns>
        public static IServiceResponse<T2> Bind<T, T2>(this IServiceResponse<T> serviceResponse, Func<T, IServiceResponse<T2>> bindingFunction)
        {
            if (serviceResponse.IsLeft)
            {
                return serviceResponse.CreateGenericErrorResponse<T, T2>(serviceResponse.GetLeft());
            }

            return bindingFunction.Invoke(serviceResponse.GetRight());
        }

        /// <summary>
        /// Invokes the specified action if <paramref name="serviceResponse"/>.IsLeft. 
        /// Returns the current instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResponse">The service response.</param>
        /// <param name="action">The action to invoke.</param>
        /// <returns>The current <see cref="IServiceResponse{T}" /> instance.</returns>
        public static IServiceResponse<T> Catch<T>(this IServiceResponse<T> serviceResponse, Action<Error> action)
        {
            if (serviceResponse.IsLeft)
            {
                action.Invoke(serviceResponse.GetLeft());
            }

            return serviceResponse;
        }

        /// <summary>
        /// Invokes the specified <paramref name="continueWithFunction"/> function if <paramref name="serviceResponse"/>.IsLeft. 
        /// Allows you to re-direct control flow with a new <typeparamref name="T" /> value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResponse">The service response.</param>
        /// <param name="continueWithFunction">The continue with function.</param>
        /// <returns>If errors exist, returns the instance of IServiceResponse{T} returned by <paramref name="continueWithFunction" />, else returns current instance.</returns>
        public static IServiceResponse<T> CatchAndContinue<T>(this IServiceResponse<T> serviceResponse, Func<Error, IServiceResponse<T>> continueWithFunction)
        {
            if (serviceResponse.IsLeft)
            {
                return continueWithFunction.Invoke(serviceResponse.GetLeft());
            }

            return serviceResponse;
        }

        /// <summary>
        /// Invokes the specified <paramref name="continueWithFunction"/> function if <paramref name="serviceResponse"/>.IsLeft. 
        /// Allows you to re-direct control flow with a new <typeparamref name="T" /> value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResponse">The service response.</param>
        /// <param name="continueWithFunction">The continue with function.</param>
        /// <returns>If errors exist, returns the instance of IServiceResponse{T} returned by <paramref name="continueWithFunction" />, else returns current instance.</returns>
        public static IServiceResponse<T> CatchAndContinue<T>(this IServiceResponse<T> serviceResponse, Func<Error, T> continueWithFunction)
        {
            if (serviceResponse.IsLeft)
            {
                T result = continueWithFunction.Invoke(serviceResponse.GetLeft());

                return new DataResponse<T>(result);
            }

            return serviceResponse;
        }

        /// <summary>
        /// Invokes the specified action if <paramref name="serviceResponse"/>.IsRight.
        /// Returns the current <see cref="IServiceResponse{T}" /> instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResponse">The service response.</param>
        /// <param name="action">The action to invoke.</param>
        /// <returns>The current <see cref="IServiceResponse{T}" /> instance.</returns>
        public static IServiceResponse<T> Let<T>(this IServiceResponse<T> serviceResponse, Action<T> action)
        {
            if (serviceResponse.IsRight)
            {
                action.Invoke(serviceResponse.GetRight());
            }

            return serviceResponse;
        }

        /// <summary>
        /// If <paramref name="serviceResponse"/>.IsLeft, returns a new <see cref="IServiceResponse{T2}" /> instance with the current
        /// <paramref name="serviceResponse"/>.Error. Otherwise, it binds the <paramref name="serviceResponse"/>.Data into 
        /// the specified <paramref name="mappingFunction" />.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2">The type of the next <see cref="IServiceResponse{T2}" /> to return.</typeparam>
        /// <param name="serviceResponse">The service response.</param>
        /// <param name="mappingFunction">The mapping function.</param>
        /// <returns>Instance of <see cref="IServiceResponse{T2}" />.</returns>
        public static IServiceResponse<T2> Fmap<T, T2>(this IServiceResponse<T> serviceResponse, Func<T, T2> mappingFunction)
        {
            if (serviceResponse.IsLeft)
            {
                return serviceResponse.CreateGenericErrorResponse<T, T2>(serviceResponse.GetLeft());
            }

            T2 result = mappingFunction.Invoke(serviceResponse.GetRight());

            return serviceResponse.CreateGenericDataResponse(result);
        }

        /// <summary>
        /// Invokes the specified action whether <paramref name="serviceResponse"/>.IsLeft or <paramref name="serviceResponse"/>.IsRight.
        /// Returns the current <paramref name="serviceResponse"/> instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResponse">The service response.</param>
        /// <param name="action">The action to invoke.</param>
        /// <returns>The current <see cref="IServiceResponse{T}" /> instance.</returns>
        public static IServiceResponse<T> Run<T>(this IServiceResponse<T> serviceResponse, Action action)
        {
            action.Invoke();

            return serviceResponse;
        }
        
        /// <summary>
        /// Returns the error.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResponse">The service response.</param>
        /// <returns>Error.</returns>
        /// <exception cref="System.InvalidOperationException">There is nothing to return from left of either - Error or Data.</exception>
        public static Error FromLeft<T>(this IServiceResponse<T> serviceResponse)
        {
            if (!serviceResponse.IsLeft)
            {
                throw new InvalidOperationException("Cannot return left of either when is right.");
            }

            return serviceResponse.GetLeft();
        }

        /// <summary>
        /// Returns the value of <typeparamref name="T"/> if there is no error.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResponse">The service response.</param>
        /// <returns><typeparamref name="T"/>.</returns>
        /// <exception cref="System.InvalidOperationException">Cannot return right of either when left (errors) exist.</exception>
        public static T FromRight<T>(this IServiceResponse<T> serviceResponse)
        {
            if (!serviceResponse.IsRight)
            {
                throw new InvalidOperationException("Cannot return right of either when is left.");
            }

            return serviceResponse.GetRight();
        }
    }
}