// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IResponseTransferObjectExtensions.cs">
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
//   Defines extension methods for 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    using NContext.Dto;

    public static class IResponseTransferObjectExtensions
    {
        public static IResponseTransferObjectAsync<T> AsAsync<T>(this IResponseTransferObject<T> responseTransferObject)
        {
            return new ServiceResponseAsyncDecorator<T>(responseTransferObject);
        }

        public static IResponseTransferObjectParallel<T> AsConcurrent<T>(this IResponseTransferObject<T> responseTransferObject)
        {
            return new ServiceResponseParallelDecorator<T>(responseTransferObject);
        }

        public static IResponseTransferObject<T> AsSequential<T>(this IResponseTransferObject<T> responseTransferObject)
        {
            return responseTransferObject;
        }
    }

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <remarks></remarks>
    //public static class IResponseTransferObjectParallelExtensions
    //{
    //    /// <summary>
    //    /// If <seealso cref="IResponseTransferObject{T}.Errors"/> exist, returns a new <see cref="IResponseTransferObject{T2}"/> instance with the current
    //    /// <seealso cref="IResponseTransferObject{T}.Errors"/>. Else, binds the data into the specified <paramref name="bindingFunction"/>.
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <typeparam name="T2">The type of the next <see cref="IResponseTransferObject{T2}"/> to return.</typeparam>
    //    /// <param name="responseTransferObject">The response transfer object.</param>
    //    /// <param name="bindingFunction">The binding function.</param>
    //    /// <returns>Instance of <see cref="IResponseTransferObject{T2}"/>.</returns>
    //    /// <exception cref="InvalidOperationException">Thrown when no errors or data exist.</exception>
    //    /// <remarks></remarks>
    //    public static IResponseTransferObjectParallel<T2> Bind<T, T2>(this IResponseTransferObjectParallel<T> responseTransferObject, Func<IEnumerable<T>, Task, CancellationTokenSource, IResponseTransferObject<T2>> bindingFunction)
    //    {
    //        if (responseTransferObject.Errors.Any())
    //        {
    //            return new ServiceResponseParallelDecorator<T2>(new ServiceResponse<T2>(responseTransferObject.Errors));
    //        }

    //        return new ServiceResponseParallelDecorator<T2>(bindingFunction.Invoke(responseTransferObject.Data, responseTransferObject.Task, responseTransferObject.CancellationTokenSource));
    //    }

    //    /// <summary>
    //    /// Invokes the specified action if <see cref="IResponseTransferObject{T}.Data"/> exists with no <see cref="IResponseTransferObject{T}.Errors"/> present.
    //    /// Returns the current <see cref="IResponseTransferObject{T}"/> instance.
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="responseTransferObject">The response transfer object.</param>
    //    /// <param name="action">The action to invoke with the data and continuation task.</param>
    //    /// <returns>The current <see cref="IResponseTransferObject{T}"/> instance.</returns>
    //    /// <remarks></remarks>
    //    public static IResponseTransferObjectParallel<T> Let<T>(this IResponseTransferObjectParallel<T> responseTransferObject, Action<IEnumerable<T>, Task, CancellationTokenSource> action)
    //    {
    //        if (!responseTransferObject.Errors.Any())
    //        {
    //            action.Invoke(responseTransferObject.Data, responseTransferObject.Task, responseTransferObject.CancellationTokenSource);
    //        }

    //        return responseTransferObject;
    //    }

    //    /// <summary>
    //    /// Invokes the specified <paramref name="actions"/> in parallel if <see cref="IResponseTransferObject{T}.Errors"/>
    //    /// exist with no <see cref="IResponseTransferObject{T}.Data"/>.
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="responseTransferObject">The response transfer object.</param>
    //    /// <param name="degreeOfParallelism">The degree of parallelism.</param>
    //    /// <param name="actions">The actions to invoke.</param>
    //    /// <returns>Instance of <see cref="IResponseTransferObjectAsync{T}"/>.</returns>
    //    /// <remarks></remarks>
    //    public static IResponseTransferObjectParallel<T> Catch<T>(this IResponseTransferObjectParallel<T> responseTransferObject, Int32 degreeOfParallelism, params Action<IEnumerable<Error>>[] actions)
    //    {
    //        return Catch(responseTransferObject, degreeOfParallelism, CancellationToken.None, ParallelExecutionMode.Default, actions);
    //    }

    //    /// <summary>
    //    /// Invokes the specified <paramref name="actions"/> in parallel if <see cref="IResponseTransferObject{T}.Errors"/>
    //    /// exist with no <see cref="IResponseTransferObject{T}.Data"/>.
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="responseTransferObject">The response transfer object.</param>
    //    /// <param name="degreeOfParallelism">The degree of parallelism.</param>
    //    /// <param name="cancellationToken">The cancellation token.</param>
    //    /// <param name="actions">The actions to invoke.</param>
    //    /// <returns>Instance of <see cref="IResponseTransferObjectAsync{T}"/>.</returns>
    //    /// <remarks></remarks>
    //    public static IResponseTransferObjectParallel<T> Catch<T>(this IResponseTransferObjectParallel<T> responseTransferObject, Int32 degreeOfParallelism, CancellationToken cancellationToken, params Action<IEnumerable<Error>>[] actions)
    //    {
    //        return Catch(responseTransferObject, degreeOfParallelism, cancellationToken, ParallelExecutionMode.Default, actions);
    //    }

    //    /// <summary>
    //    /// Invokes the specified <paramref name="actions"/> in parallel if <see cref="IResponseTransferObject{T}.Errors"/>
    //    /// exist with no <see cref="IResponseTransferObject{T}.Data"/>.
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="responseTransferObject">The response transfer object.</param>
    //    /// <param name="degreeOfParallelism">The degree of parallelism.</param>
    //    /// <param name="cancellationToken">The cancellation token.</param>
    //    /// <param name="parallelExecutionMode">The parallel execution mode.</param>
    //    /// <param name="actions">The actions to invoke.</param>
    //    /// <returns>Instance of <see cref="IResponseTransferObjectAsync{T}"/>.</returns>
    //    /// <remarks></remarks>
    //    public static IResponseTransferObjectParallel<T> Catch<T>(this IResponseTransferObjectParallel<T> responseTransferObject, Int32 degreeOfParallelism, CancellationToken cancellationToken, ParallelExecutionMode parallelExecutionMode, params Action<IEnumerable<Error>>[] actions)
    //    {
    //        if (responseTransferObject.Errors.Any())
    //        {
    //            var cancellationTokenSource = new CancellationTokenSource();
    //            if (cancellationToken != CancellationToken.None)
    //            {
    //                cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationTokenSource.Token, cancellationToken);
    //            }

    //            var task = Task.Factory.StartNew(
    //                () =>
    //                {
    //                    if (degreeOfParallelism == default(Int32) || degreeOfParallelism == Int32.MaxValue)
    //                    {
    //                        degreeOfParallelism = 1;
    //                    }

    //                    actions.AsParallel()
    //                           .WithCancellation(cancellationTokenSource.Token)
    //                           .WithDegreeOfParallelism(degreeOfParallelism)
    //                           .WithExecutionMode(parallelExecutionMode)
    //                           .ForAll(action => action.Invoke(responseTransferObject.Errors));
    //                },
    //                cancellationTokenSource.Token);

    //            return new ServiceResponseParallelDecorator<T>(responseTransferObject, task, cancellationTokenSource);
    //        }

    //        return responseTransferObject;
    //    }

    //    /// <summary>
    //    /// Invokes the specified <paramref name="actions"/> in parallel if <see cref="IResponseTransferObject{T}.Data"/>
    //    /// exists with no <see cref="IResponseTransferObject{T}.Errors"/>.
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="responseTransferObject">The response transfer object.</param>
    //    /// <param name="actions">The actions to invoke.</param>
    //    /// <returns>Instance of <see cref="IResponseTransferObjectAsync{T}"/>.</returns>
    //    /// <remarks></remarks>
    //    public static IResponseTransferObjectParallel<T> Let<T>(this IResponseTransferObjectParallel<T> responseTransferObject, params Action<IEnumerable<T>>[] actions)
    //    {
    //        return Let(responseTransferObject, Environment.ProcessorCount, actions);
    //    }

    //    /// <summary>
    //    /// Invokes the specified <paramref name="actions"/> in parallel if <see cref="IResponseTransferObject{T}.Data"/>
    //    /// exists with no <see cref="IResponseTransferObject{T}.Errors"/>.
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="responseTransferObject">The response transfer object.</param>
    //    /// <param name="cancellationToken">The cancellation token.</param>
    //    /// <param name="actions">The actions to invoke.</param>
    //    /// <returns>Instance of <see cref="IResponseTransferObjectAsync{T}"/>.</returns>
    //    /// <remarks></remarks>
    //    public static IResponseTransferObjectParallel<T> Let<T>(this IResponseTransferObjectParallel<T> responseTransferObject, CancellationToken cancellationToken, params Action<IEnumerable<T>>[] actions)
    //    {
    //        return Let(responseTransferObject, Environment.ProcessorCount, cancellationToken, actions);
    //    }

    //    /// <summary>
    //    /// Invokes the specified <paramref name="actions"/> in parallel if <see cref="IResponseTransferObject{T}.Data"/>
    //    /// exists with no <see cref="IResponseTransferObject{T}.Errors"/>.
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="responseTransferObject">The response transfer object.</param>
    //    /// <param name="cancellationToken">The cancellation token.</param>
    //    /// <param name="parallelExecutionMode">The parallel execution mode.</param>
    //    /// <param name="actions">The actions to invoke.</param>
    //    /// <returns>Instance of <see cref="IResponseTransferObjectAsync{T}"/>.</returns>
    //    /// <remarks></remarks>
    //    public static IResponseTransferObjectParallel<T> Let<T>(this IResponseTransferObjectParallel<T> responseTransferObject, CancellationToken cancellationToken, ParallelExecutionMode parallelExecutionMode, params Action<IEnumerable<T>>[] actions)
    //    {
    //        return Let(responseTransferObject, Environment.ProcessorCount, cancellationToken, parallelExecutionMode, actions);
    //    }

    //    /// <summary>
    //    /// Invokes the specified <paramref name="actions"/> in parallel if <see cref="IResponseTransferObject{T}.Data"/>
    //    /// exists with no <see cref="IResponseTransferObject{T}.Errors"/>.
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="responseTransferObject">The response transfer object.</param>
    //    /// <param name="degreeOfParallelism">The degree of parallelism.</param>
    //    /// <param name="actions">The actions to invoke.</param>
    //    /// <returns>Instance of <see cref="IResponseTransferObjectAsync{T}"/>.</returns>
    //    /// <remarks></remarks>
    //    public static IResponseTransferObjectParallel<T> Let<T>(this IResponseTransferObjectParallel<T> responseTransferObject, Int32 degreeOfParallelism, params Action<IEnumerable<T>>[] actions)
    //    {
    //        return Let(responseTransferObject, degreeOfParallelism, CancellationToken.None, ParallelExecutionMode.Default, actions);
    //    }

    //    /// <summary>
    //    /// Invokes the specified <paramref name="actions"/> in parallel if <see cref="IResponseTransferObject{T}.Data"/>
    //    /// exists with no <see cref="IResponseTransferObject{T}.Errors"/>.
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="responseTransferObject">The response transfer object.</param>
    //    /// <param name="degreeOfParallelism">The degree of parallelism.</param>
    //    /// <param name="cancellationToken">The cancellation token.</param>
    //    /// <param name="actions">The actions to invoke.</param>
    //    /// <returns>Instance of <see cref="IResponseTransferObjectAsync{T}"/>.</returns>
    //    /// <remarks></remarks>
    //    public static IResponseTransferObjectParallel<T> Let<T>(this IResponseTransferObjectParallel<T> responseTransferObject, Int32 degreeOfParallelism, CancellationToken cancellationToken, params Action<IEnumerable<T>>[] actions)
    //    {
    //        return Let(responseTransferObject, degreeOfParallelism, cancellationToken, ParallelExecutionMode.Default, actions);
    //    }

    //    /// <summary>
    //    /// Invokes the specified <paramref name="actions"/> in parallel if <see cref="IResponseTransferObject{T}.Data"/>
    //    /// exists with no <see cref="IResponseTransferObject{T}.Errors"/>.
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="responseTransferObject">The response transfer object.</param>
    //    /// <param name="degreeOfParallelism">The degree of parallelism.</param>
    //    /// <param name="cancellationToken">The cancellation token.</param>
    //    /// <param name="parallelExecutionMode">The parallel execution mode.</param>
    //    /// <param name="actions">The actions to invoke.</param>
    //    /// <returns>Instance of <see cref="IResponseTransferObjectAsync{T}"/>.</returns>
    //    /// <remarks></remarks>
    //    public static IResponseTransferObjectParallel<T> Let<T>(this IResponseTransferObjectParallel<T> responseTransferObject, Int32 degreeOfParallelism, CancellationToken cancellationToken, ParallelExecutionMode parallelExecutionMode, params Action<IEnumerable<T>>[] actions)
    //    {
    //        if (!responseTransferObject.Errors.Any())
    //        {
    //            var cancellationTokenSource = new CancellationTokenSource();
    //            if (cancellationToken != CancellationToken.None)
    //            {
    //                cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationTokenSource.Token, cancellationToken);
    //            }

    //            var task = Task.Factory.StartNew(
    //                () =>
    //                {
    //                    if (degreeOfParallelism == default(Int32) || degreeOfParallelism == Int32.MaxValue)
    //                    {
    //                        degreeOfParallelism = 1;
    //                    }

    //                    actions.AsParallel()
    //                           .WithCancellation(cancellationTokenSource.Token)
    //                           .WithDegreeOfParallelism(degreeOfParallelism)
    //                           .WithExecutionMode(parallelExecutionMode)
    //                           .ForAll(a => a.Invoke(responseTransferObject.Data));
    //                },
    //                cancellationTokenSource.Token);

    //            return new ServiceResponseParallelDecorator<T>(responseTransferObject, task, cancellationTokenSource);
    //        }

    //        return responseTransferObject;
    //    }

    //    /// <summary>
    //    /// Posts the specified <see cref="IResponseTransferObjectParallel{T}.Data"/> to the <see cref="ITargetBlock{T}"/>.
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <typeparam name="TTargetBlock">The type of the target block.</typeparam>
    //    /// <param name="responseTransferObject">The response transfer object.</param>
    //    /// <param name="targetBlock">The target block.</param>
    //    /// <returns></returns>
    //    /// <remarks></remarks>
    //    public static IResponseTransferObjectParallel<T> Post<T, TTargetBlock>(this IResponseTransferObjectParallel<T> responseTransferObject, TTargetBlock targetBlock) where TTargetBlock : ITargetBlock<T>
    //    {
    //        if (targetBlock == null)
    //        {
    //            throw new ArgumentNullException("targetBlock");
    //        }

    //        if (!responseTransferObject.Errors.Any() && responseTransferObject.Data.Any())
    //        {
    //            var cancellationTokenSource = new CancellationTokenSource();
    //            Task.Factory.StartNew(
    //                () =>
    //                {
    //                    foreach (var value in responseTransferObject.Data)
    //                    {
    //                        targetBlock.Post(value);
    //                    }

    //                    targetBlock.Complete();
    //                },
    //                cancellationTokenSource.Token);

    //            return new ServiceResponseParallelDecorator<T>(responseTransferObject, targetBlock.Completion, cancellationTokenSource);
    //        }

    //        return responseTransferObject;
    //    }
    //}
}