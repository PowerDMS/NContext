namespace NContext.Common
{
    using System;

    /// <summary>
    /// Defines a right implementation <see cref="IEither{A,B}"/>.
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <typeparam name="B"></typeparam>
    public class Right<A, B> : Either<A, B>
    {
        private readonly B _Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Right{A, B}"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public Right(B value)
        {
            _Value = value;
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
        /// Gets the right value.
        /// </summary>
        /// <returns>A.</returns>
        /// <exception cref="System.InvalidOperationException">Instance is right, not left.</exception>
        public override A GetLeft()
        {
            throw new InvalidOperationException("Instance is right, not left.");
        }

        /// <summary>
        /// Gets the right value.
        /// </summary>
        /// <returns>B.</returns>
        public override B GetRight()
        {
            return _Value;
        }
    }
}