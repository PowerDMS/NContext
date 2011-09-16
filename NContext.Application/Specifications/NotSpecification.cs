// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotSpecification.cs">
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
//   Defines a specification which converts an original specification with inverse logic.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;

namespace NContext.Application.Domain
{
    /// <summary>
    /// Defines a specification which converts an original specification with inverse logic.
    /// </summary>
    /// <typeparam name="TEntity">Type of element for this specificaiton</typeparam>
    public sealed class NotSpecification<TEntity> : SpecificationBase<TEntity> where TEntity : class, IEntity
    {
        #region Fields

        private readonly Expression<Func<TEntity, Boolean>> _OriginalCriteria;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NotSpecification&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="originalSpecification">The original specification.</param>
        /// <remarks></remarks>
        public NotSpecification(SpecificationBase<TEntity> originalSpecification)
        {
            if (originalSpecification == null)
            {
                throw new ArgumentNullException("originalSpecification");
            }

            _OriginalCriteria = originalSpecification.IsSatisfied();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotSpecification&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="originalSpecification">The original specification.</param>
        /// <remarks></remarks>
        public NotSpecification(Expression<Func<TEntity, Boolean>> originalSpecification)
        {
            if (originalSpecification == null)
            {
                throw new ArgumentNullException("originalSpecification");
            }

            _OriginalCriteria = originalSpecification;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a boolean expression which determines whether the specification is satisfied.
        /// </summary>
        /// <returns>Expression that evaluates whether the specification satifies the expression.</returns>
        public override Expression<Func<TEntity, Boolean>> IsSatisfied()
        {
            return Expression.Lambda<Func<TEntity, Boolean>>(Expression.Not(_OriginalCriteria.Body), _OriginalCriteria.Parameters.Single());
        }

        #endregion
    }
}