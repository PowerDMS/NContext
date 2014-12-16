namespace NContext.Extensions.ValueInjecter.Extensions
{
    using System;

    using Omu.ValueInjecter;

    /// <summary>
    /// Defines extension methods for Value Injecter.
    /// </summary>
    public static class ValueInjecterExtensions
    {
        /// <summary>
        /// Injects values from source into target using the default <see cref="LoopValueInjection" />.
        /// </summary>
        /// <typeparam name="TTarget">The type of the T target.</typeparam>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        /// <returns>TTarget instance.</returns>
        public static TTarget InjectFrom<TTarget>(this TTarget target, Object source)
            where TTarget : class
        {
            return target.InjectFrom<TTarget, LoopValueInjection>(source);
        }

        /// <summary>
        /// Injects values from source into target using the specified <see cref="ValueInjection" />.
        /// </summary>
        /// <typeparam name="TTarget">The type of the T target.</typeparam>
        /// <typeparam name="TValueInjection">The type of the T value injection.</typeparam>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        /// <returns>TTarget instance.</returns>
        public static TTarget InjectFrom<TTarget, TValueInjection>(this TTarget target, Object source)
            where TTarget : class
            where TValueInjection : IValueInjection, new()
        {
            return target.InjectFrom((TValueInjection)Activator.CreateInstance<TValueInjection>(), source) as TTarget;
        }
    }
}