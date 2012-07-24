namespace NContext.Extensions.EntityFramework
{
    using System;
    using System.Data.Entity;

    using Microsoft.Practices.ServiceLocation;

    public class ServiceLocatorDbContextFactory : IDbContextFactory
    {
        #region Implementation of IDbContextFactory

        public DbContext Create()
        {
            return GetContextFromServiceLocation<DbContext>(null);
        }

        public DbContext Create(String key)
        {
            return GetContextFromServiceLocation<DbContext>(key);
        }

        public TDbContext Create<TDbContext>() where TDbContext : DbContext
        {
            return GetContextFromServiceLocation<TDbContext>(null);
        }

        #endregion

        protected TDbContext GetContextFromServiceLocation<TDbContext>(String registeredNameForServiceLocation) where TDbContext : DbContext
        {
            var context = String.IsNullOrWhiteSpace(registeredNameForServiceLocation)
                              ? ServiceLocator.Current.GetInstance<TDbContext>()
                              : ServiceLocator.Current.GetInstance<TDbContext>(registeredNameForServiceLocation);

            if (context == null)
            {
                throw new ArgumentException("Context is not registered for service location.");
            }

            return context;
        }
    }
}