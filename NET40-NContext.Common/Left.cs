namespace NContext.Common
{
    using System;

    /// <summary>
    /// Defines a left implementation of <see cref="IEither{A,B}"/>.
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <typeparam name="B"></typeparam>
    public class Left<A, B> : Either<A, B>
    {
        private readonly A _Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Left{A, B}" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public Left(A value)
        {
            _Value = value;
        }

        /// <summary>
        /// Gets whether the instance is left.
        /// </summary>
        /// <value>The value, is left.</value>
        public override Boolean IsLeft
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the left value.
        /// </summary>
        /// <returns>A.</returns>
        public override A GetLeft()
        {
            return _Value;
        }

        /// <summary>
        /// Gets the right value.
        /// </summary>
        /// <returns>B.</returns>
        /// <exception cref="System.InvalidOperationException">Instance is left, not right.</exception>
        public override B GetRight()
        {
            throw new InvalidOperationException("Instance is left, not right.");
        }
    }
}