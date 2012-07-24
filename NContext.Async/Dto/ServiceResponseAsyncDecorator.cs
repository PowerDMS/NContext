﻿// --------------------------------------------------------------------------------------------------------------------
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

namespace NContext.Dto
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    /// <summary>
    /// Defines asynchronous support for ServiceResponse.
    /// </summary>
    [DataContract(Name = "ServiceResponseOf{0}")]
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
        /// <param name="cancellationTokenSource">The cancellation token source.</param>
        /// <remarks></remarks>
        protected ServiceResponseAsyncDecorator(IResponseTransferObject<T> responseTransferObject, Task task, CancellationTokenSource cancellationTokenSource)
        {
            _ResponseTransferObject = responseTransferObject;
            _Task = task;
            _CancellationTokenSource = cancellationTokenSource;
        }

        #endregion

        #region Facade of IResponseTransferObject<T>

        /// <summary>
        /// Gets the <typeparam name="T"/> data.
        /// </summary>
        [DataMember(Order = 1)]
        public IEnumerable<T> Data
        {
            get
            {
                return _ResponseTransferObject;
            }
        }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <remarks></remarks>
        [DataMember(Order = 2)]
        public IEnumerable<Error> Errors
        {
            get
            {
                return _ResponseTransferObject.Errors;
            }
        }

        /// <summary>
        /// If <seealso cref="IResponseTransferObject{T}"/> exist, returns a new <see cref="IResponseTransferObject{T}"/> instance with the current 
        /// <seealso cref="IResponseTransferObject{T}"/>. Else, binds the <seealso cref="IResponseTransferObject{T}"/> into the specified <paramref name="bindingFunction"/>.
        /// </summary>
        /// <typeparam name="T2">The type of the next <see cref="IResponseTransferObject{T}"/> to return.</typeparam>
        /// <param name="bindingFunction">The binding function.</param>
        /// <returns>Instance of <see cref="Errors"/>.</returns>
        /// <exception cref="Data">Thrown when no errors or data exist.</exception>
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

        public IResponseTransferObject<T2> Fmap<T2>(Func<IEnumerable<T>, IEnumerable<T2>> mappingFunction)
        {
            return _ResponseTransferObject.Fmap(mappingFunction);
        }

        /// <summary>
        /// Invokes the specified action if data exists with no <see cref="IResponseTransferObject{T}.Errors"/> present.
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

        #region Implementation of IResponseTransferObjectAsync<T>

        /// <summary>
        /// If <seealso cref="IResponseTransferObject{T}.Errors"/> exist, returns a new <see cref="IResponseTransferObject{T2}"/> instance with the current 
        /// <seealso cref="IResponseTransferObject{T}.Errors"/>. Else, binds the data into the specified <paramref name="bindingFunction"/>.
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

            return bindingFunction.Invoke(Data, _Task, _CancellationTokenSource);
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
            if (!Errors.Any())
            {
                action.Invoke(Data, _Task, _CancellationTokenSource);
            }

            return this;
        }

        #endregion

        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<T> GetEnumerator()
        {
            return Data.GetEnumerator();
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

    public class ServiceResponseParallelDecorator<T> : IResponseTransferObjectParallel<T>
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
        public ServiceResponseParallelDecorator(IResponseTransferObject<T> responseTransferObject)
            : this(responseTransferObject, null, null)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponseAsyncDecorator&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="task">The task.</param>
        /// <param name="cancellationTokenSource">The cancellation token source.</param>
        /// <remarks></remarks>
        protected internal ServiceResponseParallelDecorator(IResponseTransferObject<T> responseTransferObject, Task task, CancellationTokenSource cancellationTokenSource)
        {
            _ResponseTransferObject = responseTransferObject;
            _Task = task;
            _CancellationTokenSource = cancellationTokenSource;
        }

        #endregion

        #region Properties

        public Task Task
        {
            get
            {
                return _Task;
            }
        }

        public CancellationTokenSource CancellationTokenSource
        {
            get
            {
                return _CancellationTokenSource;
            }
        }

        #endregion

        #region Facade of IResponseTransferObject<T>

        /// <summary>
        /// Gets the <typeparam name="T"/> data.
        /// </summary>
        [DataMember(Order = 1)]
        public IEnumerable<T> Data
        {
            get
            {
                return _ResponseTransferObject.AsParallel();
            }
        }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <remarks></remarks>
        [DataMember(Order = 2)]
        public IEnumerable<Error> Errors
        {
            get
            {
                return _ResponseTransferObject.Errors;
            }
        }

        /// <summary>
        /// If <seealso cref="IResponseTransferObject{T}"/> exist, returns a new <see cref="IResponseTransferObject{T}"/> instance with the current 
        /// <seealso cref="IResponseTransferObject{T}"/>. Else, binds the <seealso cref="IResponseTransferObject{T}"/> into the specified <paramref name="bindingFunction"/>.
        /// </summary>
        /// <typeparam name="T2">The type of the next <see cref="IResponseTransferObject{T}"/> to return.</typeparam>
        /// <param name="bindingFunction">The binding function.</param>
        /// <returns>Instance of <see cref="Errors"/>.</returns>
        /// <exception cref="Data">Thrown when no errors or data exist.</exception>
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

        public IResponseTransferObject<T2> Fmap<T2>(Func<IEnumerable<T>, IEnumerable<T2>> mappingFunction)
        {
            return _ResponseTransferObject.Fmap(mappingFunction);
        }

        /// <summary>
        /// Invokes the specified action if data exists with no <see cref="IResponseTransferObject{T}.Errors"/> present.
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

        #region Implementation of IResponseTransferObjectParallel<T>

        /// <summary>
        /// If <seealso cref="IResponseTransferObject{T}.Errors"/> exist, returns a new <see cref="IResponseTransferObject{T2}"/> instance with the current 
        /// <seealso cref="IResponseTransferObject{T}.Errors"/>. Else, binds the data into the specified <paramref name="bindingFunction"/>.
        /// </summary>
        /// <typeparam name="T2">The type of the next <see cref="IResponseTransferObject{T2}"/> to return.</typeparam>
        /// <param name="bindingFunction">The binding function.</param>
        /// <returns>Instance of <see cref="IResponseTransferObject{T2}"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no errors or data exist.</exception>
        /// <remarks></remarks>
        public IResponseTransferObjectParallel<T2> Bind<T2>(Func<IEnumerable<T>, Task, CancellationTokenSource, IResponseTransferObject<T2>> bindingFunction)
        {
            if (Errors.Any())
            {
                return new ServiceResponseParallelDecorator<T2>(new ServiceResponse<T2>(Errors));
            }

            return new ServiceResponseParallelDecorator<T2>(bindingFunction.Invoke(Data, _Task, _CancellationTokenSource));
        }

        /// <summary>
        /// Invokes the specified action if <see cref="IResponseTransferObject{T}.Data"/> exists with no <see cref="IResponseTransferObject{T}.Errors"/> present.
        /// Returns the current <see cref="IResponseTransferObject{T}"/> instance.
        /// </summary>
        /// <param name="action">The action to invoke with the data and continuation task.</param>
        /// <returns>The current <see cref="IResponseTransferObject{T}"/> instance.</returns>
        /// <remarks></remarks>
        public IResponseTransferObjectParallel<T> Let(Action<IEnumerable<T>, Task, CancellationTokenSource> action)
        {
            if (!Errors.Any())
            {
                action.Invoke(Data, _Task, _CancellationTokenSource);
            }

            return this;
        }

        /// <summary>
        /// Invokes the specified <paramref name="actions"/> in parallel if <see cref="IResponseTransferObject{T}.Errors"/>
        /// exist with no <see cref="IResponseTransferObject{T}.Data"/>.
        /// </summary>
        /// <param name="degreeOfParallelism">The degree of parallelism.</param>
        /// <param name="actions">The actions to invoke.</param>
        /// <returns>Instance of <see cref="IResponseTransferObjectAsync{T}"/>.</returns>
        /// <remarks></remarks>
        public IResponseTransferObjectParallel<T> CatchParallel(Int32 degreeOfParallelism, params Action<IEnumerable<Error>>[] actions)
        {
            return CatchParallel(degreeOfParallelism, CancellationToken.None, ParallelExecutionMode.Default, actions);
        }

        /// <summary>
        /// Invokes the specified <paramref name="actions"/> in parallel if <see cref="IResponseTransferObject{T}.Errors"/>
        /// exist with no <see cref="IResponseTransferObject{T}.Data"/>.
        /// </summary>
        /// <param name="degreeOfParallelism">The degree of parallelism.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="actions">The actions to invoke.</param>
        /// <returns>Instance of <see cref="IResponseTransferObjectAsync{T}"/>.</returns>
        /// <remarks></remarks>
        public IResponseTransferObjectParallel<T> CatchParallel(Int32 degreeOfParallelism, CancellationToken cancellationToken, params Action<IEnumerable<Error>>[] actions)
        {
            return CatchParallel(degreeOfParallelism, cancellationToken, ParallelExecutionMode.Default, actions);
        }

        /// <summary>
        /// Invokes the specified <paramref name="actions"/> in parallel if <see cref="IResponseTransferObject{T}.Errors"/>
        /// exist with no <see cref="IResponseTransferObject{T}.Data"/>.
        /// </summary>
        /// <param name="degreeOfParallelism">The degree of parallelism.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="parallelExecutionMode">The parallel execution mode.</param>
        /// <param name="actions">The actions to invoke.</param>
        /// <returns>Instance of <see cref="IResponseTransferObjectAsync{T}"/>.</returns>
        /// <remarks></remarks>
        public IResponseTransferObjectParallel<T> CatchParallel(Int32 degreeOfParallelism, CancellationToken cancellationToken, ParallelExecutionMode parallelExecutionMode, params Action<IEnumerable<Error>>[] actions)
        {
            if (Errors.Any())
            {
                var cancellationTokenSource = new CancellationTokenSource();
                if (cancellationToken != CancellationToken.None)
                {
                    cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationTokenSource.Token, cancellationToken);
                }

                var task = Task.Factory.StartNew(
                    () =>
                    {
                        if (degreeOfParallelism == default(Int32) || degreeOfParallelism == Int32.MaxValue)
                        {
                            degreeOfParallelism = 1;
                        }

                        actions.AsParallel()
                               .WithCancellation(cancellationTokenSource.Token)
                               .WithDegreeOfParallelism(degreeOfParallelism)
                               .WithExecutionMode(parallelExecutionMode)
                               .ForAll(action => action.Invoke(Errors));
                    },
                    cancellationTokenSource.Token);

                return new ServiceResponseParallelDecorator<T>(_ResponseTransferObject, task, cancellationTokenSource);
            }

            return this;
        }

        public IResponseTransferObjectParallel<T> Let(params Action<IEnumerable<T>>[] actions)
        {
            return Let(Environment.ProcessorCount, actions);
        }

        /// <summary>
        /// Invokes the specified <paramref name="actions"/> in parallel if <see cref="IResponseTransferObject{T}.Data"/>
        /// exists with no <see cref="IResponseTransferObject{T}.Errors"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="actions">The actions to invoke.</param>
        /// <returns>Instance of <see cref="IResponseTransferObjectAsync{T}"/>.</returns>
        /// <remarks></remarks>
        public IResponseTransferObjectParallel<T> Let(CancellationToken cancellationToken, params Action<IEnumerable<T>>[] actions)
        {
            return Let(Environment.ProcessorCount, cancellationToken, actions);
        }

        /// <summary>
        /// Invokes the specified <paramref name="actions"/> in parallel if <see cref="IResponseTransferObject{T}.Data"/>
        /// exists with no <see cref="IResponseTransferObject{T}.Errors"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="parallelExecutionMode">The parallel execution mode.</param>
        /// <param name="actions">The actions to invoke.</param>
        /// <returns>Instance of <see cref="IResponseTransferObjectAsync{T}"/>.</returns>
        /// <remarks></remarks>
        public IResponseTransferObjectParallel<T> Let(CancellationToken cancellationToken, ParallelExecutionMode parallelExecutionMode, params Action<IEnumerable<T>>[] actions)
        {
            return Let(Environment.ProcessorCount, cancellationToken, parallelExecutionMode, actions);
        }

        /// <summary>
        /// Invokes the specified <paramref name="actions"/> in parallel if <see cref="IResponseTransferObject{T}.Data"/>
        /// exists with no <see cref="IResponseTransferObject{T}.Errors"/>.
        /// </summary>
        /// <param name="degreeOfParallelism">The degree of parallelism.</param>
        /// <param name="actions">The actions to invoke.</param>
        /// <returns>Instance of <see cref="IResponseTransferObjectAsync{T}"/>.</returns>
        /// <remarks></remarks>
        public IResponseTransferObjectParallel<T> Let(Int32 degreeOfParallelism, params Action<IEnumerable<T>>[] actions)
        {
            return Let(degreeOfParallelism, CancellationToken.None, ParallelExecutionMode.Default, actions);
        }

        /// <summary>
        /// Invokes the specified <paramref name="actions"/> in parallel if <see cref="IResponseTransferObject{T}.Data"/>
        /// exists with no <see cref="IResponseTransferObject{T}.Errors"/>.
        /// </summary>
        /// <param name="degreeOfParallelism">The degree of parallelism.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="actions">The actions to invoke.</param>
        /// <returns>Instance of <see cref="IResponseTransferObjectAsync{T}"/>.</returns>
        /// <remarks></remarks>
        public IResponseTransferObjectParallel<T> Let(Int32 degreeOfParallelism, CancellationToken cancellationToken, params Action<IEnumerable<T>>[] actions)
        {
            return Let(degreeOfParallelism, cancellationToken, ParallelExecutionMode.Default, actions);
        }

        /// <summary>
        /// Invokes the specified <paramref name="actions"/> in parallel if <see cref="IResponseTransferObject{T}.Data"/>
        /// exists with no <see cref="IResponseTransferObject{T}.Errors"/>.
        /// </summary>
        /// <param name="degreeOfParallelism">The degree of parallelism.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="parallelExecutionMode">The parallel execution mode.</param>
        /// <param name="actions">The actions to invoke.</param>
        /// <returns>Instance of <see cref="IResponseTransferObjectAsync{T}"/>.</returns>
        /// <remarks></remarks>
        public IResponseTransferObjectParallel<T> Let(Int32 degreeOfParallelism, CancellationToken cancellationToken, ParallelExecutionMode parallelExecutionMode, params Action<IEnumerable<T>>[] actions)
        {
            if (!Errors.Any())
            {
                var cancellationTokenSource = new CancellationTokenSource();
                if (cancellationToken != CancellationToken.None)
                {
                    cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationTokenSource.Token, cancellationToken);
                }

                var task = Task.Factory.StartNew(
                    () =>
                    {
                        if (degreeOfParallelism == default(Int32) || degreeOfParallelism == Int32.MaxValue)
                        {
                            degreeOfParallelism = 1;
                        }

                        actions.AsParallel()
                               .WithCancellation(cancellationTokenSource.Token)
                               .WithDegreeOfParallelism(degreeOfParallelism)
                               .WithExecutionMode(parallelExecutionMode)
                               .ForAll(a => a.Invoke(Data));
                    },
                    cancellationTokenSource.Token);

                return new ServiceResponseParallelDecorator<T>(_ResponseTransferObject, task, cancellationTokenSource);
            }

            return this;
        }

        public IResponseTransferObjectParallel<T> Post<TTargetBlock>(TTargetBlock targetBlock) where TTargetBlock : ITargetBlock<T>
        {
            if (targetBlock == null)
            {
                throw new ArgumentNullException("targetBlock");
            }

            if (!Errors.Any() && Data.Any())
            {
                var cancellationTokenSource = new CancellationTokenSource();
                Task.Factory.StartNew(
                    () =>
                    {
                        foreach (var value in Data)
                        {
                            targetBlock.Post(value);
                        }

                        targetBlock.Complete();
                    },
                    cancellationTokenSource.Token);

                return new ServiceResponseParallelDecorator<T>(_ResponseTransferObject, targetBlock.Completion, cancellationTokenSource);
            }

            return this;
        }

        #endregion

        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<T> GetEnumerator()
        {
            return Data.GetEnumerator();
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