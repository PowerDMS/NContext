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
//   Defines a data-transfer object contract used for domain composition.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace NContext.Application.Dto
{
    /// <summary>
    /// Defines a data-transfer object contract used for domain composition.
    /// </summary>
    /// <typeparam name="T">Type of data to return.</typeparam>
    /// <remarks></remarks>
    public interface IResponseTransferObject<T> : IDisposable
    {
        /// <summary>
        /// Gets the <typeparam name="T"/> data.
        /// </summary>
        IEnumerable<T> Data { get; }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <remarks></remarks>
        IEnumerable<Error> Errors { get; }

        /// <summary>
        /// Binds the specified <see cref="IEnumerable{T}"/> into the function which returns the specified <see cref="IResponseTransferObject{T}"/>.
        /// </summary>
        /// <typeparam name="T2">The type of the next <see cref="T"/> to return.</typeparam>
        /// <param name="func">The func.</param>
        /// <returns>Instance of <see cref="IResponseTransferObject{T}"/>.</returns>
        /// <remarks></remarks>
        IResponseTransferObject<T2> Bind<T2>(Func<IEnumerable<T>, IResponseTransferObject<T2>> func);

        /// <summary>
        /// Invokes the specified action if there are any errors. 
        /// Returns the current <see cref="IResponseTransferObject{T}"/>.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The current <see cref="IResponseTransferObject{T}"/> instance.</returns>
        /// <remarks></remarks>
        IResponseTransferObject<T> Catch(Action<IEnumerable<Error>> action);

        /// <summary>
        /// Invokes the specified action if there are no errors or validation results.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <remarks></remarks>
        void Do(Action<IEnumerable<T>> action);

        /// <summary>
        /// A combination of <see cref="IResponseTransferObject{T}.Bind{T2}"/> and <see cref="IResponseTransferObject{T}.Catch"/>. 
        /// It will invoke data function if there is any data, or errors function if there any errors exist.
        /// </summary>
        /// <typeparam name="T2">The type of the next data transfer object to return.</typeparam>
        /// <param name="data">The function to call if there is data and there are no errors.</param>
        /// <param name="errors">The function to call if there are any errors.</param>
        /// <returns>Instance of <see cref="IResponseTransferObject{T}"/>.</returns>
        IResponseTransferObject<T2> Either<T2>(Func<IEnumerable<T>, IResponseTransferObject<T2>> data,
                                               Func<IEnumerable<Error>, IResponseTransferObject<T2>> errors);

        /// <summary>
        /// Invokes the specified action if there is data. 
        /// Returns the current <see cref="IResponseTransferObject{T}"/>.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        /// <returns>The current <see cref="IResponseTransferObject{T}"/> instance.</returns>
        /// <remarks></remarks>
        IResponseTransferObject<T> Let(Action<IEnumerable<T>> action);
    }
}