namespace NContext.Security.Cryptography
{
    using System;
    using System.Security.Cryptography;

    using NContext.Configuration;

    /// <summary>
    /// Defines an interface for application-wide cryptographic management.
    /// </summary>
    /// <remarks></remarks>
    public interface IManageCryptography : IApplicationComponent
    {
        /// <summary>
        /// Gets the hash provider.
        /// </summary>
        /// <remarks></remarks>
        IProvideHashing HashProvider { get; }

        /// <summary>
        /// Gets the keyed hash provider.
        /// </summary>
        /// <remarks></remarks>
        IProvideKeyedHashing KeyedHashProvider { get; }

        /// <summary>
        /// Gets the symmetric encryption provider.
        /// </summary>
        /// <remarks></remarks>
        IProvideSymmetricEncryption SymmetricEncryptionProvider { get; }

        /// <summary>
        /// Gets or sets the default symmetric algorithm. Default is <see cref="AesManaged"/>.
        /// </summary>
        /// <value>The default symmetric algorithm.</value>
        /// <remarks></remarks>
        Type DefaultSymmetricAlgorithm { get; set; }

        /// <summary>
        /// Gets or sets the default keyed hash algorithm. Default is <see cref="HMACSHA256"/>.
        /// </summary>
        /// <value>The default keyed hash algorithm.</value>
        /// <remarks></remarks>
        Type DefaultKeyedHashAlgorithm { get; set; }

        /// <summary>
        /// Gets the default hash algorithm. Default is <see cref="SHA256Managed"/>.
        /// </summary>
        /// <remarks></remarks>
        Type DefaultHashAlgorithm { get; set; }
    }
}