namespace NContext.Data.Specifications
{
    using System;
    using System.Linq.Expressions;

    using NContext.Data.Persistence;

    /// <summary>
    /// Defines a generic specification which can be used for composition.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity in this specification.</typeparam>
    /// <remarks></remarks>
    public sealed class TrueSpecification<TEntity> : SpecificationBase<TEntity> where TEntity : class, IEntity
    {
        /// <summary>
        /// Returns a boolean expression which determines whether the specification is satisfied.
        /// </summary>
        /// <returns>Expression that evaluates whether the specification satifies the expression.</returns>
        public override Expression<Func<TEntity, Boolean>> IsSatisfiedBy()
        {
            Boolean result = true;
            Expression<Func<TEntity, Boolean>> trueExpression = t => result;

            return trueExpression;
        }
    }
}