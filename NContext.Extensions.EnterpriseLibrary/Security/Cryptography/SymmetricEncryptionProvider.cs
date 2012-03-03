using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;

using NContext.Security.Cryptography;

namespace NContext.Extensions.EnterpriseLibrary.Security.Cryptography
{
    /// <summary>
    /// Defines a default implementation of <see cref="IProvideSymmetricEncryption"/>.
    /// </summary>
    /// <remarks></remarks>
    public class SymmetricEncryptionProvider : IProvideSymmetricEncryption
    {
        #region Fields

        private readonly Type _DefaultSymmetricAlgorithm;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SymmetricEncryptionProvider"/> class.
        /// </summary>
        /// <param name="defaultSymmetricAlgorithm">The default symmetric algorithm.</param>
        /// <remarks></remarks>
        public SymmetricEncryptionProvider(Type defaultSymmetricAlgorithm)
        {
            if (!defaultSymmetricAlgorithm.Implements<SymmetricAlgorithm>())
            {
                throw new ArgumentException("DefaultSymmetricAlgorithm is invalid. Must be of type SymmetricAlgorithm.", "defaultSymmetricAlgorithm");
            }

            _DefaultSymmetricAlgorithm = defaultSymmetricAlgorithm;
        }

        #endregion

        #region Symmetric Encryption Methods

        /// <summary>
        /// Encrypts the specified text using the application's
        /// default <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted byte array.</returns>
        /// <remarks></remarks>
        public Byte[] Encrypt(Byte[] symmetricKey, Byte[] plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encrypt(_DefaultSymmetricAlgorithm, symmetricKey, plainText, dataProtectionScope);
        }

        /// <summary>
        /// Encrypts the specified text using the application's
        /// default <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted text.</returns>
        /// <remarks></remarks>
        public String EncryptToBase64String(String symmetricKey, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Convert.ToBase64String(Encrypt(_DefaultSymmetricAlgorithm, Encoding.UTF8.GetBytes(symmetricKey), Encoding.UTF8.GetBytes(plainText), dataProtectionScope));
        }

        /// <summary>
        /// Encrypts the specified text using the application's
        /// default <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted byte array.</returns>
        /// <remarks></remarks>
        public Byte[] Encrypt(FileInfo protectedKeyFile, Byte[] plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encrypt(_DefaultSymmetricAlgorithm, protectedKeyFile.FullName, plainText, dataProtectionScope);
        }

        /// <summary>
        /// Encrypts the specified text using the application's
        /// default <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted text.</returns>
        /// <remarks></remarks>
        public String EncryptToBase64String(FileInfo protectedKeyFile, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Convert.ToBase64String(Encrypt(_DefaultSymmetricAlgorithm, protectedKeyFile.FullName, Encoding.UTF8.GetBytes(plainText), dataProtectionScope));
        }

        /// <summary>
        /// Encrypts the text with the specified <typeparamref name="TSymmetricAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted byte array.</returns>
        /// <remarks></remarks>
        public Byte[] Encrypt<TSymmetricAlgorithm>(Byte[] symmetricKey, Byte[] plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encrypt(typeof(TSymmetricAlgorithm), symmetricKey, plainText, dataProtectionScope);
        }

        /// <summary>
        /// Encrypts the text with the specified <typeparamref name="TSymmetricAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted text.</returns>
        /// <remarks></remarks>
        public String EncryptToBase64String<TSymmetricAlgorithm>(String symmetricKey, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Convert.ToBase64String(Encrypt(typeof(TSymmetricAlgorithm), Encoding.UTF8.GetBytes(symmetricKey), Encoding.UTF8.GetBytes(plainText), dataProtectionScope));
        }

        /// <summary>
        /// Encrypts the specified text.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted byte array.</returns>
        /// <remarks></remarks>
        public Byte[] Encrypt<TSymmetricAlgorithm>(FileInfo protectedKeyFile, Byte[] plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encrypt(typeof(TSymmetricAlgorithm), protectedKeyFile.FullName, plainText, dataProtectionScope);
        }

        /// <summary>
        /// Encrypts the text with the specified <typeparamref name="TSymmetricAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted text.</returns>
        /// <remarks></remarks>
        public String EncryptToBase64String<TSymmetricAlgorithm>(FileInfo protectedKeyFile, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Convert.ToBase64String(Encrypt(typeof(TSymmetricAlgorithm), protectedKeyFile.FullName, Encoding.UTF8.GetBytes(plainText), dataProtectionScope));
        }

        /// <summary>
        /// Encrypts the specified symmetric algorithm.
        /// </summary>
        /// <param name="symmetricAlgorithm">The symmetric algorithm.</param>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plaintext">The plaintext.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private Byte[] Encrypt(Type symmetricAlgorithm, Byte[] symmetricKey, Byte[] plaintext, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encrypt(symmetricAlgorithm, ProtectedKey.CreateFromPlaintextKey(symmetricKey, dataProtectionScope), plaintext);
        }

        /// <summary>
        /// Encrypts the specified symmetric algorithm.
        /// </summary>
        /// <param name="symmetricAlgorithm">The symmetric algorithm.</param>
        /// <param name="protectedKeyFileName">Name of the protected key file.</param>
        /// <param name="plaintext">The plaintext.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private Byte[] Encrypt(Type symmetricAlgorithm, String protectedKeyFileName, Byte[] plaintext, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encrypt(symmetricAlgorithm, KeyManager.Read(protectedKeyFileName, dataProtectionScope), plaintext);
        }

        /// <summary>
        /// Encrypts the specified symmetric algorithm.
        /// </summary>
        /// <param name="symmetricAlgorithm">The symmetric algorithm.</param>
        /// <param name="protectedKey">The protected key.</param>
        /// <param name="plaintext">The plaintext.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private Byte[] Encrypt(Type symmetricAlgorithm, ProtectedKey protectedKey, Byte[] plaintext)
        {
            var symmetricProvider = new SymmetricAlgorithmProvider(
                symmetricAlgorithm, protectedKey);

            return symmetricProvider.Encrypt(plaintext);
        }

        #endregion

        #region Symmetric Decryption Methods

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted byte array.</returns>
        /// <remarks></remarks>
        public Byte[] Decrypt(Byte[] symmetricKey, Byte[] cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Decrypt(_DefaultSymmetricAlgorithm, symmetricKey, cipherText, dataProtectionScope);
        }

        /// <summary>
        /// Decrypts the specified symmetric key.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted text.</returns>
        /// <remarks></remarks>
        public String DecryptFromBase64String(String symmetricKey, String cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encoding.UTF8.GetString(Decrypt(_DefaultSymmetricAlgorithm, Encoding.UTF8.GetBytes(symmetricKey), Convert.FromBase64String(cipherText), dataProtectionScope));
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted byte array.</returns>
        /// <remarks></remarks>
        public Byte[] Decrypt(FileInfo protectedKeyFile, Byte[] cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Decrypt(_DefaultSymmetricAlgorithm, protectedKeyFile.FullName, cipherText, dataProtectionScope);
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted text.</returns>
        /// <remarks></remarks>
        public String DecryptFromBase64String(FileInfo protectedKeyFile, String cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encoding.UTF8.GetString(Decrypt(_DefaultSymmetricAlgorithm, protectedKeyFile.FullName, Convert.FromBase64String(cipherText), dataProtectionScope));
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted byte array.</returns>
        /// <remarks></remarks>
        public Byte[] Decrypt<TSymmetricAlgorithm>(Byte[] symmetricKey, Byte[] cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Decrypt(typeof(TSymmetricAlgorithm), symmetricKey, cipherText, dataProtectionScope);
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted text.</returns>
        /// <remarks></remarks>
        public String DecryptFromBase64String<TSymmetricAlgorithm>(String symmetricKey, String cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encoding.UTF8.GetString(Decrypt(typeof(TSymmetricAlgorithm), Encoding.UTF8.GetBytes(symmetricKey), Convert.FromBase64String(cipherText), dataProtectionScope));
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted byte array.</returns>
        /// <remarks></remarks>
        public Byte[] Decrypt<TSymmetricAlgorithm>(FileInfo protectedKeyFile, Byte[] cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Decrypt(typeof(TSymmetricAlgorithm), protectedKeyFile.FullName, cipherText, dataProtectionScope);
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted text.</returns>
        /// <remarks></remarks>
        public String DecryptFromBase64String<TSymmetricAlgorithm>(FileInfo protectedKeyFile, String cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encoding.UTF8.GetString(Decrypt(typeof(TSymmetricAlgorithm), protectedKeyFile.FullName, Convert.FromBase64String(cipherText), dataProtectionScope));
        }

        /// <summary>
        /// Decrypts the specified symmetric algorithm.
        /// </summary>
        /// <param name="symmetricAlgorithm">The symmetric algorithm.</param>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private Byte[] Decrypt(Type symmetricAlgorithm, Byte[] symmetricKey, Byte[] cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Decrypt(symmetricAlgorithm, ProtectedKey.CreateFromPlaintextKey(symmetricKey, dataProtectionScope), cipherText);
        }

        /// <summary>
        /// Decrypts the specified symmetric algorithm.
        /// </summary>
        /// <param name="symmetricAlgorithm">The symmetric algorithm.</param>
        /// <param name="protectedKeyFileName">Name of the protected key file.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private Byte[] Decrypt(Type symmetricAlgorithm, String protectedKeyFileName, Byte[] cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Decrypt(symmetricAlgorithm, KeyManager.Read(protectedKeyFileName, dataProtectionScope), cipherText);
        }

        /// <summary>
        /// Decrypts the specified symmetric algorithm.
        /// </summary>
        /// <param name="symmetricAlgorithm">The symmetric algorithm.</param>
        /// <param name="protectedKey">The protected key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private Byte[] Decrypt(Type symmetricAlgorithm, ProtectedKey protectedKey, Byte[] cipherText)
        {
            var symmetricProvider = new SymmetricAlgorithmProvider(
                symmetricAlgorithm, protectedKey);

            return symmetricProvider.Decrypt(cipherText);
        }

        #endregion
    }
}