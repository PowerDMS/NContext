namespace NContext.Extensions.ValueInjecter.Extensions
{
    using System;
    using System.Diagnostics.Contracts;

    using NContext.Common;

    using Omu.ValueInjecter;

    /// <summary>
    /// Defines extension methods for <see cref="IServiceResponse{T}"/>
    /// </summary>
    public static class IResponseTransferObjectExtensions
    {
        /// <summary>
        /// Translates the <see cref="IServiceResponse{TSource}"/> instance into an <see cref="IServiceResponse{TTarget}"/>
        /// using <see name="LoopValueInjection"/> and the optionally-specified <paramref name="mapper"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of source object.</typeparam>
        /// <typeparam name="TTarget">The type of target object.</typeparam>
        /// <param name="source">Source instance to translate.</param>
        /// <param name="mapper">A custom anonymous object mapper used for post processing after value injection has taken place.</param>
        /// <returns>Instance of <see cref="IServiceResponse{TTarget}"/>.</returns>
        public static IServiceResponse<TTarget> BindInject<TSource, TTarget>(this IServiceResponse<TSource> source, Func<TSource, Object> mapper = null)
            where TTarget : class, new()
        {
            return source.BindInject<TSource, TTarget, LoopValueInjection>(mapper);
        }

        /// <summary>
        /// Translates the <see cref="IServiceResponse{TSource}"/> instance into an <see cref="IServiceResponse{TTarget}"/>
        /// using the specified <typeparamref name="TValueInjection"/> and custom <paramref name="mapper"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of source object.</typeparam>
        /// <typeparam name="TTarget">The type of target object.</typeparam>
        /// <typeparam name="TValueInjection">The type of <see cref="IValueInjection"/> to use.</typeparam>
        /// <param name="source">Source instance to translate.</param>
        /// <param name="mapper">A custom anonymous object mapper used for post processing after value injection has taken place.</param>
        /// <returns>Instance of <see cref="IServiceResponse{TTarget}"/>.</returns>
        public static IServiceResponse<TTarget> BindInject<TSource, TTarget, TValueInjection>(this IServiceResponse<TSource> source, Func<TSource, Object> mapper = null)
            where TTarget : class, new()
            where TValueInjection : IValueInjection, new()
        {
            return source.BindInject(Activator.CreateInstance<TTarget>(), Activator.CreateInstance<TValueInjection>(), mapper);
        }

        /// <summary>
        /// Translates the <see cref="IServiceResponse{TSource}"/> instance into an <see cref="IServiceResponse{TTarget}"/>
        /// using the specified <typeparamref name="TValueInjection"/> and custom <paramref name="mapper"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of source object.</typeparam>
        /// <typeparam name="TTarget">The type of target object.</typeparam>
        /// <typeparam name="TValueInjection">The type of <see cref="IValueInjection"/> to use.</typeparam>
        /// <param name="source">Source instance to translate.</param>
        /// <param name="target">Target instance to inject into.</param>
        /// <param name="valueInjection"><see cref="IValueInjection"/> instance to use. Default is <see cref="LoopValueInjection"/>.</param>
        /// <param name="mapper">A custom anonymous object mapper used for post processing after value injection has taken place.</param>
        /// <returns>Instance of <see cref="IServiceResponse{TTarget}"/>.</returns>
        public static IServiceResponse<TTarget> BindInject<TSource, TTarget, TValueInjection>(this IServiceResponse<TSource> source, TTarget target, TValueInjection valueInjection, Func<TSource, Object> mapper)
            where TValueInjection : IValueInjection, new()
        {
            Contract.Requires(source != null);

            if (source.Error != null)
            {
                return new ErrorResponse<TTarget>(source.Error);
            }

            return new DataResponse<TTarget>(source.Data.Inject().Using(valueInjection).Into(target, mapper == null ? null : mapper.Invoke(source.Data)));
        }
    }
}