namespace NContext.Tests.Specs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class SpecAssertionExtensions
    {
        public static Boolean ShouldNotContainOnly<T>(this IEnumerable<T> enumerable, T value)
        {
            return enumerable.Any() && !enumerable.All(item => item.Equals(value));
        }
    }
}