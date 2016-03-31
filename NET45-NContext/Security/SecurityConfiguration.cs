namespace NContext.Security
{
    using System;

    /// <summary>
    /// Defines application security configuration settings.
    /// </summary>
    public class SecurityConfiguration
    {
        private readonly SecurityTokenExpirationPolicy _ExpirationPolicy;

        public SecurityConfiguration(SecurityTokenExpirationPolicy expirationPolicy)
        {
            _ExpirationPolicy = expirationPolicy;
        }

        public SecurityTokenExpirationPolicy TokenExpirationPolicy
        {
            get
            {
                return _ExpirationPolicy;
            }
        }
    }
}