namespace NContext.Security
{
    using System;

    public struct SecurityTokenExpirationPolicy
    {
        private readonly Boolean _Expires;

        private readonly Boolean _IsAbsolute;

        private readonly TimeSpan _ExpirationTime;

        /// <summary>
        /// Creates a policy for handling 
        /// </summary>
        /// <param name="expirationTime"></param>
        /// <param name="expirationIsAbsolute"></param>
        public SecurityTokenExpirationPolicy(TimeSpan expirationTime, Boolean expirationIsAbsolute)
        {
            _Expires = true;
            _ExpirationTime = expirationTime;
            _IsAbsolute = expirationIsAbsolute;
        }

        /// <summary>
        /// Gets whether security token expires or not.
        /// </summary>
        public Boolean Expires
        {
            get
            {
                return _Expires;
            }
        }

        /// <summary>
        /// Gets whether the <see cref="ExpirationTime"/> is used for absolute expiration or sliding. Default is Sliding.
        /// </summary>
        public Boolean IsAbsolute
        {
            get
            {
                return _IsAbsolute;
            }
        }

        public TimeSpan ExpirationTime
        {
            get
            {
                return _ExpirationTime;
            }
        }
    }
}