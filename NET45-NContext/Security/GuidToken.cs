namespace NContext.Security
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IdentityModel.Tokens;

    using NContext.Common;

    /// <summary>
    /// Defines a generic <see cref="SecurityToken"/> using a <see cref="Guid"/>
    /// as a token identifier.
    /// </summary>
    public class GuidToken : SecurityToken, IToken
    {
        private readonly Guid _Id;

        private readonly DateTime _ValidFrom;

        private readonly DateTime _ValidTo;

        private readonly ReadOnlyCollection<SecurityKey> _SecurityKeys;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.IdentityModel.Tokens.SecurityToken"/> class.
        /// </summary>
        /// <remarks></remarks>
        public GuidToken()
            : this(Guid.NewGuid())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuidToken"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <remarks></remarks>
        public GuidToken(Guid id)
        {
            _Id = id;
            _ValidFrom = DateTime.UtcNow;
            _ValidTo = DateTimeOffset.MaxValue.UtcDateTime;
            _SecurityKeys = new ReadOnlyCollection<SecurityKey>(new List<SecurityKey>());
        }

        /// <summary>
        /// Gets a unique identifier of the security token.
        /// </summary>
        /// <returns>The unique identifier of the security token.</returns>
        /// <remarks></remarks>
        public override String Id
        {
            get
            {
                return _Id.ToString();
            }
        }

        /// <summary>
        /// Gets the cryptographic keys associated with the security token.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1"/> of type <see cref="T:System.IdentityModel.Tokens.SecurityKey"/> that contains the set of keys associated with the security token.</returns>
        /// <remarks></remarks>
        public override ReadOnlyCollection<SecurityKey> SecurityKeys
        {
            get
            {
                return _SecurityKeys;
            }
        }

        /// <summary>
        /// Gets the first instant in time at which this security token is valid.
        /// </summary>
        /// <returns>A <see cref="T:System.DateTime"/> that represents the instant in time at which this security token is first valid.</returns>
        /// <remarks></remarks>
        public override DateTime ValidFrom
        {
            get
            {
                return _ValidFrom;
            }
        }

        /// <summary>
        /// Gets the last instant in time at which this security token is valid.
        /// </summary>
        /// <returns>A <see cref="T:System.DateTime"/> that represents the last instant in time at which this security token is valid.</returns>
        /// <remarks></remarks>
        public override DateTime ValidTo
        {
            get
            {
                return _ValidTo;
            }
        }

        public String Value
        {
            get
            {
                return _Id.ToString();
            }
        }
    }
}