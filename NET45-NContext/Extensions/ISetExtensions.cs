namespace NContext.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines extension methods for <see cref="ISet{T}"/>.
    /// </summary>
    public static class ISetExtensions
    {
        /// <summary>
        /// Adds the items to the hash set and return the set.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="set">The set.</param>
        /// <param name="itemsToAdd">The items to add.</param>
        /// <returns>The hash set.</returns>
        /// <remarks></remarks>
        public static Boolean AddRange<T>(this ISet<T> set, IEnumerable<T> itemsToAdd)
        {
            return itemsToAdd.All(set.Add);
        }
    }
}