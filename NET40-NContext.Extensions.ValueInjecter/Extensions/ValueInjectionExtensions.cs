namespace NContext.Extensions.ValueInjecter.Extensions
{
    using System;

    using NContext.Extensions.ValueInjecter.Configuration;

    /// <summary>
    /// Defines extension methods for ValueInjecter.
    /// </summary>
    /// <remarks></remarks>
    public static class ValueInjectionExtensions
    {
        /// <summary>
        /// Returns a new <see cref="IFluentValueInjecter{T}"/> instance using <paramref name="source"/>
        /// as the source object for value injection. Warning! This does not apply any application-wide 
        /// conventions set <see cref="ValueInjecterManager.Conventions"/>.
        /// </summary>
        /// <typeparam name="T">Source type.</typeparam>
        /// <param name="source">The source instance.</param>
        /// <returns>IFluentValueInjecter{T}.</returns>
        /// <exception cref="System.ArgumentNullException">source</exception>
        public static IFluentValueInjecter<T> Inject<T>(this T source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return new FluentValueInjecter<T>(source, ValueInjecterManager.Conventions);
        }
    }
}