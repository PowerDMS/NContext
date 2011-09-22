// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUnitOfWork.cs">
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
//   Defines an interface contract for the UnitOfWork pattern.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace NContext.Persistence.EntityFramework
{
    /// <summary>
    /// Defines an interface contract for the UnitOfWork pattern.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the context container.
        /// </summary>
        /// <remarks></remarks>
        IContextContainer ContextContainer { get; }

        /// <summary>
        /// Commits the changes in each context to the database.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rolls back each context within the unit of work.
        /// </summary>
        void Rollback();

        /// <summary>
        /// Validates the contexts.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        IEnumerable<DbEntityValidationResult> Validate();
    }
}