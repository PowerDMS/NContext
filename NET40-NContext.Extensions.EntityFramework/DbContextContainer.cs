namespace NContext.Extensions.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    /// <summary>
    /// Defines an implementation of IDbContextContainer which is responsible for ensuring that only one instance 
    /// of a given <see cref="DbContext"/> exists within a <see cref="IEfUnitOfWork"/>.
    /// </summary>
    /// <remarks></remarks>
    internal class DbContextContainer : IDbContextContainer
    {
        private readonly Guid _Id;

        private readonly IDictionary<String, DbContext> _Contexts;

        private Boolean _IsDisposed;

        private Boolean _IsDisposing;

        protected internal DbContextContainer()
        {
            _Id = Guid.NewGuid();
            _Contexts = new Dictionary<String, DbContext>();
        }

        /// <summary>
        /// Gets all contexts.
        /// </summary>
        public IEnumerable<DbContext> Contexts
        {
            get { return _Contexts.Values; }
        }

        public Guid Id
        {
            get
            {
                return _Id;
            }
        }

        public Boolean IsDisposing
        {
            get
            {
                return _IsDisposing;
            }
        }

        public Boolean IsDisposed
        {
            get
            {
                return _IsDisposed;
            }
        }

        public void Add(DbContext dbContext)
        {
            if (Contains(dbContext.GetType().Name))
            {
                return;
            }

            _Contexts.Add(dbContext.GetType().Name, dbContext);
        }

        public void Add(String key, DbContext dbContext)
        {
            _Contexts.Add(key, dbContext);
        }

        public Boolean Contains(String key)
        {
            return _Contexts.ContainsKey(key);
        }

        public Boolean Contains(DbContext dbContext)
        {
            return _Contexts.ContainsKey(dbContext.GetType().Name);
        }

        /// <summary>
        /// Gets or creates the <typeparamref name="TContext"/> context.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <returns>Instance of <typeparamref name="TContext"/>.</returns>
        /// <remarks></remarks>
        public TContext GetContext<TContext>() 
            where TContext : DbContext
        {
            if (_Contexts.ContainsKey(typeof(TContext).Name))
            {
                return _Contexts[typeof(TContext).Name] as TContext;
            }

            return null;
        }

        public DbContext GetContext(Type contextType)
        {
            return GetContext(contextType.Name);
        }

        public DbContext GetContext(String key)
        {
            return Contains(key) ? _Contexts.Single(c => c.Key == key).Value : null;
        }

        #region Implementation of IDisposable
        
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(Boolean disposeManagedResources)
        {
            if (IsDisposed) return;

            if (disposeManagedResources)
            {
                _IsDisposing = true;
                foreach (var dbContext in Contexts)
                {
                    dbContext.Dispose();
                }

                _IsDisposing = false;
            }

            _IsDisposed = true;
        }

        #endregion
    }
}