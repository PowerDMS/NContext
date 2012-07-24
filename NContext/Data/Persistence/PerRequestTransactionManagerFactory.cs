namespace NContext.Data.Persistence
{
    using System;

    public sealed class PerRequestTransactionManagerFactory : IAmbientTransactionManagerFactory
    {
        #region Implementation of IAmbientTransactionManagerFactory

        public Func<AmbientTransactionManagerBase> Create()
        {
            return () => new PerRequestTransactionManager();
        }

        #endregion
    }
}