// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMaybe.cs">
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
//   Defines a Maybe monad interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace NContext.Application.Extensions
{
    /// <summary>
    /// Defines a Maybe monad interface.
    /// </summary>
    /// <typeparam name="T">The type to wrap.</typeparam>
    /// <remarks>
    /// http://haskell.org/ghc/docs/latest/html/libraries/base/Data-Maybe.html
    /// </remarks>
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
    }
}