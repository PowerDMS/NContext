namespace NContext.Security.Cryptography
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    using NContext.Extensions;
    using NContext.Utilities;

    /// <summary>
    /// Defines a provider for keyed hash cryptographic operations.
    /// </summary>
    /// <remarks></remarks>
    public class KeyedHashProvider : IProvideKeyedHashing
    {
        private readonly Type _DefaultKeyedHashAlgorithm;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedHashProvider"/> class.
        /// </summary>
        /// <param name="defaultKeyedHashAlgorithm">The default <see cref="KeyedHashAlgorithm"/>.</param>
        /// <remarks></remarks>
        public KeyedHashProvider(Type defaultKeyedHashAlgorithm)
        {
            if (defaultKeyedHashAlgorithm == null)
            {
                throw new ArgumentNullException("defaultKeyedHashAlgorithm");
            }

            if (!defaultKeyedHashAlgorithm.Implements<KeyedHashAlgorithm>())
            {
                throw new ArgumentException("DefaultKeyedHashAlgorithm is invalid. Must be of type KeyedHashAlgorithm.", "defaultKeyedHashAlgorithm");
            }

            _DefaultKeyedHashAlgorithm = defaultKeyedHashAlgorithm;
        }

        #region Keyed-Hash CreateHash Methods

        /// <summary>
        /// Creates the <see cref="HMAC"/> hash with the specified symmetric key 
        /// using the default <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The hashed message authentication code. (<see cref="HMAC"/>)</returns>
        /// <remarks></remarks>
        public Byte[] CreateHash(Byte[] symmetricKey, Byte[] plainText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return CreateHash(_DefaultKeyedHashAlgorithm, symmetricKey, plainText, saltEnabled, dataProtectionScope);
        }

        /// <summary>
        /// Creates the <see cref="HMAC"/> hash with the specified symmetric key using the default <see cref="KeyedHashAlgorithm"/>.
        /// Returns the hash as hexadecimal.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>Hexadecimal representation of the hashed message authentication code. (<see cref="HMAC"/>)</returns>
        /// <remarks></remarks>
        public String CreateHash(String symmetricKey, String plainText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return CreateHash(_DefaultKeyedHashAlgorithm, symmetricKey.ToBytes<UnicodeEncoding>(), plainText.ToBytes<UnicodeEncoding>(), saltEnabled, dataProtectionScope).ToHexadecimal();
        }

        /// <summary>
        /// Creates the <see cref="HMAC"/> hash with the specified symmetric key 
        /// using the default <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The hashed message authentication code. (<see cref="HMAC"/>)</returns>
        /// <remarks></remarks>
        public String CreateHashToBase64(String symmetricKey, String plainText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return CreateHash(_DefaultKeyedHashAlgorithm, symmetricKey.ToBytes<UnicodeEncoding>(), plainText.ToBytes<UnicodeEncoding>(), saltEnabled, dataProtectionScope).ToBase64();
        }

        /// <summary>
        /// Creates the <see cref="HMAC"/> hash with the specified symmetric key 
        /// and <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TKeyedHashAlgorithm">The type of the keyed hash algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The hashed message authentication code. (<see cref="HMAC"/>)</returns>
        /// <remarks></remarks>
        public Byte[] CreateHash<TKeyedHashAlgorithm>(Byte[] symmetricKey, Byte[] plainText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm
        {
            return CreateHash(typeof(TKeyedHashAlgorithm), symmetricKey, plainText, saltEnabled, dataProtectionScope);
        }

        /// <summary>
        /// Creates the <see cref="HMAC"/> hash with the specified symmetric key and <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TKeyedHashAlgorithm">The type of the keyed hash algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>Hexadecimal representation of the hashed message authentication code. (<see cref="HMAC"/>)</returns>
        /// <remarks></remarks>
        public String CreateHash<TKeyedHashAlgorithm>(String symmetricKey, String plainText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm
        {
            return CreateHash(_DefaultKeyedHashAlgorithm, symmetricKey.ToBytes<UnicodeEncoding>(), plainText.ToBytes<UnicodeEncoding>(), saltEnabled, dataProtectionScope).ToHexadecimal();
        }

        /// <summary>
        /// Creates the <see cref="HMAC"/> hash with the specified symmetric key 
        /// and <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TKeyedHashAlgorithm">The type of the keyed hash algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The hashed message authentication code. (<see cref="HMAC"/>)</returns>
        /// <remarks></remarks>
        public String CreateHashToBase64<TKeyedHashAlgorithm>(String symmetricKey, String plainText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm
        {
            return CreateHash(typeof(TKeyedHashAlgorithm), symmetricKey.ToBytes<UnicodeEncoding>(), plainText.ToBytes<UnicodeEncoding>(), saltEnabled, dataProtectionScope).ToBase64();
        }

        private Byte[] CreateHash(Type keyedHashAlgorithmType, Byte[] symmetricKey, Byte[] plainText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            var keyedHashAlgorithm = Activator.CreateInstance(keyedHashAlgorithmType, true) as KeyedHashAlgorithm;
            if (keyedHashAlgorithm == null)
            {
                throw new InvalidOperationException(String.Format("Could not create instance of type {0}.", keyedHashAlgorithmType));
            }

            keyedHashAlgorithm.Key = symmetricKey;

            // TODO: (DG) Support SALT!
            return keyedHashAlgorithm.ComputeHash(plainText);
        }

        #endregion

        #region Keyed-Hash CompareHash Methods

        /// <summary>
        /// Compares the hashes using the specified symmetric key and default <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        public Boolean CompareHash(Byte[] symmetricKey, Byte[] plainText, Byte[] hashedText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return CompareHash(_DefaultKeyedHashAlgorithm,
                symmetricKey,
                plainText,
                hashedText,
                saltEnabled,
                dataProtectionScope);
        }

        /// <summary>
        /// Compares the hashes using the specified symmetric key and default <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hexadecimalHash">The hashed text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        public Boolean CompareHash(String symmetricKey, String plainText, String hexadecimalHash, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return CompareHash(_DefaultKeyedHashAlgorithm,
                symmetricKey.ToBytes<UnicodeEncoding>(),
                plainText.ToBytes<UnicodeEncoding>(),
                hexadecimalHash.ToBytesFromHexadecimal(),
                saltEnabled,
                dataProtectionScope);
        }

        /// <summary>
        /// Compares the hashes using the specified symmetric key and default <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="base64EncodedHash">The hashed text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        public Boolean CompareHashFromBase64(String symmetricKey, String plainText, String base64EncodedHash, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return CompareHash(_DefaultKeyedHashAlgorithm,
                symmetricKey.ToBytes<UnicodeEncoding>(),
                plainText.ToBytes<UnicodeEncoding>(),
                base64EncodedHash.ToBytesFromBase64(),
                saltEnabled,
                dataProtectionScope);
        }

        /// <summary>
        /// Compares the hashes using the specified symmetric key and <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TKeyedHashAlgorithm">The type of the keyed hash algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        public Boolean CompareHash<TKeyedHashAlgorithm>(Byte[] symmetricKey, Byte[] plainText, Byte[] hashedText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm
        {
            return CompareHash(typeof(TKeyedHashAlgorithm),
                symmetricKey,
                plainText,
                hashedText,
                saltEnabled,
                dataProtectionScope);
        }

        /// <summary>
        /// Compares the hashes using the specified symmetric key and default <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hexadecimalHash">The hashed text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        public Boolean CompareHash<TKeyedHashAlgorithm>(String symmetricKey, String plainText, String hexadecimalHash, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm
        {
            return CompareHash(typeof(TKeyedHashAlgorithm),
                symmetricKey.ToBytes<UnicodeEncoding>(),
                plainText.ToBytes<UnicodeEncoding>(),
                hexadecimalHash.ToBytesFromHexadecimal(),
                saltEnabled,
                dataProtectionScope);
        }

        /// <summary>
        /// Compares the hashes using the specified symmetric key and <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TKeyedHashAlgorithm">The type of the keyed hash algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="base64EncodedHash">The hashed text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        public Boolean CompareHashFromBase64<TKeyedHashAlgorithm>(String symmetricKey, String plainText, String base64EncodedHash, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm
        {
            return CompareHash(typeof(TKeyedHashAlgorithm),
                symmetricKey.ToBytes<UnicodeEncoding>(),
                plainText.ToBytes<UnicodeEncoding>(),
                base64EncodedHash.ToBytesFromBase64(),
                saltEnabled,
                dataProtectionScope);
        }
        
        private Boolean CompareHash(Type keyedHashAlgorithmType, Byte[] symmetricKey, Byte[] plainText, Byte[] hashedText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            if (symmetricKey == null || plainText == null || hashedText == null)
            {
                return false;
            }

            var keyedHashAlgorithm = Activator.CreateInstance(keyedHashAlgorithmType, true) as KeyedHashAlgorithm;
            if (keyedHashAlgorithm == null)
            {
                throw new InvalidOperationException(String.Format("Could not create instance of type {0}.", keyedHashAlgorithmType));
            }

            keyedHashAlgorithm.Key = symmetricKey;
            var hashedPlainText = keyedHashAlgorithm.ComputeHash(plainText);

            // TODO: (DG) Support SALT!
            return CryptographyUtility.CompareBytes(hashedPlainText, hashedText);
        }

        #endregion
    }
}