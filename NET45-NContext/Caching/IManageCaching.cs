namespace NContext.Caching
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Caching;

    using NContext.Configuration;

    /// <summary>
    /// Defines a contract for managing the application's cache provider.
    /// </summary>
    public interface IManageCaching : IApplicationComponent
    {
        /// <summary>
        /// Gets the cache providers.
        /// </summary>
        /// <remarks></remarks>
        IEnumerable<ObjectCache> Providers { get; }

        /// <summary>
        /// Gets the provider by unique name.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <returns>ObjectCache.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">providerName;There is no cache provider registered with name: providerName</exception>
        ObjectCache GetProvider(String providerName);

        /// <summary>
        /// Gets the provider by unique name.
        /// </summary>
        /// <typeparam name="TProvider">The type of the cache provider.</typeparam>
        /// <param name="providerName">Name of the provider.</param>
        /// <returns>ObjectCache.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">providerName;There is no cache provider registered with name: providerName</exception>
        /// <exception cref="System.InvalidOperationException">
        /// Occurs when the cache provider associated with <paramref name="providerName"/> 
        /// is not of type <typeparamref name="TProvider"/>
        /// </exception>
        TProvider GetProvider<TProvider>(String providerName) where TProvider : class;
    }
}