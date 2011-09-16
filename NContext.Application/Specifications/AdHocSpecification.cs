// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdHocSpecification.cs">
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
//   Defines a specification which is composed AdHoc via constructor.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq.Expressions;

namespace NContext.Application.Domain
{
    /// <summary>
    /// Defines a specification which is composed AdHoc via constructor.
    /// </summary>
    /// <typeparam name = "TEntity">Type of entity that check this specification</typeparam>
    public sealed class AdHocSpecification<TEntity> : SpecificationBase<TEntity> where TEntity : class, IEntity
    {
        #region Fields

        private readonly Expression<Func<TEntity, Boolean>> _MatchingCriteria;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AdHocSpecification{TEntity}"/> class.
        /// </summary>
        /// <param name="matchingCriteria">The matching criteria.</param>
        /// <remarks></remarks>
        public AdHocSpecification(Expression<Func<TEntity, Boolean>> matchingCriteria)
        {
            if (matchingCriteria == null)
            {
                throw new ArgumentNullException("matchingCriteria");
            }

            _MatchingCriteria = matchingCriteria;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a boolean expression which determines whether the specification is satisfied.
        /// </summary>
        /// <returns>Expression that evaluates whether the specification satifies the expression.</returns>
        public override Expression<Func<TEntity, Boolean>> IsSatisfied()
        {
            return _MatchingCriteria;
        }

        #endregion
    }
}