// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationConfigurationBuilder.cs">
//   This file is part of NContext.
//
//   NContext is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or any later version.
//
//   NContext is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with NContext.  If not, see <http://www.gnu.org/licenses/>.// </copyright>
// <summary>
//   Defines a fluent interface builder for application configuration.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace NContext.Application.Configuration
{
    /// <summary>
    /// Defines a fluent interface builder for application configuration.
    /// </summary>
    /// <remarks></remarks>
    public class ApplicationConfigurationBuilder
    {
        #region Fields
        
        private static readonly ApplicationConfiguration _ApplicationConfiguration;

        private static readonly Dictionary<Type, ApplicationComponentConfigurationBuilder> _Components =
            new Dictionary<Type, ApplicationComponentConfigurationBuilder>();
        
        #endregion

        #region Constructors
        
        /// <summary>
        /// Initializes static members of the <see cref="ApplicationConfigurationBuilder"/> class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        static ApplicationConfigurationBuilder()
        {
            _ApplicationConfiguration = new ApplicationConfiguration();
        }

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
        /// Registers the component with the <see cref="IApplicationConfiguration"/>.
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