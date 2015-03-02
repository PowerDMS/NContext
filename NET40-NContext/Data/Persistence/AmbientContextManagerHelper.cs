namespace NContext.Data.Persistence
{
    using System;

    public sealed class AmbientContextManagerHelper : IAmbientContextManagerFactory
    {
        private readonly Func<AmbientContextManagerBase> _AmbientContextManagerFactory;

        internal AmbientContextManagerHelper(Func<AmbientContextManagerBase> ambientContextManagerFactory)
        {
            _AmbientContextManagerFactory = ambientContextManagerFactory;
        }

        public static IAmbientContextManagerFactory CreateFactory<TAmbientContextManager>(Func<TAmbientContextManager> ambientContextManagerFactory) 
            where TAmbientContextManager : AmbientContextManagerBase
        {
            return new AmbientContextManagerHelper(ambientContextManagerFactory);
        }

        public static IAmbientContextManagerFactory CreateFactory<TAmbientContextManager>()
            where TAmbientContextManager : AmbientContextManagerBase, new()
        {
            return new AmbientContextManagerHelper(Activator.CreateInstance<TAmbientContextManager>);
        }

        public AmbientContextManagerBase Create()
        {
            return _AmbientContextManagerFactory();
        }
    }
}