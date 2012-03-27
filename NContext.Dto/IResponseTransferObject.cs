// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IResponseTransferObject.cs">
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
//   Defines a data-transfer object contract used for functional composition.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace NContext.Dto
{
    /// <summary>
    /// Defines a data-transfer object contract used for functional composition.
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
        /// If <seealso cref="Errors"/> exist, returns a new <see cref="IResponseTransferObject{T2}"/> instance with the current 
        /// <seealso cref="Errors"/>. Else, binds the <seealso cref="Data"/> into the specified <paramref name="bindingFunction"/>.
        /// </summary>
        /// <typeparam name="T2">The type of the next <see cref="IResponseTransferObject{T2}"/> to return.</typeparam>
        /// <param name="bindingFunction">The binding function.</param>
        /// <returns>Instance of <see cref="IResponseTransferObject{T2}"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no errors or data exist.</exception>
        /// <remarks></remarks>
        IResponseTransferObject<T2> Bind<T2>(Func<IEnumerable<T>, IResponseTransferObject<T2>> bindingFunction);

        /// <summary>
        /// Invokes the specified action if there are any errors. Returns the current instance.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        /// <returns>The current <see cref="IResponseTransferObject{T}"/> instance.</returns>
        /// <remarks></remarks>
        IResponseTransferObject<T> Catch(Action<IEnumerable<Error>> action);

        /// <summary>
        /// Invokes the specified function if there are any errors - allows you to re-direct control flow with a new <typeparamref name="T"/> value.
        /// </summary>
        /// <param name="continueWithFunction">The continue with function.</param>
        /// <returns>If errors exist, returns the instance of IResponseTransferObject&lt;T&gt; returned by <paramref name="continueWithFunction"/>, else returns current instance.</returns>
        /// <remarks></remarks>
        IResponseTransferObject<T> CatchAndContinue(Func<IEnumerable<Error>, IResponseTransferObject<T>> continueWithFunction);

        /// <summary>
        /// Invokes the specified action if <see cref="Data"/> exists with no <see cref="Errors"/> present.
        /// Returns the current <see cref="IResponseTransferObject{T}"/> instance.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        /// <returns>The current <see cref="IResponseTransferObject{T}"/> instance.</returns>
        /// <remarks></remarks>
        IResponseTransferObject<T> Let(Action<IEnumerable<T>> action);
    }
}