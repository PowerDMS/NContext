// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationComponentBuilder.cs" company="Waking Venture, Inc.">
//   Copyright (c) 2012 Waking Venture, Inc.
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

namespace NContext.Configuration
{
    using System;

    /// <summary>
    /// Defines a fluent-interface builder for application components.
    /// </summary>
    public class ApplicationComponentBuilder
    {
        private readonly ApplicationConfigurationBuilder _ApplicationConfigurationBuilder;

        private ApplicationComponentConfigurationBuilderBase _ApplicationComponentConfigurationBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationComponentBuilder"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration builder.</param>
        /// <remarks></remarks>
        public ApplicationComponentBuilder(ApplicationConfigurationBuilder applicationConfigurationBuilder)
        {
            _ApplicationConfigurationBuilder = applicationConfigurationBuilder;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ApplicationComponentConfigurationBuilderBase"/>
        /// to <see cref="ApplicationConfiguration"/>.
        /// </summary>
        /// <param name="applicationComponentBuilder">The component configuration builder.</param>
        /// <returns>The result of the conversion.</returns>
        /// <remarks></remarks>
        public static implicit operator ApplicationConfiguration(ApplicationComponentBuilder applicationComponentBuilder)
        {
            return applicationComponentBuilder._ApplicationConfigurationBuilder;
        }

        /// <summary>
        /// Configure's the <see cref="IApplicationComponent"/> with the specified <typeparamref name="TComponentConfigurationBuilder"/> instance.
        /// </summary>
        /// <typeparam name="TComponentConfigurationBuilder">The type of the component configuration.</typeparam>
        /// <returns><typeparamref name="TComponentConfigurationBuilder"/> instance to configure.</returns>
        /// <remarks></remarks>
        public TComponentConfigurationBuilder With<TComponentConfigurationBuilder>() 
            where TComponentConfigurationBuilder : ApplicationComponentConfigurationBuilderBase
        {
            if (_ApplicationComponentConfigurationBuilder != null)
            {
                return _ApplicationComponentConfigurationBuilder as TComponentConfigurationBuilder;
            }

            var applicationComponentConfiguration = 
                (TComponentConfigurationBuilder)Activator.CreateInstance(typeof(TComponentConfigurationBuilder), _ApplicationConfigurationBuilder);

            _ApplicationComponentConfigurationBuilder = applicationComponentConfiguration;

            return applicationComponentConfiguration;
        }
    }
}