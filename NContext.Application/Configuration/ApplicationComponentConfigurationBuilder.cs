// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationComponentConfigurationBuilder.cs">
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
//   Defines a fluent-interface builder for application components.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace NContext.Application.Configuration
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