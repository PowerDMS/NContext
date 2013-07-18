namespace NContext.Extensions.ValueInjecter
{
    using NContext.Configuration;

    /// <summary>
    /// Defines an application component for ValueInjecter. This component is used for 
    /// mapping objects based upon specified conventions.
    /// </summary>
    public interface IManageValueInjecter : IApplicationComponent
    {
        /// <summary>
        /// Returns a new <see cref="IFluentValueInjecter{T}" /> instance using <paramref name="source" />
        /// as the source object for value injection.
        /// </summary>
        /// <typeparam name="TSource">The type of the T source.</typeparam>
        /// <param name="source">The source intance.</param>
        /// <returns>IFluentValueInjecter{TSource}.</returns>
        /// <exception cref="System.ArgumentNullException">source</exception>
        IFluentValueInjecter<TSource> Inject<TSource>(TSource source);
    }
}