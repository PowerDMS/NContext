// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceResponseAsyncDecorator.cs">
//   Copyright (c) 2012
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
//   Defines asynchronous support for ServiceResponse.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

using NContext.Dto;
using NContext.Extensions;

namespace NContext
{
    /// <summary>
    /// Defines asynchronous support for ServiceResponse.
    /// </summary>
    public class ServiceResponseAsyncDecorator<T> : IResponseTransferObjectAsync<T>
    {
        #region Fields

        private readonly IResponseTransferObject<T> _ResponseTransferObject;

        private readonly Task _Task;

        private readonly CancellationTokenSource _CancellationTokenSource;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponseAsyncDecorator&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <remarks></remarks>
        public ServiceResponseAsyncDecorator(IResponseTransferObject<T> responseTransferObject)
            : this(responseTransferObject, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponseAsyncDecorator&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="task">The task.</param>
        /// <remarks></remarks>
        protected ServiceResponseAsyncDecorator(IResponseTransferObject<T> responseTransferObject, Task task)
            : this(responseTransferObject, task, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponseAsyncDecorator&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="task">The task.</param>
        /// <param name="cancellationTokenSource">The cancellation token source.</param>
        /// <remarks></remarks>
        protected ServiceResponseAsyncDecorator(IResponseTransferObject<T> responseTransferObject, Task task, CancellationTokenSource cancellationTokenSource)
        {
            _ResponseTransferObject = responseTransferObject;
            _Task = task;
            _CancellationTokenSource = cancellationTokenSource;
        }

        #endregion
        
        #region Implementation of IResponseTransferObjectAsync<T>

        /// <summary>
        /// Returns a new <see cref="IResponseTransferObject{T2}"/> instance with the current <see cref="IResponseTransferObject{T}.Errors"/> passed if <see cref="IResponseTransferObject{T}.Errors"/> exist.
        /// Binds the <see cref="IResponseTransferObject{T}.Data"/> into the specified <paramref name="bindingFunction" /> if <see cref="IResponseTransferObject{T}.Data"/> exists - returning a <see cref="IResponseTransferObject{T2}"/>.
        /// </summary>
        /// <typeparam name="T2">The type of the next <see cref="IResponseTransferObject{T2}"/> to return.</typeparam>
        /// <param name="bindingFunction">The binding function.</param>
        /// <returns>Instance of <see cref="IResponseTransferObject{T2}"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no errors or data exist.</exception>
        /// <remarks></remarks>
        public IResponseTransferObject<T2> Bind<T2>(Func<IEnumerable<T>, Task, CancellationTokenSource, IResponseTransferObject<T2>> bindingFunction)
        {
            if (Errors.Any())
            {
                return new ServiceResponse<T2>(Errors);
            }

            if (Data.Any())
            {
                return bindingFunction.Invoke(Data, _Task, _CancellationTokenSource);
            }

            throw new InvalidOperationException("Invalid use of Bind(). ServiceResponse must contain either data or errors. " +
                                                "Use Default() to handle situations where both data and errors are empty.");
        }

        /// <summary>
        /// Invokes the specified <paramref name="actions"/> in parallel if <see cref="IResponseTransferObject{T}.Errors"/>
        /// exist with no <see cref="IResponseTransferObject{T}.Data"/>.
        /// </summary>
        /// <param name="maxDegreeOfParallelism">The max degree of parallelism.</param>
        /// <param name="actions">The actions to invoke.</param>
        /// <returns>Instance of <see cref="IResponseTransferObjectAsync{T}"/>.</returns>
        /// <remarks></remarks>
        public IResponseTransferObjectAsync<T> CatchParallel(Int32 maxDegreeOfParallelism = 1, params Action<IEnumerable<Error>>[] actions)
        {
            if (Errors.Any())
            {
                var cancellationTokenSource = new CancellationTokenSource();
                var task = Task.Factory.StartNew(
                    () =>
                        {
                            if (maxDegreeOfParallelism == default(Int32) || maxDegreeOfParallelism == Int32.MaxValue)
                            {
                                maxDegreeOfParallelism = 1;
                            }

                            actions.Invoke(
                                Errors,
                                new ParallelOptions
                                    {
                                        CancellationToken = cancellationTokenSource.Token,
                                        MaxDegreeOfParallelism = maxDegreeOfParallelism
                                    });
                        },
                    cancellationTokenSource.Token,
                    TaskCreationOptions.None,
                    TaskScheduler.Default);

                return new ServiceResponseAsyncDecorator<T>(_ResponseTransferObject, task, cancellationTokenSource);
            }

            return this;
        }

        /// <summary>
        /// Invokes the specified action if <see cref="IResponseTransferObject{T}.Data"/> exists with no <see cref="IResponseTransferObject{T}.Errors"/> present.
        /// Returns the current <see cref="IResponseTransferObject{T}"/> instance.
        /// </summary>
        /// <param name="action">The action to invoke with the data and continuation task.</param>
        /// <returns>The current <see cref="IResponseTransferObject{T}"/> instance.</returns>
        /// <remarks></remarks>
        public IResponseTransferObject<T> Let(Action<IEnumerable<T>, Task, CancellationTokenSource> action)
        {
            if (!Errors.Any() && Data.Any())
            {
                action.Invoke(Data, _Task, _CancellationTokenSource);
            }

            return this;
        }

        /// <summary>
        /// Invokes the specified <paramref name="actions"/> in parallel if <see cref="IResponseTransferObject{T}.Data"/>
        /// exists with no <see cref="IResponseTransferObject{T}.Errors"/>.
        /// </summary>
        /// <param name="maxDegreeOfParallelism">The max degree of parallelism.</param>
        /// <param name="actions">The actions to invoke.</param>
        /// <returns>Instance of <see cref="IResponseTransferObjectAsync{T}"/>.</returns>
        /// <remarks></remarks>
        public IResponseTransferObjectAsync<T> LetParallel(Int32 maxDegreeOfParallelism = 1, params Action<IEnumerable<T>>[] actions)
        {
            if (!Errors.Any() && Data.Any())
            {
                var cancellationTokenSource = new CancellationTokenSource();
                var task = Task.Factory.StartNew(
                    () =>
                        {
                            if (maxDegreeOfParallelism == default(Int32) || maxDegreeOfParallelism == Int32.MaxValue)
                            {
                                maxDegreeOfParallelism = 1;
                            }

                            actions.Invoke(
                                Data,
                                new ParallelOptions
                                    {
                                        CancellationToken = cancellationTokenSource.Token,
                                        MaxDegreeOfParallelism = maxDegreeOfParallelism
                                    });
                        },
                    cancellationTokenSource.Token,
                    TaskCreationOptions.None,
                    TaskScheduler.Default);

                return new ServiceResponseAsyncDecorator<T>(_ResponseTransferObject, task, cancellationTokenSource);
            }

            return this;
        }

        public IResponseTransferObject<T> Post<TTargetBlock>(TTargetBlock targetBlock, ParallelOptions parallelOptions = null) where TTargetBlock : ITargetBlock<T>
        {
            if (targetBlock == null)
            {
                throw new ArgumentNullException("targetBlock");
            }

            if (!Errors.Any() && Data.Any())
            {
                Parallel.ForEach(Data, parallelOptions ?? new ParallelOptions(), data => targetBlock.Post(data));
            }

            return _ResponseTransferObject;
        }

        #endregion

        #region Facade of IResponseTransferObject<T>

        /// <summary>
        /// Gets the <typeparam name="T"/> data.
        /// </summary>
        public IEnumerable<T> Data
        {
            get
            {
                return _ResponseTransferObject.Data;
            }
        }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <remarks></remarks>
        public IEnumerable<Error> Errors
        {
            get
            {
                return _ResponseTransferObject.Errors;
            }
        }

        /// <summary>
        /// Returns a new <see cref="IResponseTransferObject{T2}"/> instance with the current <see cref="IResponseTransferObject{T}.Errors"/> passed if <see cref="IResponseTransferObject{T}.Errors"/> exist.
        /// Binds the <see cref="IResponseTransferObject{T}.Data"/> into the specified <param name="bindingFunction"></param> if <see cref="IResponseTransferObject{T}.Data"/> exists - returning a <see cref="IResponseTransferObject{T2}"/>.
        /// </summary>
        /// <typeparam name="T2">The type of the next <see cref="IResponseTransferObject{T2}"/> to return.</typeparam>
        /// <param name="bindingFunction">The binding function.</param>
        /// <returns>Instance of <see cref="IResponseTransferObject{T2}"/>.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when no errors or data exist.</exception>
        /// <remarks></remarks>
        public IResponseTransferObject<T2> Bind<T2>(Func<IEnumerable<T>, IResponseTransferObject<T2>> bindingFunction)
        {
            return _ResponseTransferObject.Bind(bindingFunction);
        }

        /// <summary>
        /// Invokes the specified action if there are any errors. Returns the current instance.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        /// <returns>The current <see cref="IResponseTransferObject{T}"/> instance.</returns>
        /// <remarks></remarks>
        public IResponseTransferObject<T> Catch(Action<IEnumerable<Error>> action)
        {
            return _ResponseTransferObject.Catch(action);
        }

        /// <summary>
        /// Invokes the specified function if there are any errors - allows you to re-direct control flow with a new <typeparamref name="T"/> value.
        /// </summary>
        /// <param name="continueWithFunction">The continue with function.</param>
        /// <returns>If errors exist, returns the instance of IResponseTransferObject&lt;T&gt; returned by <paramref name="continueWithFunction"/>, else returns current instance.</returns>
        /// <remarks></remarks>
        public IResponseTransferObject<T> CatchAndContinue(Func<IEnumerable<Error>, IResponseTransferObject<T>> continueWithFunction)
        {
            return _ResponseTransferObject.CatchAndContinue(continueWithFunction);
        }

        /// <summary>
        /// Invokes the specified <param name="defaultFunction"></param> function if both <see cref="IResponseTransferObject{T}.Errors"/> and <see cref="IResponseTransferObject{T}.Data"/> are empty.
        /// </summary>
        /// <typeparam name="T2">The type of the next <see cref="IResponseTransferObject{T2}"/> to return.</typeparam>
        /// <param name="defaultFunction">The default response.</param>
        /// <returns>Instance of <see cref="IResponseTransferObject{T2}"/>.</returns>
        /// <remarks></remarks>
        public IResponseTransferObject<T2> Default<T2>(Func<IResponseTransferObject<T2>> defaultFunction)
        {
            return _ResponseTransferObject.Default(defaultFunction);
        }

        /// <summary>
        /// Invokes the specified action if <see cref="IResponseTransferObject{T}.Data"/> exists with no <see cref="IResponseTransferObject{T}.Errors"/> present.
        /// Returns the current <see cref="IResponseTransferObject{T}"/> instance.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        /// <returns>The current <see cref="IResponseTransferObject{T}"/> instance.</returns>
        /// <remarks></remarks>
        public IResponseTransferObject<T> Let(Action<IEnumerable<T>> action)
        {
            return _ResponseTransferObject.Let(action);
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            _ResponseTransferObject.Dispose();
        }

        #endregion
    }
}