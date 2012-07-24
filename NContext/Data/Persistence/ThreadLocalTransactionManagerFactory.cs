namespace NContext.Data.Persistence
{
    using System;

    public sealed class ThreadLocalTransactionManagerFactory : IAmbientTransactionManagerFactory
    {
        #region Implementation of IAmbientTransactionManagerFactory

        public Func<AmbientTransactionManagerBase> Create()
        {
            return () => new ThreadLocalTransactionManager();
        }

        #endregion
    }
}