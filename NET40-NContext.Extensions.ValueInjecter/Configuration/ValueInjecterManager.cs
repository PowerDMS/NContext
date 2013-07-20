// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueInjecterManager.cs" company="Waking Venture, Inc.">
//   Copyright (c) 2013 Waking Venture, Inc.
// 
//   Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//   documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
//   the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
//   and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
//   The above copyright notice and this permission notice shall be included in all copies or substantial portions 
//   of the Software.
// 
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//   TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//   CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//   DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
// 
namespace NContext.Extensions.ValueInjecter.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NContext.Configuration;

    using Omu.ValueInjecter;

    /// <summary>
    /// Defines an application component for ValueInjecter. This component is used for 
    /// mapping objects based upon specified conventions.
    /// </summary>
    public class ValueInjecterManager : IManageValueInjecter
    {
        private static IEnumerable<Func<IValueInjection>> _ApplicationInjectionConventions;

        private Boolean _IsConfigured;

        static ValueInjecterManager()
        {
            _ApplicationInjectionConventions = Enumerable.Empty<Func<IValueInjection>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueInjecterManager"/> class.
        /// </summary>
        /// <param name="valueInjecterConfiguration">The value injecter configuration.</param>
        public ValueInjecterManager(ValueInjecterConfiguration valueInjecterConfiguration)
        {
            if (valueInjecterConfiguration.Conventions != null)
            {
                _ApplicationInjectionConventions = valueInjecterConfiguration.Conventions;
            }
        }

        /// <summary>
        /// Gets the application's injection conventions.
        /// </summary>
        /// <value>The conventions.</value>
        public static IEnumerable<Func<IValueInjection>> Conventions
        {
            get { return _ApplicationInjectionConventions; }
        }

        /// <summary>
        /// Returns a new <see cref="IFluentValueInjecter{T}" /> instance using <paramref name="source" />
        /// as the source object for value injection.
        /// </summary>
        /// <typeparam name="TSource">The type of the T source.</typeparam>
        /// <param name="source">The source intance.</param>
        /// <returns>IFluentValueInjecter{TSource}.</returns>
        /// <exception cref="System.ArgumentNullException">source</exception>
        public IFluentValueInjecter<TSource> Inject<TSource>(TSource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return new FluentValueInjecter<TSource>(source, Conventions);
        }

        public Boolean IsConfigured
        {
            get { return _IsConfigured; }
        }

        public void Configure(ApplicationConfigurationBase applicationConfiguration)
        {
            if (IsConfigured)
            {
                return;
            }

            _IsConfigured = true;
        }
    }
}