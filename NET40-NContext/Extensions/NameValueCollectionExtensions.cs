namespace NContext.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;

    /// <summary>
    /// Defines extension methods for <see cref="NameValueCollection"/>s.
    /// </summary>
    public static class NameValueCollectionExtensions
    {
        /// <summary>
        /// Converts the <see cref="NameValueCollection"/> to a <see cref="Dictionary{TKey,TValue}"/>
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns><see cref="Dictionary{TKey,TValue}"/> which can be enumerated on.</returns>
        /// <remarks></remarks>
        public static IDictionary<String, String> ToDictionary(this NameValueCollection source)
        {
            if (source == null || source.Count <= 0)
            {
                return new Dictionary<String, String>();
            }

            return source.Cast<String>()
                         .Where(key => !String.IsNullOrWhiteSpace(key))
                         .Select(key => new KeyValuePair<String, String>(key, source[key]))
                         .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}