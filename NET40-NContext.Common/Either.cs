namespace NContext.Common
{
    using System;

    /// <summary>
    /// Defines an either abstraction.
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <typeparam name="B"></typeparam>
    public abstract class Either<A, B> : IEither<A, B>
    {
        /// <summary>
        /// Gets whether the implementation is left.
        /// </summary>
        /// <value>The value, is left.</value>
        public abstract Boolean IsLeft { get; }

        /// <summary>
        /// Gets whether the instance is left.
        /// </summary>
        /// <value>The value, is right.</value>
        public Boolean IsRight { get { return !IsLeft; } }

        /// <summary>
        /// Gets the left value.
        /// </summary>
        /// <returns>A.</returns>
        /// <exception cref="InvalidOperationException">Implementations may choose to throw this exception if the instance <see cref="IsRight"/>.  Some may choose to just return null.</exception>
        public abstract A GetLeft();

        /// <summary>
        /// Gets the right value.
        /// </summary>
        /// <returns>B.</returns>
        /// <exception cref="InvalidOperationException">Implementations may choose to throw this exception if the instance <see cref="IsLeft"/>.  Some may choose to just return null.</exception>
        public abstract B GetRight();
    }
}