// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdHocSpecification.cs" company="Waking Venture, Inc.">
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
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.Data.Specifications
{
    using System;
    using System.Linq.Expressions;

    using NContext.Data.Persistence;

    /// <summary>
    /// Defines a specification which is composed AdHoc via constructor.
    /// </summary>
    /// <typeparam name = "TEntity">Type of entity that check this specification</typeparam>
    public sealed class AdHocSpecification<TEntity> : SpecificationBase<TEntity> where TEntity : class, IEntity
    {
        private readonly Expression<Func<TEntity, Boolean>> _MatchingCriteria;

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

        /// <summary>
        /// Returns a boolean expression which determines whether the specification is satisfied.
        /// </summary>
        /// <returns>Expression that evaluates whether the specification satifies the expression.</returns>
        public override Expression<Func<TEntity, Boolean>> IsSatisfiedBy()
        {
            return _MatchingCriteria;
        }
    }
}