namespace NContext.Extensions.ValueInjecter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    using NContext.Common;

    using Omu.ValueInjecter;

    /// <summary>
    /// Defines a fluent, composable way to use ValueInjecter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FluentValueInjecter<T> : IFluentValueInjecter<T>
    {
        private readonly T _Source;

        private readonly IEnumerable<Func<IValueInjection>> _ApplicationInjectionConventions;

        private readonly ISet<Func<IValueInjection>> _ValueInjections;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FluentValueInjecter{T}" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="applicationInjectionConventions">The application injection conventions.</param>
        /// <exception cref="System.ArgumentNullException">source</exception>
        public FluentValueInjecter(T source, IEnumerable<Func<IValueInjection>> applicationInjectionConventions)
        {
            if (source == null) throw new ArgumentNullException("source");

            _Source = source;
            _ApplicationInjectionConventions = applicationInjectionConventions ?? Enumerable.Empty<Func<IValueInjection>>();
            _ValueInjections = new HashSet<Func<IValueInjection>>();
        }

        /// <summary>
        /// Returns a new instance of <typeparamref name="T2"/>; injecting the source <typeparamref name="T"/> using the 
        /// configured <see cref="IValueInjection"/>. Default <see cref="IValueInjection"/> is <see cref="LoopValueInjection"/>.
        /// </summary>
        /// <typeparam name="T2">The type of target to return.</typeparam>
        /// <returns><typeparamref name="T2"/> instance.</returns>
        public T2 Into<T2>() where T2 : class, new()
        {
            return Into(Activator.CreateInstance<T2>(), null);
        }

        /// <summary>
        /// Returns a new instance of <typeparamref name="T2"/>; injecting the source <typeparamref name="T"/> using the 
        /// configured <see cref="IValueInjection"/>. Then applies the custom mapping logic with <paramref name="mapper"/> 
        /// to the result instance. Default <see cref="IValueInjection"/> is <see cref="LoopValueInjection"/>.
        /// </summary>
        /// <typeparam name="T2">The type of target to return.</typeparam>
        /// <returns><typeparamref name="T2"/> instance.</returns>
        public T2 Into<T2>(Object mapper) where T2 : class, new()
        {
            return Into(Activator.CreateInstance<T2>(), mapper);
        }

        /// <summary>
        /// Returns <paramref name="targetInstance" />; injecting the source <typeparamref name="T" /> using the configured
        /// <see cref="IValueInjection" />. Default <see cref="IValueInjection" /> is <see cref="LoopValueInjection" />.
        /// </summary>
        /// <typeparam name="T2">The type of target to return.</typeparam>
        /// <param name="targetInstance">The target instance.</param>
        /// <returns><typeparamref name="T2" /> instance.</returns>
        public T2 Into<T2>(T2 targetInstance)
        {
            return Into(targetInstance, null);
        }

        /// <summary>
        /// Returns <paramref name="targetInstance" />; injecting the source <typeparamref name="T" /> using the configured
        /// <see cref="IValueInjection" />. Then applies the custom mapping logic with <paramref name="mapper" /> to
        /// <paramref name="targetInstance" />. Default <see cref="IValueInjection" /> is <see cref="LoopValueInjection" />.
        /// </summary>
        /// <typeparam name="T2">The type of target to return.</typeparam>
        /// <param name="targetInstance">The target instance.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns><typeparamref name="T2" /> instance.</returns>
        public T2 Into<T2>(T2 targetInstance, Object mapper)
        {
            if (targetInstance == null) throw new ArgumentNullException("targetInstance");

            if (!_ValueInjections.Any())
            {
                _ValueInjections.Add(() => new LoopValueInjection());
            }

            foreach (var valueInjection in _ValueInjections.Select(valueInjectionFactory => valueInjectionFactory.Invoke())
                                                           .Concat(_ApplicationInjectionConventions.Select(valueInjectionFactory => valueInjectionFactory.Invoke())))
            {
                targetInstance.InjectFrom(valueInjection, _Source);
            }

            if (mapper != null)
            {
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(mapper))
                {
                    PropertyDescriptor propertyDescriptor = descriptor;
                    typeof(T2).GetProperty(descriptor.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase)
                              .ToMaybe()
                              .Let(propertyInfo => propertyInfo.SetValue(targetInstance, propertyDescriptor.GetValue(mapper), null));
                }
            }

            return targetInstance;
        }

        /// <summary>
        /// Configures the type of <see cref="IValueInjection"/> to use with this injector. 
        /// </summary>
        /// <typeparam name="TValueInjection">The type of <see cref="IValueInjection"/>.</typeparam>
        /// <returns>Current <see cref="IFluentValueInjecter{T}"/> instance.</returns>
        public IFluentValueInjecter<T> Using<TValueInjection>() where TValueInjection : IValueInjection, new()
        {
            _ValueInjections.Add(() => Activator.CreateInstance<TValueInjection>());

            return this;
        }

        /// <summary>
        /// Configures the <see cref="IValueInjection"/> instance to use with this injector. 
        /// </summary>
        /// <typeparam name="TValueInjection">The type of <see cref="IValueInjection"/>.</typeparam>
        /// <returns>Current <see cref="IFluentValueInjecter{T}"/> instance.</returns>
        public IFluentValueInjecter<T> Using<TValueInjection>(TValueInjection valueInjection) where TValueInjection : IValueInjection
        {
            _ValueInjections.Add(() => valueInjection);

            return this;
        }
    }
}