namespace NContext.Common
{
    using System;

    /// <summary>
    /// Defines a generic data-transfer-object for containing an error.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ErrorResponse<T> : ServiceResponse<T>
    {
        private readonly Error _Error;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponse{T}"/> class.
        /// </summary>
        /// <param name="error">The error.</param>
        public ErrorResponse(Error error)
        {
            _Error = error;
        }

        /// <summary>
        /// Gets whether the instance is left.
        /// </summary>
        /// <value>The is left.</value>
        public override Boolean IsLeft
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the left value.
        /// </summary>
        /// <returns>Error.</returns>
        public override Error GetLeft()
        {
            return _Error;
        }

        /// <summary>
        /// Gets the right value.
        /// </summary>
        /// <returns>T.</returns>
        public override T GetRight()
        {
            return default(T);
        }

        /// <summary>
        /// Gets the error.
        /// </summary>
        /// <value>The error.</value>
        public override Error Error
        {
            get { return _Error; }
        }
    }
}