namespace NContext.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Defines extension methods for heiarchical data structures.
    /// </summary>
    public static class HierarchicalDataExtensions
    {
        public static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source, Func<TSource, TSource> hierarchyNavigationExpression)
            where TSource : class
        {
            return FromHierarchy(source, hierarchyNavigationExpression, continuation => continuation != null);
        }

        public static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source, 
            Func<TSource, TSource> hierarchyNavigationExpression, 
            Func<TSource, Boolean> continuationPredicate)
        {
            for (var current = source; continuationPredicate.Invoke(current); current = hierarchyNavigationExpression.Invoke(current))
            {
                yield return current;
            }
        }
    }
}