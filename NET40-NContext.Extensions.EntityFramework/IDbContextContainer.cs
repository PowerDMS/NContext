namespace NContext.Extensions.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;

    /// <summary>
    /// Defines a contract which aides in management of a distinct collection of <see cref="DbContext"/>s 
    /// based on type. It is responsible for ensuring that only one instance of a given context 
    /// exists at any given time per ambient context.
    /// </summary>
    public interface IDbContextContainer : IDisposable
    {
        Guid Id { get; }

        /// <summary>
        /// Gets all DbContexts.
        /// </summary>
        IEnumerable<DbContext> Contexts { get; }

        Boolean IsDisposing { get; }

        Boolean IsDisposed { get; }

        void Add(DbContext dbContext);

        void Add(String key, DbContext dbContext);

        Boolean Contains(String key);

        Boolean Contains(DbContext dbContext);

        /// <summary>
        /// Gets or creates the <typeparamref name="TContext"/> context.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <returns>Instance of <typeparamref name="TContext"/>.</returns>
        /// <remarks></remarks>
        TContext GetContext<TContext>() where TContext : DbContext;

        DbContext GetContext(Type contextType);

        DbContext GetContext(String key);
    }
}