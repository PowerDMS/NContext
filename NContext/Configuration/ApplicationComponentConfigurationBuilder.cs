// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationComponentConfigurationBuilder.cs">
//   Copyright (c) 2012
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
//
// <summary>
//   Defines a fluent-interface builder for application components.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace NContext.Configuration
{
    /// <summary>
    /// Defines a fluent-interface builder for application components.
    /// </summary>
    public class ApplicationComponentConfigurationBuilder
    {
        #region Fields

        private readonly ApplicationConfigurationBuilder _ApplicationConfigurationBuilder;

        private ApplicationComponentConfigurationBase _ApplicationComponentConfiguration;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationComponentConfigurationBuilder"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration builder.</param>
        /// <remarks></remarks>
        public ApplicationComponentConfigurationBuilder(ApplicationConfigurationBuilder applicationConfigurationBuilder)
        {
            _ApplicationConfigurationBuilder = applicationConfigurationBuilder;
        }

        #endregion

        /// <summary>
        /// Performs an implicit conversion from <see cref="ApplicationComponentConfigurationBase"/>
        /// to <see cref="ApplicationConfiguration"/>.
        /// </summary>
        /// <param name="componentConfigurationBuilder">The component configuration builder.</param>
        /// <returns>The result of the conversion.</returns>
        /// <remarks></remarks>
        public static implicit operator ApplicationConfiguration(ApplicationComponentConfigurationBuilder componentConfigurationBuilder)
        {
            return componentConfigurationBuilder._ApplicationConfigurationBuilder;
        }

        /// <summary>
        /// Configure's the <see cref="IApplicationComponent"/> with the specified <typeparamref name="TComponentConfiguration"/> instance.
        /// </summary>
        /// <typeparam name="TComponentConfiguration">The type of the component configuration.</typeparam>
        /// <returns><typeparamref name="TComponentConfiguration"/> instance to configure.</returns>
        /// <remarks></remarks>
        public TComponentConfiguration With<TComponentConfiguration>() 
            where TComponentConfiguration : ApplicationComponentConfigurationBase
        {
            if (_ApplicationComponentConfiguration != null)
            {
                return _ApplicationComponentConfiguration as TComponentConfiguration;
            }

            var applicationComponentConfiguration = 
                (TComponentConfiguration)Activator.CreateInstance(typeof(TComponentConfiguration), _ApplicationConfigurationBuilder);

            _ApplicationComponentConfiguration = applicationComponentConfiguration;

            return applicationComponentConfiguration;
        }
    }
}