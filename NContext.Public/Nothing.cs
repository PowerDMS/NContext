// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Nothing.cs">
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
//   Defines a Nothing implementation of IMaybe<T>.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace NContext
{
    /// <summary>
    /// Defines a Nothing implementation of <see cref="IMaybe{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of object.</typeparam>
    public sealed class Nothing<T> : IMaybe<T>
    {
        /// <summary>
        /// Gets a value indicating whether the instance is <see cref="Just{T}"/>.
        /// </summary>
        /// <remarks></remarks>
        public Boolean IsJust
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the instance is <see cref="Nothing{T}"/>.
        /// </summary>
        /// <remarks></remarks>
        public Boolean IsNothing
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Returns the specified default value if the <see cref="IMaybe{T}"/> is <see cref="Nothing{T}"/>; 
        /// otherwise, it returns the value contained in the <see cref="IMaybe{T}"/>.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Instance of <typeparamref name="T"/>.</returns>
        /// <remarks></remarks>
        public T FromMaybe(T defaultValue)
        {
            return defaultValue;
        }

        /// <summary>
        /// Returns this instance.
        /// </summary>
        /// <returns></returns>
        public IMaybe<T> Empty()
        {
            return this;
        }

        /// <summary>
        /// Returns a new <see cref="Nothing{T2}"/>.
        /// </summary>
        /// <typeparam name="T2">The type of the result.</typeparam>
        /// <param name="bindingFunction">The function used to bind.</param>
        /// <returns>Instance of <see cref="IMaybe{TResult}"/>.</returns>
        /// <remarks></remarks>
        public IMaybe<T2> Bind<T2>(Func<T, IMaybe<T2>> bindingFunction)
        {
            return new Nothing<T2>();
        }

        /// <summary>
        /// Returns the current instance.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        /// <returns>Current instance.</returns>
        /// <remarks></remarks>
        public IMaybe<T> Let(Action<T> action)
        {
            return this;
        }
    }
}