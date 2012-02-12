// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMaybe.cs">
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
//   Defines a Maybe monad interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace NContext
{
    /// <summary>
    /// Defines a Maybe monad interface.
    /// </summary>
    /// <typeparam name="T">The type to wrap.</typeparam>
    public interface IMaybe<T>
    {
        /// <summary>
        /// Gets a value indicating whether the instance is <see cref="Just{T}"/>.
        /// </summary>
        /// <remarks></remarks>
        Boolean IsJust { get; }

        /// <summary>
        /// Gets a value indicating whether the instance is <see cref="Nothing{T}"/>.
        /// </summary>
        /// <remarks></remarks>
        Boolean IsNothing { get; }

        /// <summary>
        /// Returns the specified default value if the <see cref="IMaybe{T}"/> is <see cref="Nothing{T}"/>; 
        /// otherwise, it returns the value contained in the <see cref="IMaybe{T}"/>.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Instance of <typeparamref name="T"/>.</returns>
        /// <remarks></remarks>
        T FromMaybe(T defaultValue);

        /// <summary>
        /// Returns <see cref="Nothing{T}"/>
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        IMaybe<T> Empty();

        /// <summary>
        /// Returns a new <see cref="Nothing{T}"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="bindFunc">The function used to map.</param>
        /// <returns>Instance of <see cref="IMaybe{TResult}"/>.</returns>
        /// <remarks></remarks>
        IMaybe<TResult> Bind<TResult>(Func<T, IMaybe<TResult>> bindFunc);

        /// <summary>
        /// Invokes the specified action.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        /// <returns>Current instance.</returns>
        /// <remarks></remarks>
        IMaybe<T> Let(Action<T> action);
    }
}