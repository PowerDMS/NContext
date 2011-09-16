// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Nothing.cs">
//   This file is part of NContext.
//
//   NContext is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or any later version.
//
//   NContext is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with NContext.  If not, see <http://www.gnu.org/licenses/>.// </copyright>
// <summary>
//   Defines a Nothing implementation of IMaybe<T>.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace NContext.Application.Extensions
{
    /// <summary>
    /// Defines a Nothing implementation of <see cref="IMaybe{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
        /// Returns a new <see cref="Nothing{T}"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="bindFunc">The function used to map.</param>
        /// <returns>Instance of <see cref="IMaybe{TResult}"/>.</returns>
        /// <remarks></remarks>
        public IMaybe<TResult> Bind<TResult>(Func<T, IMaybe<TResult>> bindFunc)
        {
            return new Nothing<TResult>();
        }
    }
}
