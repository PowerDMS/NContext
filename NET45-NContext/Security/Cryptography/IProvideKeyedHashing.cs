namespace NContext.Security.Cryptography
{
    using System;
    using System.Security.Cryptography;

    /// <summary>
    /// Defines cryptographic operations for keyed hashing.
    /// </summary>
    /// <remarks></remarks>
    public interface IProvideKeyedHashing
    {
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
        Byte[] CreateHash(Byte[] symmetricKey, Byte[] plainText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

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
        String CreateHash(String symmetricKey, String plainText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

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
        String CreateHashToBase64(String symmetricKey, String plainText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

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
        Byte[] CreateHash<TKeyedHashAlgorithm>(Byte[] symmetricKey, Byte[] plainText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm;

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
        String CreateHash<TKeyedHashAlgorithm>(String symmetricKey, String plainText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm;

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
        String CreateHashToBase64<TKeyedHashAlgorithm>(String symmetricKey, String plainText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm;

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
        Boolean CompareHash(Byte[] symmetricKey, Byte[] plainText, Byte[] hashedText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

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
        Boolean CompareHash(String symmetricKey, String plainText, String hexadecimalHash, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);
        
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
        Boolean CompareHashFromBase64(String symmetricKey, String plainText, String base64EncodedHash, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

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
        Boolean CompareHash<TKeyedHashAlgorithm>(Byte[] symmetricKey, Byte[] plainText, Byte[] hashedText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm;

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
        Boolean CompareHash<TKeyedHashAlgorithm>(String symmetricKey, String plainText, String hexadecimalHash, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm;

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
        Boolean CompareHashFromBase64<TKeyedHashAlgorithm>(String symmetricKey, String plainText, String base64EncodedHash, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm;

        #endregion
    }
}