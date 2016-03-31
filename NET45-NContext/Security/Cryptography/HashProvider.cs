namespace NContext.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    using NContext.Extensions;

    /// <summary>
    /// Defines a provider for cryptographic hash operations.
    /// </summary>
    public class HashProvider : IProvideHashing
    {
        private readonly Type _DefaultHashAlgorithm;

        private readonly RandomNumberGenerator _RngCryptoServiceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="HashProvider"/> class.
        /// </summary>
        /// <param name="defaultHashAlgorithm">The default <see cref="HashAlgorithm"/>.</param>
        /// <remarks></remarks>
        public HashProvider(Type defaultHashAlgorithm)
        {
            if (defaultHashAlgorithm == null)
            {
                throw new ArgumentNullException("defaultHashAlgorithm");
            }

            if (!defaultHashAlgorithm.Implements<HashAlgorithm>())
            {
                throw new ArgumentException("DefaultHashAlgorithm is invalid. Must be of type HashAlgorithm.", "defaultHashAlgorithm");
            }

            _DefaultHashAlgorithm = defaultHashAlgorithm;
            _RngCryptoServiceProvider = new RNGCryptoServiceProvider();
        }

        /// <summary>
        /// Creates the hash using the default <see cref="HashAlgorithm" />.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltLength">The length of bytes to use as salt.</param>
        /// <returns>The created hash.</returns>
        public Byte[] CreateHash(Byte[] plainText, Int32 saltLength = 16)
        {
            return CreateHash(_DefaultHashAlgorithm, plainText, saltLength);
        }

        /// <summary>
        /// Creates the hash using the default <see cref="HashAlgorithm" />.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltLength">The length of bytes to use as salt.</param>
        /// <returns>The created hash.</returns>
        public Byte[] CreateHash(Stream plainText, Int32 saltLength = 16)
        {
            return CreateHash(_DefaultHashAlgorithm, plainText, saltLength);
        }

        /// <summary>
        /// Creates the hash using the default <see cref="HashAlgorithm" />.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltLength">The length of bytes to use as salt.</param>
        /// <returns>The created hash.</returns>
        public String CreateHash(String plainText, Int32 saltLength = 16)
        {
            return CreateHash(_DefaultHashAlgorithm, plainText.ToBytes<UnicodeEncoding>(), saltLength).ToHexadecimal();
        }

        /// <summary>
        /// Creates the base64 encoded hash using the default <see cref="HashAlgorithm" />.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltLength">The length of bytes to use as salt.</param>
        /// <returns>The created hash.</returns>
        public String CreateHashToBase64(String plainText, Int32 saltLength = 16)
        {
            return CreateHash(_DefaultHashAlgorithm, plainText.ToBytes<UnicodeEncoding>(), saltLength).ToBase64();
        }

        /// <summary>
        /// Creates the hash using the specified <see cref="HashAlgorithm" />.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltLength">The length of bytes to use as salt.</param>
        /// <returns>The created hash.</returns>
        public Byte[] CreateHash<THashAlgorithm>(Byte[] plainText, Int32 saltLength = 16)
            where THashAlgorithm : HashAlgorithm, new()
        {
            return CreateHash(typeof(THashAlgorithm), plainText, saltLength);
        }

        /// <summary>
        /// Creates the hash using the specified <see cref="HashAlgorithm" />.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltLength">The length of bytes to use as salt.</param>
        /// <returns>The created hash.</returns>
        public Byte[] CreateHash<THashAlgorithm>(Stream plainText, Int32 saltLength = 16)
            where THashAlgorithm : HashAlgorithm, new()
        {
            return CreateHash(typeof(THashAlgorithm), plainText, saltLength);
        }

        /// <summary>
        /// Creates the hash using the specified <see cref="HashAlgorithm" />.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltLength">The length of bytes to use as salt.</param>
        /// <returns>The created hash.</returns>
        public String CreateHash<THashAlgorithm>(String plainText, Int32 saltLength = 16)
            where THashAlgorithm : HashAlgorithm, new()
        {
            return CreateHash(typeof(THashAlgorithm), plainText.ToBytes<UnicodeEncoding>(), saltLength).ToHexadecimal();
        }

        /// <summary>
        /// Creates the base64 encoded hash using the specified <typeparamref name="THashAlgorithm" />.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltLength">The length of bytes to use as salt.</param>
        /// <returns>The created hash.</returns>
        public String CreateHashToBase64<THashAlgorithm>(String plainText, Int32 saltLength = 16)
            where THashAlgorithm : HashAlgorithm, new()
        {
            return CreateHash(typeof(THashAlgorithm), plainText.ToBytes<UnicodeEncoding>(), saltLength).ToBase64();
        }

        private Byte[] CreateHash(Type hashAlgorithmType, Byte[] plainText, Int32 saltLength = 16)
        {
            var hashAlgorithm = Activator.CreateInstance(hashAlgorithmType, true) as HashAlgorithm;
            if (hashAlgorithm == null)
            {
                throw new InvalidOperationException(String.Format("Could not create instance of type {0}.", hashAlgorithmType));
            }

            // Generate salt
            var salt = new Byte[saltLength];
            _RngCryptoServiceProvider.GetNonZeroBytes(salt);

            // Compute hash
            var hashedText = hashAlgorithm.ComputeHash(CryptographyUtility.CombineBytes(salt, plainText));

            // Randomize plain text
            _RngCryptoServiceProvider.GetBytes(plainText);

            return CryptographyUtility.CombineBytes(salt, hashedText);
        }

        private Byte[] CreateHash(Type hashAlgorithmType, Stream plainText, Int32 saltLength = 16)
        {
            var hashAlgorithm = Activator.CreateInstance(hashAlgorithmType, true) as HashAlgorithm;
            if (hashAlgorithm == null)
            {
                throw new InvalidOperationException(String.Format("Could not create instance of type {0}.", hashAlgorithmType));
            }

            // Generate salt
            var salt = new Byte[saltLength];
            _RngCryptoServiceProvider.GetNonZeroBytes(salt);

            // Compute hash
            plainText.Position = 0;
            var hashedText = hashAlgorithm.ComputeHash(new SaltyStream(salt, plainText));

            // Close the stream
            plainText.Close();

            return CryptographyUtility.CombineBytes(salt, hashedText);
        }

        /// <summary>
        /// Compares the hash using the default <see cref="HashAlgorithm" />.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedBytes">The hashed text.</param>
        /// <param name="saltLength">The length of bytes used for salt in the <paramref name="hashedBytes" />.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        public Boolean CompareHash(Byte[] plainText, Byte[] hashedBytes, Int32 saltLength = 16)
        {
            return CompareHash(_DefaultHashAlgorithm, plainText, hashedBytes, saltLength);
        }

        /// <summary>
        /// Compares the hash using the default <see cref="HashAlgorithm" />.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedBytes">The hashed text.</param>
        /// <param name="saltLength">The length of bytes used for salt in the <paramref name="hashedBytes" />.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        public Boolean CompareHash(Stream plainText, Byte[] hashedBytes, Int32 saltLength = 16)
        {
            return CompareHash(_DefaultHashAlgorithm, plainText, hashedBytes, saltLength);
        }

        /// <summary>
        /// Compares the hash using the default <see cref="HashAlgorithm" />.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltLength">The length of bytes used for salt in the <paramref name="hashedText" />.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        public Boolean CompareHash(String plainText, String hashedText, Int32 saltLength = 16)
        {
            return CompareHash(_DefaultHashAlgorithm, plainText.ToBytes<UnicodeEncoding>(), hashedText.ToBytesFromHexadecimal(), saltLength);
        }

        /// <summary>
        /// Compares the hash using the default <see cref="HashAlgorithm" />.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltLength">The length of bytes used for salt in the <paramref name="hashedText" />.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        public Boolean CompareHashFromBase64(String plainText, String hashedText, Int32 saltLength = 16)
        {
            return CompareHash(_DefaultHashAlgorithm, plainText.ToBytes<UnicodeEncoding>(), hashedText.ToBytesFromBase64(), saltLength);
        }

        /// <summary>
        /// Compares the hash using the specified <see cref="HashAlgorithm" />.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedBytes">The hashed text.</param>
        /// <param name="saltLength">The length of bytes used for salt in the <paramref name="hashedBytes" />.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        public Boolean CompareHash<THashAlgorithm>(Byte[] plainText, Byte[] hashedBytes, Int32 saltLength = 16)
            where THashAlgorithm : HashAlgorithm, new()
        {
            return CompareHash(typeof(THashAlgorithm), plainText, hashedBytes, saltLength);
        }

        /// <summary>
        /// Compares the hash using the specified <see cref="HashAlgorithm" />.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedBytes">The hashed text.</param>
        /// <param name="saltLength">The length of bytes used for salt in the <paramref name="hashedBytes" />.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        public Boolean CompareHash<THashAlgorithm>(Stream plainText, Byte[] hashedBytes, Int32 saltLength = 16)
            where THashAlgorithm : HashAlgorithm, new()
        {
            return CompareHash(typeof(THashAlgorithm), plainText, hashedBytes, saltLength);
        }

        /// <summary>
        /// Compares the hash using the specified <see cref="HashAlgorithm" />.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltLength">The length of bytes used for salt in the <paramref name="hashedText" />.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        public Boolean CompareHash<THashAlgorithm>(String plainText, String hashedText, Int32 saltLength = 16)
            where THashAlgorithm : HashAlgorithm, new()
        {
            return CompareHash(typeof(THashAlgorithm), plainText.ToBytes<UnicodeEncoding>(), hashedText.ToBytesFromHexadecimal(), saltLength);
        }

        /// <summary>
        /// Compares the hash using the specified <see cref="HashAlgorithm" />.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltLength">The length of bytes used for salt in the <paramref name="hashedText" />.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        public Boolean CompareHashFromBase64<THashAlgorithm>(String plainText, String hashedText, Int32 saltLength = 16)
            where THashAlgorithm : HashAlgorithm, new()
        {
            return CompareHash(typeof(THashAlgorithm), plainText.ToBytes<UnicodeEncoding>(), hashedText.ToBytesFromBase64(), saltLength);
        }

        private Boolean CompareHash(Type hashAlgorithmType, Byte[] plainText, Byte[] hashedBytes, Int32 saltLength = 16)
        {
            var hashAlgorithm = Activator.CreateInstance(hashAlgorithmType, true) as HashAlgorithm;
            if (hashAlgorithm == null)
            {
                throw new InvalidOperationException(String.Format("Could not create instance of type {0}.", hashAlgorithmType));
            }

            var salt = CryptographyUtility.GetBytes(hashedBytes, saltLength);
            var targetHash = CryptographyUtility.CombineBytes(salt, hashAlgorithm.ComputeHash(CryptographyUtility.CombineBytes(salt, plainText)));

            return CryptographyUtility.CompareBytes(hashedBytes, targetHash);
        }

        private Boolean CompareHash(Type hashAlgorithmType, Stream plainText, Byte[] hashedBytes, Int32 saltLength = 16)
        {
            var hashAlgorithm = Activator.CreateInstance(hashAlgorithmType, true) as HashAlgorithm;
            if (hashAlgorithm == null)
            {
                throw new InvalidOperationException(String.Format("Could not create instance of type {0}.", hashAlgorithmType));
            }

            plainText.Position = 0;
            var salt = CryptographyUtility.GetBytes(hashedBytes, saltLength);
            var targetHash = CryptographyUtility.CombineBytes(salt, hashAlgorithm.ComputeHash(new SaltyStream(salt, plainText)));

            return CryptographyUtility.CompareBytes(hashedBytes, targetHash);
        }

        private class SaltyStream : Stream
        {
            private Int64 _Position;

            private readonly IEnumerable<Stream> _Streams;

            public SaltyStream(Byte[] salt, Stream plainTextStream)
            {
                _Streams = new List<Stream>(
                    new[]
                    {
                        new MemoryStream(salt),
                        plainTextStream
                    });
            }

            public override Boolean CanRead
            {
                get { return true; }
            }

            public override Boolean CanSeek
            {
                get { return true; }
            }

            public override Boolean CanWrite
            {
                get { return false; }
            }

            public override Int64 Length
            {
                get
                {
                    return _Streams.Sum(stream => stream.Length);
                }
            }

            public override Int64 Position
            {
                get { return _Position; }
                set { Seek(value, SeekOrigin.Begin); }
            }

            public override void Flush() { }

            public override Int64 Seek(Int64 offset, SeekOrigin origin)
            {
                var len = Length;
                switch (origin)
                {
                    case SeekOrigin.Begin:
                        _Position = offset;
                        break;
                    case SeekOrigin.Current:
                        _Position += offset;
                        break;
                    case SeekOrigin.End:
                        _Position = len - offset;
                        break;
                }

                if (_Position > len)
                {
                    _Position = len;
                }

                else if (_Position < 0)
                {
                    _Position = 0;
                }

                return _Position;
            }

            public override void SetLength(Int64 value)
            {
            }

            public override Int32 Read(Byte[] buffer, Int32 offset, Int32 count)
            {
                Int64 len = 0;
                Int32 result = 0;
                Int32 bufPos = offset;

                foreach (var stream in _Streams)
                {
                    if (_Position < (len + stream.Length))
                    {
                        stream.Position = _Position - len;
                        var bytesRead = stream.Read(buffer, bufPos, count);
                        result += bytesRead;
                        bufPos += bytesRead;
                        _Position += bytesRead;
                        if (bytesRead < count)
                        {
                            count -= bytesRead;
                        }
                        else
                        {
                            break;
                        }
                    }

                    len += stream.Length;
                }

                return result;
            }
            
            public override void Write(Byte[] buffer, Int32 offset, Int32 count)
            {
            }
        }
    }
}