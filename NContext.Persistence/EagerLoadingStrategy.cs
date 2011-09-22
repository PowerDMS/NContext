// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EagerLoadingStrategy.cs">
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
//   Summary description for EagerLoadingStrategy
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using NContext.Application.Domain;

namespace NContext.Persistence
{
    /// <summary>
    /// Defines the root interface to specify eager fetching strategy.
    /// </summary>
    /// <typeparam name="TEntity">The entity for eager fetching strategy.</typeparam>
    public class EagerLoadingStrategy<TEntity> where TEntity : IEntity
    {
        private readonly IList<Expression> _Paths = new List<Expression>();

        /// <summary>
        /// An array of <see cref="Expression"/> containing the eager fetching paths.
        /// </summary>
        /// <remarks></remarks>
        public IEnumerable<Expression> Paths
        {
            get { return _Paths.ToArray(); }
        }

        /// <summary>
        /// Specify the path to eagerly fetch.
        /// </summary>
        /// <typeparam name="TChild">The type of the child.</typeparam>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public EagerLoadingPath<TChild> Fetch<TChild>(Expression<Func<TEntity, Object>> path) where TChild : IEntity
        {
            _Paths.Add(path);
            return new EagerLoadingPath<TChild>(_Paths);
        }
    }
}