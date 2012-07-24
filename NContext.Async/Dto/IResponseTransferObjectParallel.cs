namespace NContext.Dto
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    public interface IResponseTransferObjectParallel<T> : IResponseTransferObject<T>
    {
        Task Task { get; }

        CancellationTokenSource CancellationTokenSource { get; }

        /// <summary>
        /// Invokes the specified <paramref name="actions"/> in parallel if <see cref="IResponseTransferObject{T}.Errors"/>
        /// exist with no <see cref="IResponseTransferObject{T}.Data"/>.
        /// </summary>
        /// <param name="degreeOfParallelism">The degree of parallelism.</param>
        /// <param name="actions">The actions to invoke.</param>
        /// <returns>Instance of <see cref="IResponseTransferObjectAsync{T}"/>.</returns>
        /// <remarks></remarks>
        IResponseTransferObjectParallel<T> CatchParallel(Int32 degreeOfParallelism, params Action<IEnumerable<Error>>[] actions);

        /// <summary>
        /// Invokes the specified <paramref name="actions"/> in parallel if <see cref="IResponseTransferObject{T}.Errors"/>
        /// exist with no <see cref="IResponseTransferObject{T}.Data"/>.
        /// </summary>
        /// <param name="degreeOfParallelism">The degree of parallelism.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="actions">The actions to invoke.</param>
        /// <returns>Instance of <see cref="IResponseTransferObjectAsync{T}"/>.</returns>
        /// <remarks></remarks>
        IResponseTransferObjectParallel<T> CatchParallel(Int32 degreeOfParallelism, CancellationToken cancellationToken, params Action<IEnumerable<Error>>[] actions);

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
        IResponseTransferObjectParallel<T> CatchParallel(Int32 degreeOfParallelism, CancellationToken cancellationToken, ParallelExecutionMode parallelExecutionMode, params Action<IEnumerable<Error>>[] actions);

        IResponseTransferObjectParallel<T> Let(params Action<IEnumerable<T>>[] actions);

        /// <summary>
        /// Invokes the specified <paramref name="actions"/> in parallel if <see cref="IResponseTransferObject{T}.Data"/>
        /// exists with no <see cref="IResponseTransferObject{T}.Errors"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="actions">The actions to invoke.</param>
        /// <returns>Instance of <see cref="IResponseTransferObjectAsync{T}"/>.</returns>
        /// <remarks></remarks>
        IResponseTransferObjectParallel<T> Let(CancellationToken cancellationToken, params Action<IEnumerable<T>>[] actions);

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
        IResponseTransferObjectParallel<T> Let(CancellationToken cancellationToken, ParallelExecutionMode parallelExecutionMode, params Action<IEnumerable<T>>[] actions);

        /// <summary>
        /// Invokes the specified <paramref name="actions"/> in parallel if <see cref="IResponseTransferObject{T}.Data"/>
        /// exists with no <see cref="IResponseTransferObject{T}.Errors"/>.
        /// </summary>
        /// <param name="degreeOfParallelism">The degree of parallelism.</param>
        /// <param name="actions">The actions to invoke.</param>
        /// <returns>Instance of <see cref="IResponseTransferObjectAsync{T}"/>.</returns>
        /// <remarks></remarks>
        IResponseTransferObjectParallel<T> Let(Int32 degreeOfParallelism, params Action<IEnumerable<T>>[] actions);

        /// <summary>
        /// Invokes the specified <paramref name="actions"/> in parallel if <see cref="IResponseTransferObject{T}.Data"/>
        /// exists with no <see cref="IResponseTransferObject{T}.Errors"/>.
        /// </summary>
        /// <param name="degreeOfParallelism">The degree of parallelism.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="actions">The actions to invoke.</param>
        /// <returns>Instance of <see cref="IResponseTransferObjectAsync{T}"/>.</returns>
        /// <remarks></remarks>
        IResponseTransferObjectParallel<T> Let(Int32 degreeOfParallelism, CancellationToken cancellationToken, params Action<IEnumerable<T>>[] actions);

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
        IResponseTransferObjectParallel<T> Let(Int32 degreeOfParallelism, CancellationToken cancellationToken, ParallelExecutionMode parallelExecutionMode, params Action<IEnumerable<T>>[] actions);

        IResponseTransferObjectParallel<T> Post<TTargetBlock>(TTargetBlock targetBlock) where TTargetBlock : ITargetBlock<T>;

        /// <summary>
        /// If <seealso cref="IResponseTransferObject{T}.Errors"/> exist, returns a new <see cref="IResponseTransferObject{T2}"/> instance with the current 
        /// <seealso cref="IResponseTransferObject{T}.Errors"/>. Else, binds the data into the specified <paramref name="bindingFunction"/>.
        /// </summary>
        /// <typeparam name="T2">The type of the next <see cref="IResponseTransferObject{T2}"/> to return.</typeparam>
        /// <param name="bindingFunction">The binding function.</param>
        /// <returns>Instance of <see cref="IResponseTransferObject{T2}"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no errors or data exist.</exception>
        /// <remarks></remarks>
        IResponseTransferObjectParallel<T2> Bind<T2>(Func<IEnumerable<T>, Task, CancellationTokenSource, IResponseTransferObject<T2>> bindingFunction);

        /// <summary>
        /// Invokes the specified action if <see cref="IResponseTransferObject{T}.Data"/> exists with no <see cref="IResponseTransferObject{T}.Errors"/> present.
        /// Returns the current <see cref="IResponseTransferObject{T}"/> instance.
        /// </summary>
        /// <param name="action">The action to invoke with the data and continuation task.</param>
        /// <returns>The current <see cref="IResponseTransferObject{T}"/> instance.</returns>
        /// <remarks></remarks>
        IResponseTransferObjectParallel<T> Let(Action<IEnumerable<T>, Task, CancellationTokenSource> action);
    }
}