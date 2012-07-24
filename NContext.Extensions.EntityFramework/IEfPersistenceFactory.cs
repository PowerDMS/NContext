namespace NContext.Extensions.EntityFramework
{
    using System;
    using System.Data.Entity;

    using NContext.Data;
    using NContext.Data.Persistence;

    public interface IEfPersistenceFactory : IPersistenceFactory
    {
        /// <summary>
        /// Creates an <see cref="IEfGenericRepository{TEntity}"/> instance using the application's default <see cref="DbContext"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity to create.</typeparam>
        /// <returns>Instance of <see cref="IEfGenericRepository{TEntity}"/>.</returns>
        /// <remarks></remarks>
        IEfGenericRepository<TEntity> CreateRepository<TEntity>() where TEntity : class, IEntity;

        /// <summary>
        /// Creates an <see cref="IEfGenericRepository{TEntity}"/> instance using the specified <see cref="DbContext"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity to create.</typeparam>
        /// <typeparam name="TDbContext">The type of <see cref="DbContext"/>.</typeparam>
        /// <returns>Instance of <see cref="IEfGenericRepository{TEntity}"/>.</returns>
        /// <remarks></remarks>
        IEfGenericRepository<TEntity> CreateRepository<TEntity, TDbContext>()
            where TEntity : class, IEntity
            where TDbContext : DbContext;

        /// <summary>
        /// Creates an <see cref="IEfGenericRepository{TEntity}"/> instance using the <see cref="DbContext"/> registered with the specified key.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity to create.</typeparam>
        /// <param name="registeredDbContextNameForServiceLocation">The context's registered name for service location.</param>
        /// <returns>Instance of <see cref="IEfGenericRepository{TEntity}"/>.</returns>
        /// <remarks></remarks>
        IEfGenericRepository<TEntity> CreateRepository<TEntity>(String registeredDbContextNameForServiceLocation)
            where TEntity : class, IEntity;

        /// <summary>
        /// Gets the default context from the ambient <see cref="IEfUnitOfWork"/> if one exists, else it tries to create a new context via service location.
        /// </summary>
        /// <returns>Instance of <see cref="DbContext"/>.</returns>
        /// <remarks></remarks>
        DbContext GetOrCreateDbContext();

        /// <summary>
        /// Gets the context from the ambient <see cref="IEfUnitOfWork"/> if one exists, 
        /// else it tries to create a new one via service location with the specified registered name.
        /// </summary>
        /// <param name="registeredNameForServiceLocation">The context's registered name for service location.</param>
        /// <returns>Instance of <see cref="DbContext"/>.</returns>
        /// <remarks></remarks>
        DbContext GetOrCreateDbContext(String registeredNameForServiceLocation);
        
        /// <summary>
        /// Gets the specified context from the ambient <see cref="IEfUnitOfWork"/> if one exists, else it tries to create a new one via service location.
        /// </summary>
        /// <returns>Instance of <see cref="DbContext"/>.</returns>
        /// <remarks></remarks>
        TDbContext GetOrCreateDbContext<TDbContext>()
            where TDbContext : DbContext;
    }
}