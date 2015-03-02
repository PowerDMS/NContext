namespace NContext.Common
{
    using System;
    using System.Collections;

    /// <summary>
    /// Defines extension methods for <see cref="IMaybe{T}"/>.
    /// </summary>
    public static class IMaybeExtensions
    {
        /// <summary>
        /// TODO: (DG) fill...
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="instance">The maybe <typeparamref name="T"/> instance.</param>
        /// <param name="selectFunc">The selectFunc function.</param>
        /// <returns><see cref="IMaybe{TResult}"/>.</returns>
        public static IMaybe<TResult> Select<T, TResult>(this IMaybe<T> instance, Func<T, IMaybe<TResult>> selectFunc)
            where T : IEnumerable
            where TResult : IEnumerable
        {
            return instance.Bind(selectFunc);
        }

        /// <summary>
        /// Returns a new <see cref="IServiceResponse{T}"/>. If <paramref name="instance"/> is <see cref="Nothing{T}"/>, then 
        /// the <paramref name="isNothingToErrorBindingFunc"/> is invoked to return a <see cref="IServiceResponse{T}"/> with 
        /// errors.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The <see cref="IMaybe{T}"/> instance.</param>
        /// <param name="isNothingToErrorBindingFunc">The function to invoke if <paramref name="instance"/> is <see cref="Nothing{T}"/>.</param>
        /// <returns>IServiceResponse{T}.</returns>
        /// <exception cref="System.ArgumentNullException">instance</exception>
        public static IServiceResponse<T> ToServiceResponse<T>(this IMaybe<T> instance, Func<Error> isNothingToErrorBindingFunc)
        {
            if (instance == null) throw new ArgumentNullException("instance");

            if (isNothingToErrorBindingFunc == null) throw new ArgumentNullException("isNothingToErrorBindingFunc");

            if (instance.IsJust)
            {
                return new DataResponse<T>(instance.FromMaybe(default(T)));
            }

            return new ErrorResponse<T>(isNothingToErrorBindingFunc.Invoke());
        }
        
        /// <summary>
        /// Returns a new <see cref="IServiceResponse{T}"/>. If <paramref name="instance"/> is <see cref="Nothing{T}"/>, then 
        /// the <paramref name="defaultValue"/> is used instead of errors.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="defaultValue">The default value to use if <typeparamref name="T"/> is <see cref="Nothing{T}"/>.</param>
        /// <returns>IServiceResponse{0}.</returns>
        /// <exception cref="System.ArgumentNullException">instance</exception>
        public static IServiceResponse<T> ToServiceResponse<T>(this IMaybe<T> instance, T defaultValue)
        {
            if (instance == null) throw new ArgumentNullException("instance");

            if (defaultValue == null) throw new ArgumentNullException("defaultValue");

            return new DataResponse<T>(instance.FromMaybe(defaultValue));
        }

        /// <summary>
        /// Returns the instance as a <see cref="IMaybe{T}"/>
        /// </summary>
        /// <typeparam name="T">The type of the object to wrap</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns><see cref="IMaybe{T}"/></returns>
        public static IMaybe<T> ToMaybe<T>(this T instance)
        {
            if (instance == null)
            {
                return new Nothing<T>();
            }

            return new Just<T>(instance);
        }
    }
}