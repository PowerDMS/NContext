namespace NContext
{
    using System;

    /// <summary>
    /// Defines a Nothing implementation of <see cref="IMaybe{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of object.</typeparam>
    public sealed class Error<T> : IMaybe<T>
    {
        public Error()
        {
            // TODO: (DG) Rethink this!
        }

        /// <summary>
        /// Gets a value indicating whether the instance is <see cref="Just{T}"/>.
        /// </summary>
        /// <remarks></remarks>
        public Boolean IsJust
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the instance is <see cref="Error{T}"/>.
        /// </summary>
        /// <remarks></remarks>
        public Boolean IsNothing
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Returns the specified default value if the <see cref="IMaybe{T}"/> is <see cref="Error{T}"/>; 
        /// otherwise, it returns the value contained in the <see cref="IMaybe{T}"/>.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Instance of <typeparamref name="T"/>.</returns>
        /// <remarks></remarks>
        public T FromMaybe(T defaultValue)
        {
            return defaultValue;
        }

        /// <summary>
        /// Returns this instance.
        /// </summary>
        /// <returns>Nothing{T}.</returns>
        public IMaybe<T> Empty()
        {
            return this;
        }

        /// <summary>
        /// Returns a new <see cref="Error{T}"/>.
        /// </summary>
        /// <typeparam name="T2">The type of the result.</typeparam>
        /// <param name="bindingFunction">The function used to bind.</param>
        /// <returns>Instance of <see cref="IMaybe{TResult}"/>.</returns>
        /// <remarks></remarks>
        public IMaybe<T2> Bind<T2>(Func<T, IMaybe<T2>> bindingFunction)
        {
            return new Error<T2>();
        }

        /// <summary>
        /// Returns the current instance.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        /// <returns>Current instance.</returns>
        /// <remarks></remarks>
        public IMaybe<T> Let(Action<T> action)
        {
            return this;
        }
    }
}