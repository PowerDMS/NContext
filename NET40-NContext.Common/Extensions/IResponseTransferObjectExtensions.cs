// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IResponseTransferObjectExtensions.cs" company="Waking Venture, Inc.">
//   Copyright (c) 2012 Waking Venture, Inc.
//
//   Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//   documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
//   the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
//   and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
//   The above copyright notice and this permission notice shall be included in all copies or substantial portions 
//   of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//   TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//   CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//   DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.Common
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Defines extension methods for <see cref="IResponseTransferObject{T}"/>.
    /// </summary>
    public static class IResponseTransferObjectExtensions
    {
        /// <summary>
        /// If <seealso cref="IResponseTransferObject{T}.Error" /> is not null, returns a new <see cref="IResponseTransferObject{T2}" /> instance with the current
        /// <seealso cref="IResponseTransferObject{T}.Error" />. Else, binds the <seealso cref="IResponseTransferObject{T}.Data" /> into the specified <paramref name="bindingFunction" />.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2">The type of the next <see cref="IResponseTransferObject{T2}" /> to return.</typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="bindingFunction">The binding function.</param>
        /// <returns>Instance of <see cref="IResponseTransferObject{T2}" />.</returns>
        public static IResponseTransferObject<T2> Bind<T, T2>(this IResponseTransferObject<T> responseTransferObject, Func<T, IResponseTransferObject<T2>> bindingFunction)
        {
            if (responseTransferObject.Error != null)
            {
                return CreateGenericServiceResponse<T, T2>(responseTransferObject, responseTransferObject.Error);
            }

            return bindingFunction.Invoke(responseTransferObject.Data);
        }

        /// <summary>
        /// Invokes the specified action if <seealso cref="IResponseTransferObject{T}.Error" /> is not null. Returns the current instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="action">The action to invoke.</param>
        /// <returns>The current <see cref="IResponseTransferObject{T}" /> instance.</returns>
        public static IResponseTransferObject<T> Catch<T>(this IResponseTransferObject<T> responseTransferObject, Action<Error> action)
        {
            if (responseTransferObject.Error != null)
            {
                action.Invoke(responseTransferObject.Error);
            }

            return responseTransferObject;
        }

        /// <summary>
        /// Invokes the specified function if <seealso cref="IResponseTransferObject{T}.Error" /> is not null. Allows you to re-direct control flow with a new <typeparamref name="T" /> value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="continueWithFunction">The continue with function.</param>
        /// <returns>If errors exist, returns the instance of IResponseTransferObject{T} returned by <paramref name="continueWithFunction" />, else returns current instance.</returns>
        public static IResponseTransferObject<T> CatchAndContinue<T>(this IResponseTransferObject<T> responseTransferObject, Func<Error, IResponseTransferObject<T>> continueWithFunction)
        {
            if (responseTransferObject.Error != null)
            {
                return continueWithFunction.Invoke(responseTransferObject.Error);
            }

            return responseTransferObject;
        }
        
        /// <summary>
        /// Invokes the specified function if <seealso cref="IResponseTransferObject{T}.Error" /> is not null. Allows you to re-direct control flow with a new <typeparamref name="T" /> value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="continueWithFunction">The continue with function.</param>
        /// <returns>If errors exist, returns the instance of IResponseTransferObject{T} returned by <paramref name="continueWithFunction" />, else returns current instance.</returns>
        public static IResponseTransferObject<T> CatchAndContinue<T>(this IResponseTransferObject<T> responseTransferObject, Func<Error, T> continueWithFunction)
        {
            if (responseTransferObject.Error != null)
            {
                T result = continueWithFunction.Invoke(responseTransferObject.Error);

                return CreateGenericServiceResponse<T>(responseTransferObject, result);
            }

            return responseTransferObject;
        }

        /// <summary>
        /// Invokes the specified action if <see cref="IResponseTransferObject{T}.Error" /> is null.
        /// Returns the current <see cref="IResponseTransferObject{T}" /> instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="action">The action to invoke.</param>
        /// <returns>The current <see cref="IResponseTransferObject{T}" /> instance.</returns>
        public static IResponseTransferObject<T> Let<T>(this IResponseTransferObject<T> responseTransferObject, Action<T> action)
        {
            if (responseTransferObject.Error == null)
            {
                action.Invoke(responseTransferObject.Data);
            }

            return responseTransferObject;
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
        public static IResponseTransferObject<T2> Fmap<T, T2>(this IResponseTransferObject<T> responseTransferObject, Func<T, T2> mappingFunction)
        {
            if (responseTransferObject.Error != null)
            {
                return CreateGenericServiceResponse<T, T2>(responseTransferObject, responseTransferObject.Error);
            }

            T2 result = mappingFunction.Invoke(responseTransferObject.Data);

            return CreateGenericServiceResponse(responseTransferObject, result);
        }

        /// <summary>
        /// Invokes the specified action whether there is <see cref="IResponseTransferObject{T}.Data" /> or an <see cref="IResponseTransferObject{T}.Error" />.
        /// Returns the current <see cref="IResponseTransferObject{T}" /> instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="action">The action to invoke.</param>
        /// <returns>The current <see cref="IResponseTransferObject{T}" /> instance.</returns>
        public static IResponseTransferObject<T> Run<T>(this IResponseTransferObject<T> responseTransferObject, Action<T> action)
        {
            action.Invoke(responseTransferObject.Data);

            return responseTransferObject;
        }
        
        internal static IResponseTransferObject<T> CreateGenericServiceResponse<T>(IResponseTransferObject<T> originalResponse, T data)
        {
            if (originalResponse is ServiceResponse<T>)
            {
                return new ServiceResponse<T>(data);
            }

            try
            {
                return Activator.CreateInstance(
                    originalResponse.GetType()
                                    .GetGenericTypeDefinition()
                                    .MakeGenericType(typeof(T)),
                    data) as IResponseTransferObject<T>;
            }
            catch (TargetInvocationException)
            {
                // No contructor found that supported IEnumerable<T>! Return default.
                return new ServiceResponse<T>(data);
            }
        }

        internal static IResponseTransferObject<T2> CreateGenericServiceResponse<T, T2>(IResponseTransferObject<T> originalResponse, T2 data)
        {
            if (originalResponse is ServiceResponse<T>)
            {
                return new ServiceResponse<T2>(data);
            }

            try
            {
                return Activator.CreateInstance(
                    originalResponse.GetType()
                                    .GetGenericTypeDefinition()
                                    .MakeGenericType(typeof(T)),
                    data) as IResponseTransferObject<T2>;
            }
            catch (TargetInvocationException)
            {
                // No contructor found that supported IEnumerable<T>! Return default.
                return new ServiceResponse<T2>(data);
            }
        }

        internal static IResponseTransferObject<T2> CreateGenericServiceResponse<T, T2>(IResponseTransferObject<T> originalResponse, Error error)
        {
            if (originalResponse is ServiceResponse<T>)
            {
                return new ServiceResponse<T2>(error);
            }

            try
            {
                return Activator.CreateInstance(
                    originalResponse.GetType()
                                    .GetGenericTypeDefinition()
                                    .MakeGenericType(typeof(T)),
                    error) as IResponseTransferObject<T2>;
            }
            catch (TargetInvocationException)
            {
                // No contructor found that supported IEnumerable<T>! Return default.
                return new ServiceResponse<T2>(error);
            }
        }

        /// <summary>
        /// Returns the error.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <returns>Error.</returns>
        /// <exception cref="System.InvalidOperationException">There is nothing to return from left of either - Error or Data.</exception>
        public static Error FromLeft<T>(this IResponseTransferObject<T> responseTransferObject)
        {
            if (responseTransferObject.Error != null)
            {
                throw new InvalidOperationException("There is nothing to return from left of either - Error or Data.");
            }

            return responseTransferObject.Error;
        }

        /// <summary>
        /// Returns the value of <typeparamref name="T"/> if there is no error.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <returns><typeparamref name="T"/>.</returns>
        /// <exception cref="System.InvalidOperationException">Cannot return right of either when left (errors) exist.</exception>
        public static T FromRight<T>(this IResponseTransferObject<T> responseTransferObject)
        {
            if (responseTransferObject.Error != null)
            {
                throw new InvalidOperationException("Cannot return right of either when left (errors) exist.");
            }

            return responseTransferObject.Data;
        }
    }
}