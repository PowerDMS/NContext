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
using System.Threading.Tasks;

using NContext.Dto;

namespace NContext.Extensions
{
    /// <summary>
    /// Defines extension methods for <see cref="IResponseTransferObject{T}"/>.
    /// </summary>
    public static class IResponseTransferObjectExtensions
    {
        #region CatchParallel

        /// <summary>
        /// Invokes the specified <paramref name="actions"/> in parallel if <see cref="IResponseTransferObject{T}.Errors"/>
        /// exist with no <see cref="IResponseTransferObject{T}.Data"/>.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IResponseTransferObject{T}.Data"/>.</typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="maxDegreeOfParallelism">The max degree of parallelism.</param>
        /// <param name="actions">The actions to invoke.</param>
        /// <returns>Instance of <see cref="Task{IResponseTransferObject}"/> with the current <see cref="IResponseTransferObject{T}"/>.</returns>
        /// <remarks></remarks>
        public static IResponseTransferObjectAsync<T> CatchAsync<T>(this IResponseTransferObject<T> responseTransferObject, Int32 maxDegreeOfParallelism = 1, params Action<IEnumerable<Error>>[] actions)
        {
            return new ServiceResponseAsyncDecorator<T>(responseTransferObject).CatchParallel(maxDegreeOfParallelism, actions);
        }

        #endregion

        #region LetParallel

        /// <summary>
        /// Invokes the specified <paramref name="actions"/> in parallel if <see cref="IResponseTransferObject{T}.Data"/>
        /// exist with no <see cref="IResponseTransferObject{T}.Errors"/>.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IResponseTransferObject{T}.Data"/>.</typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="maxDegreeOfParallelism">The max degree of parallelism.</param>
        /// <param name="actions">The actions to invoke.</param>
        /// <returns>Instance of <see cref="Task{IResponseTransferObject}"/> with the current <see cref="IResponseTransferObject{T}"/>.</returns>
        /// <remarks></remarks>
        public static IResponseTransferObjectAsync<T> LetParallel<T>(this IResponseTransferObject<T> responseTransferObject, Int32 maxDegreeOfParallelism = 1, params Action<IEnumerable<T>>[] actions)
        {
            return new ServiceResponseAsyncDecorator<T>(responseTransferObject).LetParallel(maxDegreeOfParallelism, actions);
        }

        #endregion
    }
}