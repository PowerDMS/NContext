namespace NContext.Security.Cryptography
{
    using System;
    using System.Security.Cryptography;

    using NContext.Configuration;

    /// <summary>
    /// Defines a configuration class to build the application's <see cref="CryptographyManager"/>.
    /// </summary>
    public class CryptographyManagerBuilder : ApplicationComponentConfigurationBuilderBase
    {
        private Type _DefaultHashAlgorithm;

        private Type _DefaultKeyedHashAlgorithm;

        private Type _DefaultSymmetricAlgorithm;

        private Func<IProvideHashing> _HashProviderFactory;

        private Func<IProvideKeyedHashing> _KeyedHashProviderFactory;

        private Func<IProvideSymmetricEncryption> _SymmetricEncryptionProviderFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptographyManagerBuilder"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        /// <remarks></remarks>
        public CryptographyManagerBuilder(ApplicationConfigurationBuilder applicationConfigurationBuilder)
            : base(applicationConfigurationBuilder)
        {
        }

        /// <summary>
        /// Sets the application's default cryptographic algorithms. (<see cref="HashAlgorithm" />, <see cref="KeyedHashAlgorithm" />, <see cref="SymmetricAlgorithm" />)
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of hash algorithm.</typeparam>
        /// <typeparam name="TKeyedHashAlgorithm">The type of keyed hash algorithm.</typeparam>
        /// <typeparam name="TSymmetricAlgorithm">The type of symmetric algorithm.</typeparam>
        /// <returns>Current CryptographyManagerBuilder instance.</returns>
        public CryptographyManagerBuilder SetDefaults<THashAlgorithm, TKeyedHashAlgorithm, TSymmetricAlgorithm>()
            where THashAlgorithm : HashAlgorithm
            where TKeyedHashAlgorithm : KeyedHashAlgorithm
            where TSymmetricAlgorithm : SymmetricAlgorithm
        {
            _DefaultHashAlgorithm = typeof(THashAlgorithm);
            _DefaultKeyedHashAlgorithm = typeof(TKeyedHashAlgorithm);
            _DefaultSymmetricAlgorithm = typeof(TSymmetricAlgorithm);

            return this;
        }

        /// <summary>
        /// Sets the application's hash provider.
        /// </summary>
        /// <param name="hashProviderFactory">The hash provider factory.</param>
        /// <returns>Current <see cref="CryptographyManagerBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public CryptographyManagerBuilder SetHashProvider(Func<IProvideHashing> hashProviderFactory)
        {
            _HashProviderFactory = hashProviderFactory;

            return this;
        }

        /// <summary>
        /// Sets the application's keyed hash provider.
        /// </summary>
        /// <param name="keyedHashProviderFactory">The keyed hash provider factory.</param>
        /// <returns>Current <see cref="CryptographyManagerBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public CryptographyManagerBuilder SetKeyedHashProvider(Func<IProvideKeyedHashing> keyedHashProviderFactory)
        {
            _KeyedHashProviderFactory = keyedHashProviderFactory;

            return this;
        }

        /// <summary>
        /// Sets the application's symmetric encryption provider.
        /// </summary>
        /// <param name="symmetricEncryptionProvider">The symmetric encryption provider.</param>
        /// <returns>Current <see cref="CryptographyManagerBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public CryptographyManagerBuilder SetSymmetricEncryptionProvider(Func<IProvideSymmetricEncryption> symmetricEncryptionProvider)
        {
            _SymmetricEncryptionProviderFactory = symmetricEncryptionProvider;

            return this;
        }

        /// <summary>
        /// Sets the application's cryptography manager using 
        /// the configuration created from this instance.
        /// </summary>
        /// <remarks></remarks>
        protected override void Setup()
        {
            Builder.ApplicationConfiguration
                   .RegisterComponent<IManageCryptography>(
                   () => 
                       new CryptographyManager(
                           new CryptographyConfiguration(
                               _DefaultHashAlgorithm,
                               _DefaultKeyedHashAlgorithm,
                               _DefaultSymmetricAlgorithm,
                               _HashProviderFactory,
                               _KeyedHashProviderFactory,
                               _SymmetricEncryptionProviderFactory)));
        }
    }
}