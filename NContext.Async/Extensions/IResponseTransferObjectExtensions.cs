// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IResponseTransferObjectExtensions.cs">
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
//
// <summary>
//   Defines extension methods for 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using NContext.Dto;

namespace NContext.Extensions
{
    /// <summary>
    /// Defines extension methods for <see cref="IResponseTransferObject{T}"/>.
    /// </summary>
    public static class IResponseTransferObjectExtensions
    {
        #region BindAsync
        
        /// <summary>
        /// Binds the specified <param name="bindingFunction"></param> asynchronously if <see cref="IResponseTransferObject{T}.Data"/> 
        /// exists with no <see cref="IResponseTransferObject{T}.Errors"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IResponseTransferObject{T}"/>.</typeparam>
        /// <typeparam name="T2">The type of <see cref="IResponseTransferObject{T2}"/> to bind to.</typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="bindingFunction">The binding function.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Instance of <see cref="Task{IResponseTransferObject}"/>.</returns>
        /// <remarks></remarks>
        public static IResponseTransferObject<T2> BindAsync<T, T2>(this IResponseTransferObject<T> responseTransferObject, Func<IEnumerable<T>, IResponseTransferObject<T2>> bindingFunction, CancellationToken cancellationToken = default(CancellationToken))
        {
            return responseTransferObject.BindAsyncInternal(bindingFunction, cancellationToken).Result;
        }

        /// <summary>
        /// Binds the specified <param name="bindingFunction"></param> asynchronously if <see cref="IResponseTransferObject{T}.Data"/> 
        /// exists with no <see cref="IResponseTransferObject{T}.Errors"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IResponseTransferObject{T}"/>.</typeparam>
        /// <typeparam name="T2">The type of <see cref="IResponseTransferObject{T2}"/> to bind to.</typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="bindingFunction">The binding function.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Instance of <see cref="Task{IResponseTransferObject}"/>.</returns>
        /// <remarks></remarks>
        public static async Task<IResponseTransferObject<T2>> BindAsyncInternal<T, T2>(
            this IResponseTransferObject<T> responseTransferObject, 
            Func<IEnumerable<T>, IResponseTransferObject<T2>> bindingFunction, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var tcs = new TaskCompletionSource<IResponseTransferObject<T2>>();
            if (responseTransferObject.Errors.Any())
            {
                tcs.SetResult(new ServiceResponse<T2>(responseTransferObject.Errors));
            }
            else if (responseTransferObject.Data.Any())
            {
                await Task.Factory.StartNew(
                    () =>
                        {
                            tcs.SetResult(bindingFunction.Invoke(responseTransferObject.Data));
                        },
                    cancellationToken,
                    TaskCreationOptions.None,
                    TaskScheduler.Default);
            }
            else
            {
                tcs.SetException(new InvalidOperationException("Invalid use of BindAsync(). IResponseTransferObject must contain either data or errors. " +
                                                               "Use Default() to handle situations where both data and errors are empty."));
            }

            return tcs.Task.Result;
        }

        #endregion

        #region CatchAsync

        /// <summary>
        /// Invokes the specified <param name="action"></param> asynchronously if <see cref="IResponseTransferObject{T}.Data"/> 
        /// exists with no <see cref="IResponseTransferObject{T}.Errors"/>.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IResponseTransferObject{T}.Data"/>.</typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="action">The action to invoke.</param>
        /// <returns>Instance of <see cref="Task{IResponseTransferObject}"/> with the current <see cref="IResponseTransferObject{T}"/>.</returns>
        /// <remarks></remarks>
        public static Task<IResponseTransferObject<T>> CatchAsync<T>(this IResponseTransferObject<T> responseTransferObject, Action<IEnumerable<Error>> action)
        {
            return responseTransferObject.CatchAsync(action, CancellationToken.None, TaskCreationOptions.None, null);
        }

        /// <summary>
        /// Invokes the specified <param name="action"></param> asynchronously if <see cref="IResponseTransferObject{T}.Data"/>
        /// exists with no <see cref="IResponseTransferObject{T}.Errors"/>.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IResponseTransferObject{T}.Data"/>.</typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="action">The action to invoke.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Instance of <see cref="Task{IResponseTransferObject}"/> with the current <see cref="IResponseTransferObject{T}"/>.</returns>
        /// <remarks></remarks>
        public static Task<IResponseTransferObject<T>> CatchAsync<T>(this IResponseTransferObject<T> responseTransferObject, Action<IEnumerable<Error>> action, CancellationToken cancellationToken)
        {
            return responseTransferObject.CatchAsync(action, cancellationToken, TaskCreationOptions.None, null);
        }

        /// <summary>
        /// Invokes the specified <param name="action"></param> asynchronously if <see cref="IResponseTransferObject{T}.Data"/>
        /// exists with no <see cref="IResponseTransferObject{T}.Errors"/>.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IResponseTransferObject{T}.Data"/>.</typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="action">The action to invoke.</param>
        /// <param name="taskCreationOptions">The task creation options.</param>
        /// <returns>Instance of <see cref="Task{IResponseTransferObject}"/> with the current <see cref="IResponseTransferObject{T}"/>.</returns>
        /// <remarks></remarks>
        public static Task<IResponseTransferObject<T>> CatchAsync<T>(this IResponseTransferObject<T> responseTransferObject, Action<IEnumerable<Error>> action, TaskCreationOptions taskCreationOptions)
        {
            return responseTransferObject.CatchAsync(action, CancellationToken.None, taskCreationOptions, null);
        }

        /// <summary>
        /// Invokes the specified <param name="action"></param> asynchronously if <see cref="IResponseTransferObject{T}.Data"/>
        /// exists with no <see cref="IResponseTransferObject{T}.Errors"/>.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IResponseTransferObject{T}.Data"/>.</typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="action">The action to invoke.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="taskCreationOptions">The task creation options.</param>
        /// <param name="taskScheduler">The task scheduler.</param>
        /// <returns>Instance of <see cref="Task{IResponseTransferObject}"/> with the current <see cref="IResponseTransferObject{T}"/>.</returns>
        /// <remarks></remarks>
        public static Task<IResponseTransferObject<T>> CatchAsync<T>(this IResponseTransferObject<T> responseTransferObject, Action<IEnumerable<Error>> action, CancellationToken cancellationToken, TaskCreationOptions taskCreationOptions, TaskScheduler taskScheduler)
        {
            var tcs = new TaskCompletionSource<IResponseTransferObject<T>>();
            if (responseTransferObject.Errors.Any())
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        action.Invoke(responseTransferObject.Errors);
                        tcs.SetResult(responseTransferObject);
                    }
                    catch (Exception e)
                    {
                        tcs.SetException(e);
                    }
                }, cancellationToken, taskCreationOptions, taskScheduler ?? TaskScheduler.Default);
            }
            else
            {
                tcs.SetResult(responseTransferObject);
            }

            return tcs.Task;
        }

        #endregion

        #region LetAsync

        /// <summary>
        /// Invokes the specified <param name="action"></param> asynchronously if <see cref="IResponseTransferObject{T}.Data"/>
        /// exists with no <see cref="IResponseTransferObject{T}.Errors"/>.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IResponseTransferObject{T}.Data"/>.</typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="action">The action to invoke.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Instance of <see cref="Task{IResponseTransferObject}"/> with the current <see cref="IResponseTransferObject{T}"/>.</returns>
        /// <remarks></remarks>
        public static Task<IResponseTransferObject<T>> LetAsync<T>(this IResponseTransferObject<T> responseTransferObject, Action<IEnumerable<T>> action, CancellationToken cancellationToken)
        {
            return responseTransferObject.LetAsync(action, cancellationToken, TaskCreationOptions.None, null);
        }

        /// <summary>
        /// Invokes the specified <param name="action"></param> asynchronously if <see cref="IResponseTransferObject{T}.Data"/>
        /// exists with no <see cref="IResponseTransferObject{T}.Errors"/>.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IResponseTransferObject{T}.Data"/>.</typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="action">The action to invoke.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="taskCreationOptions">The task creation options.</param>
        /// <param name="taskScheduler">The task scheduler.</param>
        /// <returns>Instance of <see cref="Task{IResponseTransferObject}"/> with the current <see cref="IResponseTransferObject{T}"/>.</returns>
        /// <remarks></remarks>
        public static Task<IResponseTransferObject<T>> LetAsync<T>(this IResponseTransferObject<T> responseTransferObject, Action<IEnumerable<T>> action, CancellationToken cancellationToken, TaskCreationOptions taskCreationOptions, TaskScheduler taskScheduler)
        {
            var tcs = new TaskCompletionSource<IResponseTransferObject<T>>();
            if (!responseTransferObject.Errors.Any() && responseTransferObject.Data.Any())
            {
                Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            action.Invoke(responseTransferObject.Data);
                            tcs.SetResult(responseTransferObject);
                        }
                        catch (Exception e)
                        {
                            tcs.SetException(e);
                        }
                    }, cancellationToken, taskCreationOptions, taskScheduler ?? TaskScheduler.Default);
            }
            else
            {
                tcs.SetResult(responseTransferObject);
            }

            return tcs.Task;
        }

        #endregion
    }
}