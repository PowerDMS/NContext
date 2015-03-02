namespace NContext.Data.Persistence
{
    public sealed class PerRequestAmbientContextManagerFactory : IAmbientContextManagerFactory
    {
        public AmbientContextManagerBase Create()
        {
            return new PerRequestAmbientContextManager();
        }
    }
}