namespace NContext.Common
{
    using System;

    public static class TypeExtensions
    {
        private static readonly ActivationStore _ActivationStore = new ActivationStore();

        /// <summary>
        /// Creates an instance for the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>Object.</returns>
        public static Object CreateInstance(this Type type, params Object[] args)
        {
            return CreateInstance<Object>(type, args);
        }

        /// <summary>
        /// Creates an instance for the specified <paramref name="type"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>T.</returns>
        public static T CreateInstance<T>(this Type type, params Object[] args)
        {
            return _ActivationStore.CreateInstance<T>(type, args);
        }
    }
}