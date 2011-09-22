// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NameValueCollectionExtensions.cs">
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
//   along with NContext.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
//
// <summary>
//   Defines extension methods for NameValueCollections.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace NContext.Application.Extensions
{
    /// <summary>
    /// Defines extension methods for <see cref="NameValueCollection"/>s.
    /// </summary>
    public static class NameValueCollectionExtensions
    {
        /// <summary>
        /// Converts the <see cref="NameValueCollection"/> to a <see cref="Dictionary{TKey,TValue}"/>
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns><see cref="Dictionary{TKey,TValue}"/> which can be enumerated on.</returns>
        /// <remarks></remarks>
        public static IDictionary<String, String> ToDictionary(this NameValueCollection source)
        {
            if (source == null || source.Count <= 0)
            {
                return new Dictionary<String, String>();
            }

            return source.Cast<String>()
                         .Where(key => !String.IsNullOrWhiteSpace(key))
                         .Select(key => new KeyValuePair<String, String>(key, source[key]))
                         .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}