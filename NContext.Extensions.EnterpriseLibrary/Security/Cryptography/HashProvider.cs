using System;
using System.Security.Cryptography;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;

using NContext.Security.Cryptography;

namespace NContext.Extensions.EnterpriseLibrary.Security.Cryptography
{
    /// <summary>
    /// Defines a default implementation of <see cref="IProvideHashing"/>.
    /// </summary>
    /// <remarks></remarks>
    public class HashProvider : IProvideHashing
    {
        #region Fields

        private readonly Type _DefaultHashAlgorithm;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HashProvider"/> class.
        /// </summary>
        /// <param name="defaultHashAlgorithm">The default <see cref="HashAlgorithm"/>.</param>
        /// <remarks></remarks>
        public HashProvider(Type defaultHashAlgorithm)
        {
            if (!defaultHashAlgorithm.Implements<HashAlgorithm>())
            {
                throw new ArgumentException("DefaultHashAlgorithm is invalid. Must be of type HashAlgorithm.", "defaultHashAlgorithm");
            }

            _DefaultHashAlgorithm = defaultHashAlgorithm;
        }

        #endregion

        #region Hash CreateHash Methods

        /// <summary>
        /// Creates the hash using the specified <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns>The created hash.</returns>
        /// <remarks></remarks>
        public Byte[] CreateHash(Byte[] plainText, Boolean saltEnabled = true)
        {
            return CreateHash(_DefaultHashAlgorithm, plainText, saltEnabled);
        }

        /// <summary>
        /// Creates the hash using the specified <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns>The created hash.</returns>
        /// <remarks></remarks>
        public String CreateHash(String plainText, Boolean saltEnabled = true)
        {
            return Encoding.UTF8.GetString(CreateHash(_DefaultHashAlgorithm, Encoding.UTF8.GetBytes(plainText), saltEnabled));
        }

        /// <summary>
        /// Creates the hash using the specified <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns>The created hash.</returns>
        /// <remarks></remarks>
        public Byte[] CreateHash<THashAlgorithm>(Byte[] plainText, Boolean saltEnabled = true)
            where THashAlgorithm : HashAlgorithm
        {
            return CreateHash(typeof(THashAlgorithm), plainText, saltEnabled);
        }

        /// <summary>
        /// Creates the hash using the specified <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns>The created hash.</returns>
        /// <remarks></remarks>
        public String CreateHash<THashAlgorithm>(String plainText, Boolean saltEnabled = true)
            where THashAlgorithm : HashAlgorithm
        {
            return Encoding.UTF8.GetString(CreateHash(typeof(THashAlgorithm), Encoding.UTF8.GetBytes(plainText), saltEnabled));
        }

        private Byte[] CreateHash(Type keyedHashAlgorithm, Byte[] plainText, Boolean saltEnabled = true)
        {
            var hashAlgorithmProvider =
                new HashAlgorithmProvider(keyedHashAlgorithm, saltEnabled);

            return hashAlgorithmProvider.CreateHash(plainText);
        }

        #endregion

        #region Hash CompareHash Methods

        /// <summary>
        /// Compares the hashes.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        public Boolean CompareHash(Byte[] plainText, Byte[] hashedText, Boolean saltEnabled = true)
        {
            return CompareHash(_DefaultHashAlgorithm, plainText, hashedText, saltEnabled);
        }

        /// <summary>
        /// Compares the hash.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        public Boolean CompareHash(String plainText, String hashedText, Boolean saltEnabled = true)
        {
            return CompareHash(_DefaultHashAlgorithm, Encoding.UTF8.GetBytes(plainText), Encoding.UTF8.GetBytes(hashedText));
        }

        /// <summary>
        /// Compares the hash.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        public Boolean CompareHash<THashAlgorithm>(Byte[] plainText, Byte[] hashedText, Boolean saltEnabled = true)
            where THashAlgorithm : HashAlgorithm
        {
            return CompareHash(typeof(THashAlgorithm), plainText, hashedText, saltEnabled);
        }

        /// <summary>
        /// Compares the hash.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        public Boolean CompareHash<THashAlgorithm>(String plainText, String hashedText, Boolean saltEnabled = true)
            where THashAlgorithm : HashAlgorithm
        {
            return CompareHash(typeof(THashAlgorithm), Encoding.UTF8.GetBytes(plainText), Encoding.UTF8.GetBytes(hashedText));
        }

        private Boolean CompareHash(Type hashAlgorithm, Byte[] plainText, Byte[] hashedText, Boolean saltEnabled = true)
        {
            var hashAlgorithmProvider =
                new HashAlgorithmProvider(hashAlgorithm, saltEnabled);

            return hashAlgorithmProvider.CompareHash(plainText, hashedText);
        }

        #endregion
    }
}