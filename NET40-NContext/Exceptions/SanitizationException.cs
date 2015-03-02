namespace NContext.Exceptions
{
    using System;

    using NContext.Text;

    /// <summary>
    /// Defines an exception when object sanitization fails within <see cref="ObjectGraphSanitizer"/>.
    /// </summary>
    public class SanitizationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SanitizationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public SanitizationException(String message)
            : base(message)
        {
        }
    }
}