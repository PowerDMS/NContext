namespace NContext.Data.Specifications
{
    using NContext.Data.Persistence;

    /// <summary>
    /// Defines a base abstraction for creating composite specifications.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity.</typeparam>
    public abstract class CompositeSpecification<TEntity> : SpecificationBase<TEntity> where TEntity : class, IEntity
    {
        /// <summary>
        /// Gets the left side specification for this composite element.
        /// </summary>
        /// <remarks></remarks>
        public abstract SpecificationBase<TEntity> LeftSideSpecification { get; }

        /// <summary>
        /// Gets the right side specification for this composite element.
        /// </summary>
        public abstract SpecificationBase<TEntity> RightSideSpecification { get; }
    }
}