// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompositeSpecification.cs">
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
//   Defines a base abstraction for creating composite specifications.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.Application.Domain
{
    /// <summary>
    /// Defines a base abstraction for creating composite specifications.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity.</typeparam>
    public abstract class CompositeSpecification<TEntity> : SpecificationBase<TEntity> where TEntity : class, IEntity
    {
        #region Properties

        /// <summary>
        /// Gets the left side specification for this composite element.
        /// </summary>
        /// <remarks></remarks>
        public abstract SpecificationBase<TEntity> LeftSideSpecification { get; }

        /// <summary>
        /// Gets the right side specification for this composite element.
        /// </summary>
        public abstract SpecificationBase<TEntity> RightSideSpecification { get; }

        #endregion
    }
}