namespace NContext.Data.Persistence
{
    public sealed class PerThreadAmbientContextManagerFactory : IAmbientContextManagerFactory
    {
        public AmbientContextManagerBase Create()
        {
            return new PerThreadAmbientContextManager();
        }
    }
}