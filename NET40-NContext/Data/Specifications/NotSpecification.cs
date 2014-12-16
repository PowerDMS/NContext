namespace NContext.Data.Specifications
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using NContext.Data.Persistence;

    /// <summary>
    /// Defines a specification which converts an original specification with inverse logic.
    /// </summary>
    /// <typeparam name="TEntity">Type of element for this specificaiton</typeparam>
    public sealed class NotSpecification<TEntity> : SpecificationBase<TEntity> where TEntity : class, IEntity
    {
        private readonly Expression<Func<TEntity, Boolean>> _OriginalCriteria;

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

            _OriginalCriteria = originalSpecification.IsSatisfiedBy();
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

        /// <summary>
        /// Returns a boolean expression which determines whether the specification is satisfied.
        /// </summary>
        /// <returns>Expression that evaluates whether the specification satifies the expression.</returns>
        public override Expression<Func<TEntity, Boolean>> IsSatisfiedBy()
        {
            return Expression.Lambda<Func<TEntity, Boolean>>(Expression.Not(_OriginalCriteria.Body), _OriginalCriteria.Parameters.Single());
        }
    }
}