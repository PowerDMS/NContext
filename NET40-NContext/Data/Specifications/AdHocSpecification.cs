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