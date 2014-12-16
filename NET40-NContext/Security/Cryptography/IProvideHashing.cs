namespace NContext.Security.Cryptography
{
    using System;
    using System.Security.Cryptography;

    /// <summary>
    /// Defines cryptographic operations for hashing.
    /// </summary>
    /// <remarks></remarks>
    public interface IProvideHashing
    {
        /// <summary>
        /// Creates the hash using the default <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltLength">The length of bytes to use as salt.</param>
        /// <returns>The created hash.</returns>
        /// <remarks></remarks>
        Byte[] CreateHash(Byte[] plainText, Int32 saltLength = 16);

        /// <summary>
        /// Creates the hash using the default <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltLength">The length of bytes to use as salt.</param>
        /// <returns>The created hash.</returns>
        /// <remarks></remarks>
        String CreateHash(String plainText, Int32 saltLength = 16);

        /// <summary>
        /// Creates the base64 encoded hash using the default <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltLength">The length of bytes to use as salt.</param>
        /// <returns>The created hash.</returns>
        /// <remarks></remarks>
        String CreateHashToBase64(String plainText, Int32 saltLength = 16);

        /// <summary>
        /// Creates the hash using the specified <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltLength">The length of bytes to use as salt.</param>
        /// <returns>The created hash.</returns>
        /// <remarks></remarks>
        Byte[] CreateHash<THashAlgorithm>(Byte[] plainText, Int32 saltLength = 16)
            where THashAlgorithm : HashAlgorithm, new();

        /// <summary>
        /// Creates the hash using the specified <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltLength">The length of bytes to use as salt.</param>
        /// <returns>The created hash.</returns>
        /// <remarks></remarks>
        String CreateHash<THashAlgorithm>(String plainText, Int32 saltLength = 16)
            where THashAlgorithm : HashAlgorithm, new();

        /// <summary>
        /// Creates the base64 encoded hash using the specified <typeparamref name="THashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltLength">The length of bytes to use as salt.</param>
        /// <returns>The created hash.</returns>
        /// <remarks></remarks>
        String CreateHashToBase64<THashAlgorithm>(String plainText, Int32 saltLength = 16)
            where THashAlgorithm : HashAlgorithm, new();

        /// <summary>
        /// Compares the hash using the default <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltLength">The length of bytes used for salt in the <paramref name="hashedText"/>.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        Boolean CompareHash(Byte[] plainText, Byte[] hashedText, Int32 saltLength = 16);

        /// <summary>
        /// Compares the hash using the default <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltLength">The length of bytes used for salt in the <paramref name="hashedText"/>.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        Boolean CompareHash(String plainText, String hashedText, Int32 saltLength = 16);

        /// <summary>
        /// Compares the hash using the default <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltLength">The length of bytes used for salt in the <paramref name="hashedText"/>.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        Boolean CompareHashFromBase64(String plainText, String hashedText, Int32 saltLength = 16);

        /// <summary>
        /// Compares the hash using the specified <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltLength">The length of bytes used for salt in the <paramref name="hashedText"/>.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        Boolean CompareHash<THashAlgorithm>(Byte[] plainText, Byte[] hashedText, Int32 saltLength = 16)
            where THashAlgorithm : HashAlgorithm, new();

        /// <summary>
        /// Compares the hash using the specified <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltLength">The length of bytes used for salt in the <paramref name="hashedText"/>.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        Boolean CompareHash<THashAlgorithm>(String plainText, String hashedText, Int32 saltLength = 16)
            where THashAlgorithm : HashAlgorithm, new();

        /// <summary>
        /// Compares the hash using the specified <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltLength">The length of bytes used for salt in the <paramref name="hashedText"/>.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        Boolean CompareHashFromBase64<THashAlgorithm>(String plainText, String hashedText, Int32 saltLength = 16)
            where THashAlgorithm : HashAlgorithm, new();
    }
}