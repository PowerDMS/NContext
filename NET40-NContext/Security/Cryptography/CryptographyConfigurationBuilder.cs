// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CryptographyConfigurationBuilder.cs" company="Waking Venture, Inc.">
//   Copyright (c) 2012 Waking Venture, Inc.
//
//   Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//   documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
//   the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
//   and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
//   The above copyright notice and this permission notice shall be included in all copies or substantial portions 
//   of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//   TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//   CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//   DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.Security.Cryptography
{
    using System;
    using System.Security.Cryptography;

    using NContext.Configuration;

    /// <summary>
    /// Defines a configuration class to build the application's <see cref="CryptographyManager"/>.
    /// </summary>
    public class CryptographyConfigurationBuilder : ApplicationComponentConfigurationBuilderBase
    {
        private Type _DefaultHashAlgorithm;

        private Type _DefaultKeyedHashAlgorithm;

        private Type _DefaultSymmetricAlgorithm;

        private Func<IProvideHashing> _HashProviderFactory;

        private Func<IProvideKeyedHashing> _KeyedHashProviderFactory;

        private Func<IProvideSymmetricEncryption> _SymmetricEncryptionProviderFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptographyConfigurationBuilder"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        /// <remarks></remarks>
        public CryptographyConfigurationBuilder(ApplicationConfigurationBuilder applicationConfigurationBuilder)
            : base(applicationConfigurationBuilder)
        {
        }

        /// <summary>
        /// Sets the application's default cryptographic algorithms. (<see cref="HashAlgorithm" />, <see cref="KeyedHashAlgorithm" />, <see cref="SymmetricAlgorithm" />)
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of hash algorithm.</typeparam>
        /// <typeparam name="TKeyedHashAlgorithm">The type of keyed hash algorithm.</typeparam>
        /// <typeparam name="TSymmetricAlgorithm">The type of symmetric algorithm.</typeparam>
        /// <returns>Current CryptographyConfigurationBuilder instance.</returns>
        public CryptographyConfigurationBuilder SetDefaults<THashAlgorithm, TKeyedHashAlgorithm, TSymmetricAlgorithm>()
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
        /// <returns>Current <see cref="CryptographyConfigurationBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public CryptographyConfigurationBuilder SetHashProvider(Func<IProvideHashing> hashProviderFactory)
        {
            _HashProviderFactory = hashProviderFactory;

            return this;
        }

        /// <summary>
        /// Sets the application's keyed hash provider.
        /// </summary>
        /// <param name="keyedHashProviderFactory">The keyed hash provider factory.</param>
        /// <returns>Current <see cref="CryptographyConfigurationBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public CryptographyConfigurationBuilder SetKeyedHashProvider(Func<IProvideKeyedHashing> keyedHashProviderFactory)
        {
            _KeyedHashProviderFactory = keyedHashProviderFactory;

            return this;
        }

        /// <summary>
        /// Sets the application's symmetric encryption provider.
        /// </summary>
        /// <param name="symmetricEncryptionProvider">The symmetric encryption provider.</param>
        /// <returns>Current <see cref="CryptographyConfigurationBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public CryptographyConfigurationBuilder SetSymmetricEncryptionProvider(Func<IProvideSymmetricEncryption> symmetricEncryptionProvider)
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