// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EagerLoadingPath.cs">
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
//   Summary description for EagerLoadingPath
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using NContext.Application.Domain;

namespace NContext.Persistence
{
    /// <summary>
    /// Summary description for EagerLoadingPath
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <remarks></remarks>
    public class EagerLoadingPath<TEntity> where TEntity : IEntity
    {
        private readonly IList<Expression> _Paths;

        /// <summary>
        /// Default Constructor.
        /// Creates a new instance of the <see cref="EagerLoadingPath{TEntity}"/> instance.
        /// </summary>
        /// <param name="paths">The paths.</param>
        /// <remarks></remarks>
        public EagerLoadingPath(IList<Expression> paths)
        {
            _Paths = paths;
        }

        /// <summary>
        /// Specify an eager fetching path on <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TChild">The type of the child.</typeparam>
        /// <param name="path">The path.</param>
        /// <returns>The eagerly fetched path.</returns>
        /// <remarks></remarks>
        public EagerLoadingPath<TChild> And<TChild>(Expression<Func<TEntity, Object>> path) where TChild : IEntity
        {
            _Paths.Add(path);
            return new EagerLoadingPath<TChild>(_Paths);
        }
    }
}