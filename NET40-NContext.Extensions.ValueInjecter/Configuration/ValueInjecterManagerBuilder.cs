// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueInjecterManagerBuilder.cs" company="Waking Venture, Inc.">
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

    using NContext.Configuration;

    using Omu.ValueInjecter;

    /// <summary>
    /// Application component builder for <see cref="ValueInjecterManager" />.
    /// </summary>
    public class ValueInjecterManagerBuilder : ApplicationComponentConfigurationBuilderBase
    {
        private readonly ISet<Func<IValueInjection>> _Conventions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationComponentConfigurationBuilderBase" /> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        public ValueInjecterManagerBuilder(ApplicationConfigurationBuilder applicationConfigurationBuilder) : base(applicationConfigurationBuilder)
        {
            _Conventions = new HashSet<Func<IValueInjection>>();
        }

        /// <summary>
        /// Adds the injection convention to be invoked for all value injections.
        /// </summary>
        /// <typeparam name="TValueInjection">The type of the T value injection.</typeparam>
        /// <returns>ValueInjecterManagerBuilder.</returns>
        public ValueInjecterManagerBuilder AddInjectionConvention<TValueInjection>()
            where TValueInjection : IValueInjection, new()
        {
            _Conventions.Add(() => Activator.CreateInstance<TValueInjection>());

            return this;
        }

        /// <summary>
        /// Adds the injection convention to be invoked for all value injections.
        /// </summary>
        /// <param name="valueInjectionFactory">The value injection factory.</param>
        /// <returns>ValueInjecterManagerBuilder.</returns>
        public ValueInjecterManagerBuilder AddInjectionConvention(Func<IValueInjection> valueInjectionFactory)
        {
            _Conventions.Add(valueInjectionFactory);

            return this;
        }

        protected override void Setup()
        {
            Builder.RegisterComponent<IManageValueInjecter>(
                () => new ValueInjecterManager(
                          new ValueInjecterConfiguration(_Conventions)));
        }
    }
}