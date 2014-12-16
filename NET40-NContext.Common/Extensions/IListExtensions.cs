namespace NContext.Common
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines extension methods for <see cref="IList{T}"/>.
    /// </summary>
    public static class IListExtensions
    {
        public static IList<T> AddC<T>(this IList<T> list, T item)
        {
            list.Add(item);
            return list;
        }

        public static IList<T> AddRangeC<T>(this IList<T> list, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                list.Add(item);
            }

            return list;
        }
    }
}