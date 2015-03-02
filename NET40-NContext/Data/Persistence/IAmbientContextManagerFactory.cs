namespace NContext.Data.Persistence
{
    /// <summary>
    /// Defines a factory method for extending <see cref="AmbientContextManagerBase"/> implementations.
    /// </summary>
    public interface IAmbientContextManagerFactory
    {
        /// <summary>
        /// Creates an instance of <see cref="AmbientContextManagerBase"/>.
        /// </summary>
        /// <returns>AmbientContextManagerBase concrete instance.</returns>
        AmbientContextManagerBase Create();
    }
}