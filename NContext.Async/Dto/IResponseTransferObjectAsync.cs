// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IResponseTransferObject.cs">
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
//   Defines a data-transfer object contract used for asynchronous functional composition.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using NContext.Dto;

namespace NContext
{
    /// <summary>
    /// Defines a data-transfer object contract used for asynchronous functional composition.
    /// </summary>
    public interface IResponseTransferObjectAsync<T> : IResponseTransferObject<T>
    {
        /// <summary>
        /// Returns a new <see cref="IResponseTransferObject{T2}"/> instance with the current <see cref="IResponseTransferObject{T}.Errors"/> passed if <see cref="IResponseTransferObject{T}.Errors"/> exist.
        /// Binds the <see cref="IResponseTransferObject{T}.Data"/> into the specified <paramref name="bindingFunction" /> if <see cref="IResponseTransferObject{T}.Data"/> exists - returning a <see cref="IResponseTransferObject{T2}"/>.
        /// </summary>
        /// <typeparam name="T2">The type of the next <see cref="IResponseTransferObject{T2}"/> to return.</typeparam>
        /// <param name="bindingFunction">The binding function.</param>
        /// <returns>Instance of <see cref="IResponseTransferObject{T2}"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no errors or data exist.</exception>
        /// <remarks></remarks>
        IResponseTransferObject<T2> Bind<T2>(Func<IEnumerable<T>, Task, CancellationTokenSource, IResponseTransferObject<T2>> bindingFunction);

        /// <summary>
        /// Invokes the specified <paramref name="actions"/> in parallel if <see cref="IResponseTransferObject{T}.Errors"/>
        /// exist with no <see cref="IResponseTransferObject{T}.Data"/>.
        /// </summary>
        /// <param name="maxDegreeOfParallelism">The max degree of parallelism.</param>
        /// <param name="actions">The actions to invoke.</param>
        /// <returns>Instance of <see cref="IResponseTransferObjectAsync{T}"/>.</returns>
        /// <remarks></remarks>
        IResponseTransferObjectAsync<T> CatchParallel(Int32 maxDegreeOfParallelism = 1, params Action<IEnumerable<Error>>[] actions);

        /// <summary>
        /// Invokes the specified action if <see cref="IResponseTransferObject{T}.Data"/> exists with no <see cref="IResponseTransferObject{T}.Errors"/> present.
        /// Returns the current <see cref="IResponseTransferObject{T}"/> instance.
        /// </summary>
        /// <param name="action">The action to invoke with the data and continuation task.</param>
        /// <returns>The current <see cref="IResponseTransferObject{T}"/> instance.</returns>
        /// <remarks></remarks>
        IResponseTransferObject<T> Let(Action<IEnumerable<T>, Task, CancellationTokenSource> action);

        /// <summary>
        /// Invokes the specified <paramref name="actions"/> in parallel if <see cref="IResponseTransferObject{T}.Data"/>
        /// exists with no <see cref="IResponseTransferObject{T}.Errors"/>.
        /// </summary>
        /// <param name="maxDegreeOfParallelism">The max degree of parallelism.</param>
        /// <param name="actions">The actions to invoke.</param>
        /// <returns>Instance of <see cref="IResponseTransferObjectAsync{T}"/>.</returns>
        /// <remarks></remarks>
        IResponseTransferObjectAsync<T> LetParallel(Int32 maxDegreeOfParallelism = 1, params Action<IEnumerable<T>>[] actions);
    }
}