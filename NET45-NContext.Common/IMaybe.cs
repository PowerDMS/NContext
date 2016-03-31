namespace NContext.Common
{
    using System;

    /// <summary>
    /// Defines a Maybe monad contract.
    /// </summary>
    /// <typeparam name="T">Type of object.</typeparam>
    public interface IMaybe<T>
    {
        /// <summary>
        /// Gets a value indicating whether the instance is <see cref="Just{T}"/>.
        /// </summary>
        /// <remarks></remarks>
        Boolean IsJust { get; }

        /// <summary>
        /// Gets a value indicating whether the instance is <see cref="Nothing{T}"/>.
        /// </summary>
        /// <remarks></remarks>
        Boolean IsNothing { get; }

        /// <summary>
        /// Returns the specified default value if the instance is <see cref="Nothing{T}"/>. 
        /// Otherwise, it returns the value contained in <typeparamref name="T"/>.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The specified default value if the instance is <see cref="Nothing{T}"/>. 
        /// Otherwise, it returns the value contained in <typeparamref name="T"/>.
        /// </returns>
        /// <remarks></remarks>
        T FromMaybe(T defaultValue);

        /// <summary>
        /// Returns <see cref="Nothing{T}" />
        /// </summary>
        /// <returns><see cref="Nothing{T}"/>.</returns>
        IMaybe<T> Empty();

        /// <summary>
        /// Binds <typeparamref name="T"/> into an instance of <see cref="IMaybe{T2}"/> if the current 
        /// instance <seealso cref="IMaybe{T}.IsJust"/>. Else, it returns a new instance of <see cref="Nothing{T2}"/>.
        /// </summary>
        /// <typeparam name="T2">The type of the result.</typeparam>
        /// <param name="bindingFunction">The function used to bind.</param>
        /// <returns>Instance of <see cref="IMaybe{TResult}"/>.</returns>
        /// <remarks></remarks>
        IMaybe<T2> Bind<T2>(Func<T, IMaybe<T2>> bindingFunction);

        /// <summary>
        /// Invokes the specified action if the current instance <seealso cref="IsJust"/>.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        /// <returns>Current instance.</returns>
        /// <remarks></remarks>
        IMaybe<T> Let(Action<T> action);
    }
}