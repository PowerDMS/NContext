// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEnumerableExtensions.cs">
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
//   Defines extension methods for IEnumerable.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using NContext.Data;
using NContext.Data.Specifications;

namespace NContext.Extensions
{
    using NContext.Data.Persistence;

    /// <summary>
    /// Defines extension methods for IEnumerable.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Queries the context based on the provided specification and returns results that
        /// are only satisfied by the specification.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <paramref name="entities"/>
        /// <param name="specification">A <see cref="SpecificationBase{TEntity}"/> instance used to filter results
        /// that only satisfy the specification.</param>
        /// <returns>
        /// A <see cref="IQueryable{TEntity}"/> that can be used to enumerate over the results
        /// of the query.
        /// </returns>
        public static IEnumerable<TEntity> AllMatching<TEntity>(this IEnumerable<TEntity> entities, SpecificationBase<TEntity> specification)
            where TEntity : class, IEntity
        {
            return entities.AsQueryable().Where(specification.IsSatisfiedBy());
        } 
    }
}