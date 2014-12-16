namespace NContext.Extensions
{
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Defines a static class for providing Type extension methods.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Evaluates whether the current type implements the specified type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The base type or interface type to check against.</typeparam>
        /// <param name="type">The derived type.</param>
        /// <returns><c>True</c> if <paramref name="type"/> implements or inherits from type <typeparamref name="T"/>, else <c>false</c>.</returns>
        public static Boolean Implements<T>(this Type type)
        {
            return typeof(T).IsAssignableFrom(type);
        }

        /// <summary>
        /// Evaluates whether the current type implements the type specified.
        /// </summary>
        /// <param name="type">The derived type.</param>
        /// <param name="typeToCheck">The type to check.</param>
        /// <returns><c>True</c> if <paramref name="type"/> implements or inherits from type <paramref name="typeToCheck"/>, else <c>false</c>.</returns>
        /// <remarks></remarks>
        public static Boolean Implements(this Type type, Type typeToCheck)
        {
            return typeToCheck.IsAssignableFrom(type);
        }

        /// <summary>
        /// Determines whether the specified type is anonymous.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the specified type [is anonymous]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public static Boolean IsAnonymousType(this Type type)
        {
            return 
                type.IsGenericType && 
                (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic && 
                (type.Name.StartsWith("<>", StringComparison.OrdinalIgnoreCase) || 
                 type.Name.StartsWith("VB$", StringComparison.OrdinalIgnoreCase)) && 
                type.Name.Contains("AnonymousType") && 
                Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false);
        }
    }
}