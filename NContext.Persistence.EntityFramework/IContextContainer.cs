// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IContextContainer.cs">
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

// <summary>
//   Defines a contract which aides in management of a distinct collection of <see cref="DbContext"/>s 
//   based on type. It is responsible for ensuring that only one instance of a given context 
//   exists at any given point, per thread. This is done through Unity LifetimeManagers.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Data.Entity;

namespace NContext.Persistence.EntityFramework
{
    /// <summary>
    /// Defines a contract which aides in management of a distinct collection of <see cref="DbContext"/>s 
    /// based on type. It is responsible for ensuring that only one instance of a given context 
    /// exists at any given point, per thread. This is done through Unity LifetimeManagers.
    /// </summary>
    public interface IContextContainer
    {
        /// <summary>
        /// Gets all contexts.
        /// </summary>
        IEnumerable<DbContext> Contexts { get; }

        /// <summary>
        /// Gets or creates the <typeparamref name="TContext"/> context.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <returns>Instance of <typeparamref name="TContext"/>.</returns>
        /// <remarks></remarks>
        TContext GetContext<TContext>() where TContext : DbContext;
    }
}