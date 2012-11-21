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
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class IResponseTransferObjectExtensions
    {
        /// <summary>
        /// If <seealso cref="IResponseTransferObject{T}.Errors" /> exist, returns a new <see cref="IResponseTransferObject{T2}" /> instance with the current
        /// <seealso cref="IResponseTransferObject{T}.Errors" />. Else, binds the <seealso cref="IResponseTransferObject{T}.Data" /> into the specified <paramref name="bindingFunction" />.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2">The type of the next <see cref="IResponseTransferObject{T2}" /> to return.</typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="bindingFunction">The binding function.</param>
        /// <returns>Instance of <see cref="IResponseTransferObject{T2}" />.</returns>
        public static IResponseTransferObject<T2> Bind<T, T2>(this IResponseTransferObject<T> responseTransferObject, Func<T, IResponseTransferObject<T2>> bindingFunction)
        {
            if (responseTransferObject.Errors.Any())
            {
                try
                {
                    return Activator.CreateInstance(
                        responseTransferObject.GetType()
                                              .GetGenericTypeDefinition()
                                              .MakeGenericType(typeof(T2)), 
                        responseTransferObject.Errors) as IResponseTransferObject<T2>;
                }
                catch (TargetInvocationException)
                {
                    // No contructor found that supported Error. Return default.
                    return new ServiceResponse<T2>(responseTransferObject.Errors);
                }
            }

            return bindingFunction.Invoke(responseTransferObject.Data);
        }

        /// <summary>
        /// Invokes the specified action if there are any errors. Returns the current instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="action">The action to invoke.</param>
        /// <returns>The current <see cref="IResponseTransferObject{T}" /> instance.</returns>
        public static IResponseTransferObject<T> Catch<T>(this IResponseTransferObject<T> responseTransferObject, Action<IEnumerable<Error>> action)
        {
            if (responseTransferObject.Errors.Any())
            {
                action.Invoke(responseTransferObject.Errors);
            }

            return responseTransferObject;
        }

        /// <summary>
        /// Invokes the specified function if there are any errors - allows you to re-direct control flow with a new <typeparamref name="T" /> value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="continueWithFunction">The continue with function.</param>
        /// <returns>If errors exist, returns the instance of IResponseTransferObject{T} returned by <paramref name="continueWithFunction" />, else returns current instance.</returns>
        public static IResponseTransferObject<T> CatchAndContinue<T>(this IResponseTransferObject<T> responseTransferObject, Func<IEnumerable<Error>, IResponseTransferObject<T>> continueWithFunction)
        {
            if (responseTransferObject.Errors.Any())
            {
                return continueWithFunction.Invoke(responseTransferObject.Errors);
            }

            return responseTransferObject;
        }

        /// <summary>
        /// Invokes the specified function if there are any errors - allows you to re-direct control flow with a new <typeparamref name="T" /> value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2">The type of the t2.</typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="continueWithFunction">The continue with function.</param>
        /// <returns>If errors exist, returns the instance of IResponseTransferObject{T} returned by <paramref name="continueWithFunction" />, else returns current instance.</returns>
        public static IResponseTransferObject<T2> CatchAndFmap<T, T2>(this IResponseTransferObject<T> responseTransferObject, Func<IEnumerable<Error>, T2> continueWithFunction)
        {
            if (responseTransferObject.Errors.Any())
            {
                T2 result = continueWithFunction.Invoke(responseTransferObject.Errors);
                try
                {
                    return Activator.CreateInstance(
                        responseTransferObject.GetType()
                                              .GetGenericTypeDefinition()
                                              .MakeGenericType(typeof(T2)),
                        result) as IResponseTransferObject<T2>;
                }
                catch (TargetInvocationException)
                {
                    // No contructor found that supported IEnumerable<T>! Return default.
                    return new ServiceResponse<T2>(result);
                }
            }

            return new ServiceResponse<T2>(responseTransferObject.Errors);
        }

        /// <summary>
        /// Invokes the specified function if there are any errors - allows you to re-direct control flow with a new <typeparamref name="T" /> value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2">The type of the t2.</typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="continueWithFunction">The continue with function.</param>
        /// <returns>If errors exist, returns the instance of IResponseTransferObject{T} returned by <paramref name="continueWithFunction" />, else returns current instance.</returns>
        public static IResponseTransferObject<T2> CatchAndBind<T, T2>(this IResponseTransferObject<T> responseTransferObject, Func<IEnumerable<Error>, IResponseTransferObject<T2>> continueWithFunction)
        {
            if (responseTransferObject.Errors.Any())
            {
                return continueWithFunction.Invoke(responseTransferObject.Errors);
            }

            return new ServiceResponse<T2>(responseTransferObject.Errors);
        }
        
        /// <summary>
        /// Invokes the specified action if no <see cref="IResponseTransferObject{T}.Errors" /> exist.
        /// Returns the current <see cref="IResponseTransferObject{T}" /> instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="action">The action to invoke.</param>
        /// <returns>The current <see cref="IResponseTransferObject{T}" /> instance.</returns>
        public static IResponseTransferObject<T> Let<T>(this IResponseTransferObject<T> responseTransferObject, Action<T> action)
        {
            if (!responseTransferObject.Errors.Any())
            {
                action.Invoke(responseTransferObject.Data);
            }

            return responseTransferObject;
        }

        public static IResponseTransferObject<T2> Fmap<T, T2>(this IResponseTransferObject<T> responseTransferObject, Func<T, T2> mappingFunction)
        {
            if (responseTransferObject.Errors.Any())
            {
                try
                {
                    return
                        Activator.CreateInstance(
                            responseTransferObject.GetType()
                                                  .GetGenericTypeDefinition()
                                                  .MakeGenericType(typeof(T2)),
                            responseTransferObject.Errors) as IResponseTransferObject<T2>;
                }
                catch (TargetInvocationException)
                {
                    // No contructor found that supported Errors! Return default.
                    return new ServiceResponse<T2>(responseTransferObject.Errors);
                }
            }

            T2 result = mappingFunction.Invoke(responseTransferObject.Data);
            try
            {
                return Activator.CreateInstance(
                    responseTransferObject.GetType()
                                          .GetGenericTypeDefinition()
                                          .MakeGenericType(typeof(T2)),
                    result) as IResponseTransferObject<T2>;
            }
            catch (TargetInvocationException)
            {
                // No contructor found that supported IEnumerable<T>! Return default.
                return new ServiceResponse<T2>(result);
            }
        }

        /// <summary>
        /// Invokes the specified action whether there are <see cref="IResponseTransferObject{T}.Data" /> or <see cref="IResponseTransferObject{T}.Errors" />.
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
    }
}