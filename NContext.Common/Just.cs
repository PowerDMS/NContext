// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Just.cs" company="Waking Venture, Inc.">
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
// --------------------------------------------------------------------------------------------------------------------

namespace NContext
{
    using System;

    /// <summary>
    /// Defines a Just implementation of <see cref="IMaybe{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of object.</typeparam>
    public sealed class Just<T> : IMaybe<T>
    {
        private readonly T _Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Just{T}"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <remarks></remarks>
        public Just(T value)
        {
            _Value = value;
        }

        /// <summary>
        /// Gets a value indicating whether the instance is <see cref="Just{T}"/>.
        /// </summary>
        /// <remarks></remarks>
        public Boolean IsJust
        {
            get
            {
                return true;
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
                return false;
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
            return _Value;
        }

        /// <summary>
        /// Returns a new <see cref="Nothing{T}" />.
        /// </summary>
        /// <returns>Nothing{T}.</returns>
        public IMaybe<T> Empty()
        {
            return new Nothing<T>();
        }

        /// <summary>
        /// Binds <typeparamref name="T"/> into an instance of <see cref="IMaybe{T2}"/>.
        /// </summary>
        /// <typeparam name="T2">The type of the result.</typeparam>
        /// <param name="bindingFunction">The function used to bind.</param>
        /// <returns>Instance of <see cref="IMaybe{TResult}"/>.</returns>
        /// <remarks></remarks>
        public IMaybe<T2> Bind<T2>(Func<T, IMaybe<T2>> bindingFunction)
        {
            return bindingFunction.Invoke(_Value);
        }

        /// <summary>
        /// Invokes the specified action if the current instance <seealso cref="IMaybe{T}.IsJust"/>.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        /// <returns>Current instance.</returns>
        /// <remarks></remarks>
        public IMaybe<T> Let(Action<T> action)
        {
            action.Invoke(_Value);

            return this;
        }
    }
}