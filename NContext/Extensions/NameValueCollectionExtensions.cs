// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NameValueCollectionExtensions.cs">
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
//   Defines extension methods for NameValueCollections.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace NContext.Extensions
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