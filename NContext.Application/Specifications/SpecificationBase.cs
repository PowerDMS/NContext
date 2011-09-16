// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpecificationBase.cs">
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
//   Defines a base abstraction for creating specifications.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq.Expressions;

namespace NContext.Application.Domain
{
    /// <summary>
    /// Defines a base abstraction for creating specifications.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <remarks></remarks>
    public abstract class SpecificationBase<TEntity> where TEntity : class, IEntity
    {
        #region Constructor

        protected SpecificationBase()
        {
            _lazyCompiledFunction = new Lazy<Func<TEntity, bool>>(() => IsSatisfied().Compile());
        }
        
        #endregion

        #region Operator Overrides

        private readonly Lazy<Func<TEntity, Boolean>> _lazyCompiledFunction;

        /// <summary>
        ///  And operator
        /// </summary>
        /// <param name="leftSideSpecification">left operand in this AND operation</param>
        /// <param name="rightSideSpecification">right operand in this AND operation</param>
        /// <returns>New specification</returns>
        public static SpecificationBase<TEntity> operator &(
            SpecificationBase<TEntity> leftSideSpecification, SpecificationBase<TEntity> rightSideSpecification)
        {
            return new AndSpecification<TEntity>(leftSideSpecification, rightSideSpecification);
        }

        /// <summary>
        /// Or operator
        /// </summary>
        /// <param name="leftSideSpecification">left operand in this OR operation</param>
        /// <param name="rightSideSpecification">right operand in this OR operation</param>
        /// <returns>New specification </returns>
        public static SpecificationBase<TEntity> operator |(
            SpecificationBase<TEntity> leftSideSpecification, SpecificationBase<TEntity> rightSideSpecification)
        {
            return new OrSpecification<TEntity>(leftSideSpecification, rightSideSpecification);
        }

        /// <summary>
        /// Not specification
        /// </summary>
        /// <param name="specification">Specification to negate</param>
        /// <returns>New specification</returns>
        public static SpecificationBase<TEntity> operator !(SpecificationBase<TEntity> specification)
        {
            return new NotSpecification<TEntity>(specification);
        }

        /// <summary>
        /// Override operator false, only for support AND OR operators
        /// </summary>
        /// <param name="specification">Specification instance</param>
        /// <returns>See False operator in C#</returns>
        public static Boolean operator false(SpecificationBase<TEntity> specification)
        {
            return false;
        }

        /// <summary>
        /// Override operator True, only for support AND OR operators
        /// </summary>
        /// <param name="specification">Specification instance</param>
        /// <returns>See True operator in C#</returns>
        public static Boolean operator true(SpecificationBase<TEntity> specification)
        {
            return true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a boolean expression which determines whether the specification is satisfied.
        /// </summary>
        /// <returns>Expression that evaluates whether the specification satifies the expression.</returns>
        public abstract Expression<Func<TEntity, Boolean>> IsSatisfied();

        /// <summary>
        /// Determines whether [is satisfied by] [the specified entity].
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><c>true</c> if [is satisfied by] [the specified entity]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public Boolean IsSatisfiedBy(TEntity entity)
        {
            return _lazyCompiledFunction.Value.Invoke(entity);
        }

        #endregion
    }
}