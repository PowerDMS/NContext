namespace NContext.Security.Cryptography
{
    using System;
    using System.Security.Cryptography;

    /// <summary>
    /// Defines configuration settings for application cryptographic operations.
    /// </summary>
    public class CryptographyConfiguration
    {
        private readonly Type _DefaultHashAlgorithm;

        private readonly Type _DefaultKeyedHashAlgorithm;

        private readonly Type _DefaultSymmetricAlgorithm;

        private readonly Func<IProvideHashing> _HashProviderFactory;

        private readonly Func<IProvideKeyedHashing> _KeyedHashProviderFactory;

        private readonly Func<IProvideSymmetricEncryption> _SymmetricEncryptionProviderFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptographyConfiguration"/> class.
        /// </summary>
        /// <param name="defaultHashAlgorithm">The default hash algorithm.</param>
        /// <param name="defaultKeyedHashAlgorithm">The default keyed hash algorithm.</param>
        /// <param name="defaultSymmetricAlgorithm">The default symmetric algorithm.</param>
        /// <param name="hashProviderFactory">The hash provider factory.</param>
        /// <param name="keyedHashProviderFactory">The keyed hash provider factory.</param>
        /// <param name="symmetricEncryptionProviderFactory">The symmetric encryption provider factory.</param>
        /// <remarks></remarks>
        public CryptographyConfiguration(Type defaultHashAlgorithm, Type defaultKeyedHashAlgorithm, Type defaultSymmetricAlgorithm, Func<IProvideHashing> hashProviderFactory, Func<IProvideKeyedHashing> keyedHashProviderFactory, Func<IProvideSymmetricEncryption> symmetricEncryptionProviderFactory)
        {
            _DefaultHashAlgorithm = defaultHashAlgorithm;
            _DefaultKeyedHashAlgorithm = defaultKeyedHashAlgorithm;
            _DefaultSymmetricAlgorithm = defaultSymmetricAlgorithm;
            _HashProviderFactory = hashProviderFactory;
            _KeyedHashProviderFactory = keyedHashProviderFactory;
            _SymmetricEncryptionProviderFactory = symmetricEncryptionProviderFactory;
        }

        /// <summary>
        /// Gets the symmetric encryption provider factory.
        /// </summary>
        /// <remarks></remarks>
        public Func<IProvideSymmetricEncryption> SymmetricEncryptionProviderFactory
        {
            get
            {
                return _SymmetricEncryptionProviderFactory;
            }
        }

        /// <summary>
        /// Gets the keyed hash provider factory.
        /// </summary>
        /// <remarks></remarks>
        public Func<IProvideKeyedHashing> KeyedHashProviderFactory
        {
            get
            {
                return _KeyedHashProviderFactory;
            }
        }

        /// <summary>
        /// Gets the hash provider factory.
        /// </summary>
        /// <remarks></remarks>
        public Func<IProvideHashing> HashProviderFactory
        {
            get
            {
                return _HashProviderFactory;
            }
        }

        /// <summary>
        /// Gets the default <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <remarks></remarks>
        public Type DefaultHashAlgorithm
        {
            get
            {
                return _DefaultHashAlgorithm;
            }
        }

        /// <summary>
        /// Gets the default <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <remarks></remarks>
        public Type DefaultKeyedHashAlgorithm
        {
            get
            {
                return _DefaultKeyedHashAlgorithm;
            }
        }

        /// <summary>
        /// Gets the default <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        /// <remarks></remarks>
        public Type DefaultSymmetricAlgorithm
        {
            get
            {
                return _DefaultSymmetricAlgorithm;
            }
        }
    }
}