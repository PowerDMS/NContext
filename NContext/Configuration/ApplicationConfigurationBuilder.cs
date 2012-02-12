// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationConfigurationBuilder.cs">
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
//
// <summary>
//   Defines a fluent interface builder for application configuration.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace NContext.Configuration
{
    /// <summary>
    /// Defines a fluent interface builder for application configuration.
    /// </summary>
    /// <remarks></remarks>
    public class ApplicationConfigurationBuilder
    {
        #region Fields
        
        private static readonly ApplicationConfiguration _ApplicationConfiguration =
            new ApplicationConfiguration();

        private static readonly Dictionary<Type, ApplicationComponentConfigurationBuilder> _Components =
            new Dictionary<Type, ApplicationComponentConfigurationBuilder>();
        
        #endregion
        
        #region Properties

        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        /// <remarks></remarks>
        public ApplicationConfiguration ApplicationConfiguration
        {
            get
            {
                return _ApplicationConfiguration;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="ApplicationConfigurationBuilder"/> 
        /// to <see cref="Configuration.ApplicationConfiguration"/>.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration builder.</param>
        /// <returns>The result of the conversion.</returns>
        /// <remarks></remarks>
        public static implicit operator ApplicationConfiguration(ApplicationConfigurationBuilder applicationConfigurationBuilder)
        {
            return _ApplicationConfiguration;
        }

        /// <summary>
        /// Registers the component with the <see cref="ApplicationConfigurationBuilder"/> components collection.
        /// </summary>
        /// <typeparam name="TApplicationComponent">The type of the application component.</typeparam>
        /// <returns>Instance of <see cref="ApplicationComponentConfigurationBuilder"/>.</returns>
        /// <remarks></remarks>
        public ApplicationComponentConfigurationBuilder RegisterComponent<TApplicationComponent>()
            where TApplicationComponent : class, IApplicationComponent
        {
            ApplicationComponentConfigurationBuilder section;
            if (_Components.TryGetValue(typeof(TApplicationComponent), out section))
            {
                return section;
            }

            var componentConfigurationBuilder = (ApplicationComponentConfigurationBuilder)Activator.CreateInstance(typeof(ApplicationComponentConfigurationBuilder), this);
            _Components.Add(typeof(TApplicationComponent), componentConfigurationBuilder);

            return componentConfigurationBuilder;
        }

        /// <summary>
        /// Registers the component with the <see cref="IApplicationConfiguration"/> instance.
        /// </summary>
        /// <typeparam name="TApplicationComponent">The type of the application component.</typeparam>
        /// <param name="componentFactory">The component factory.</param>
        /// <returns>Current <see cref="IApplicationConfiguration"/> instance.</returns>
        /// <remarks></remarks>
        public ApplicationConfigurationBuilder RegisterComponent<TApplicationComponent>(Func<TApplicationComponent> componentFactory)
            where TApplicationComponent : class, IApplicationComponent
        {
            _ApplicationConfiguration.RegisterComponent<TApplicationComponent>(componentFactory);

            return this;
        }

        #endregion
    }
}