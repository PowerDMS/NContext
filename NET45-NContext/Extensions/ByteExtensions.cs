namespace NContext.Extensions
{
    using System;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// Defines extension methods for <see cref="Byte"/>.
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>
        /// Gets the hexadecimal representation of the specified bytes.
        /// </summary>
        /// <param name="bytes">The bytes to convert to hexadecimal.</param>
        /// <returns>Hexadecimal string value.</returns>
        /// <remarks></remarks>
        public static String ToHexadecimal(this Byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            if (bytes.Length == 0)
            {
                throw new ArgumentException("Byte array length must be greater than zero.", "bytes");
            }

            var stringBuilder = new StringBuilder(bytes.Length * 2);
            for (var index = 0; index < bytes.Length; ++index)
            {
                stringBuilder.Append(bytes[index].ToString("X2", CultureInfo.InvariantCulture));
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Gets the base64 encoded string from the specified bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>Base64 encoded string.</returns>
        /// <remarks></remarks>
        public static String ToBase64(this Byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Gets the UTF8 encoded string from the specified bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>UTF8 encoded string.</returns>
        /// <exception cref=""></exception>
        /// <remarks></remarks>
        public static String ToUTF8(this Byte[] bytes)
        {
            var encoding = new UTF8Encoding(false, true);

            return encoding.GetString(bytes);
        }
    }
}