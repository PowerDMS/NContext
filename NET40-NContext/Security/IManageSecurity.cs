namespace NContext.Security
{
    using System.IdentityModel.Tokens;
    using System.Security.Principal;

    using NContext.Caching;
    using NContext.Common;
    using NContext.Configuration;

    /// <summary>
    /// Defines interface contract which encapsulates logic for application security-related operations.
    /// </summary>
    /// <remarks></remarks>
    public interface IManageSecurity : IApplicationComponent
    {
        /// <summary>
        /// Saves the principal to cache using the application's <see cref="IManageCaching"/>.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>A token used to retrieve the <see cref="IPrincipal"/> from cache.</returns>
        /// <remarks></remarks>
        IToken SavePrincipal(IPrincipal principal);

        /// <summary>
        /// Saves the principal to cache using the application's <see cref="IManageCaching"/>.
        /// </summary>
        /// <typeparam name="TToken">The type of the token.</typeparam>
        /// <param name="principal">The principal.</param>
        /// <returns>A <typeparamref name="TToken"/> token used to retrieve the <see cref="IPrincipal"/> from cache.</returns>
        /// <remarks></remarks>
        TToken SavePrincipal<TToken>(IPrincipal principal) where TToken : class, IToken, new();

        /// <summary>
        /// Updates the cached principal associated with the specified <see cref="SecurityToken"/>, using the application's <see cref="IManageCaching"/>.
        /// </summary>
        /// <param name="principal">The cached <see cref="IPrincipal"/> instance.</param>
        /// <param name="token">The <see cref="SecurityToken"/> to associate with <paramref name="principal"/>.</param>
        /// <remarks></remarks>
        void SavePrincipal(IPrincipal principal, IToken token);

        /// <summary>
        /// Expires the principal associated with the specified <see cref="SecurityToken"/>.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <remarks></remarks>
        void ExpirePrincipal(IToken token);

        /// <summary>
        /// Gets the cached <see cref="IPrincipal"/> instance associated with the specified <see cref="SecurityToken"/>.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>Instance of <see cref="IPrincipal"/> if found in cache, else null.</returns>
        /// <remarks></remarks>
        IPrincipal GetPrincipal(IToken token);

        /// <summary>
        /// Gets the cached <typeparamref name="TPrincipal"/> instance associated with the specified <see cref="SecurityToken"/>.
        /// </summary>
        /// <typeparam name="TPrincipal">The type of the principal.</typeparam>
        /// <param name="token">The token.</param>
        /// <returns>Instance of <typeparamref name="TPrincipal"/> if found in cache, else null.</returns>
        /// <remarks></remarks>
        TPrincipal GetPrincipal<TPrincipal>(IToken token) where TPrincipal : class, IPrincipal;
    }
}