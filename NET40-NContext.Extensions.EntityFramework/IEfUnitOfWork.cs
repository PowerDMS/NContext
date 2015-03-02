namespace NContext.Extensions.EntityFramework
{
    using System.Collections.Generic;
    using System.Data.Entity.Validation;

    using NContext.Data.Persistence;

    /// <summary>
    /// Defines an Entity Framework unit of work.
    /// </summary>
    public interface IEfUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Gets the context container.
        /// </summary>
        /// <remarks></remarks>
        IDbContextContainer DbContextContainer { get; }

        /// <summary>
        /// Validates each context in the container.
        /// </summary>
        /// <returns>IEnumerable{DbEntityValidationResult}.</returns>
        IEnumerable<DbEntityValidationResult> Validate();
    }
}