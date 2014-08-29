namespace NContext.Common
{
    using System;

    /// <summary>
    /// Defines a type that represents either two possibilities, <typeparamref name="A"/> or <typeparamref name="B"/>.
    /// </summary>
    /// <typeparam name="A">The left type (typically the negative or error).</typeparam>
    /// <typeparam name="B">The right type (mnemonic for correct).</typeparam>
    public interface IEither<out A, out B>
    {
        /// <summary>
        /// Gets whether the instance is left.
        /// </summary>
        /// <value>The value, is left.</value>
        Boolean IsLeft { get; }

        /// <summary>
        /// Gets whether the instance is right.
        /// </summary>
        /// <value>The value, is right.</value>
        Boolean IsRight { get; }

        /// <summary>
        /// Gets the left value.
        /// </summary>
        /// <returns>A.</returns>
        /// <exception cref="InvalidOperationException">Implementations may choose to throw this exception if the instance <see cref="IsRight"/>.  Some may choose to just return null.</exception>
        A GetLeft();

        /// <summary>
        /// Gets the right value.
        /// </summary>
        /// <returns>B.</returns>
        /// <exception cref="InvalidOperationException">Implementations may choose to throw this exception if the instance <see cref="IsLeft"/>.  Some may choose to just return null.</exception>
        B GetRight();
    }
}