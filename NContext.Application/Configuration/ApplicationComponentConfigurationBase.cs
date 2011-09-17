// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationConfigurationBuilderSectionBase.cs">
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
//   along with NContext.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>

// <summary>
//   Defines an abstraction for creating application component configurations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace NContext.Application.Configuration
{
    /// <summary>
    /// Defines an abstraction for creating application component configurations which 
    /// can in turn be used with an <see cref="ApplicationConfigurationBuilder"/>.
    /// </summary>
    /// <remarks></remarks>
    public abstract class ApplicationComponentConfigurationBase
    {
        #region Fields

        private readonly ApplicationConfigurationBuilder _ApplicationConfigurationBuilder;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationComponentConfigurationBase"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        /// <remarks></remarks>
        protected ApplicationComponentConfigurationBase(ApplicationConfigurationBuilder applicationConfigurationBuilder)
        {
            _ApplicationConfigurationBuilder = applicationConfigurationBuilder;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the builder instance.
        /// </summary>
        /// <remarks></remarks>
        protected ApplicationConfigurationBuilder Builder
        {
            get
            {
                return _ApplicationConfigurationBuilder;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="ApplicationComponentConfigurationBase"/> 
        /// to <see cref="ApplicationConfiguration"/>.
        /// </summary>
        /// <param name="componentConfiguration">The configuration builder section.</param>
        /// <returns>The result of the conversion.</returns>
        /// <remarks></remarks>
        public static implicit operator ApplicationConfiguration(ApplicationComponentConfigurationBase componentConfiguration)
        {
            componentConfiguration.Setup();

            return componentConfiguration.Builder;
        }

        /// <summary>
        /// Registers the component with the <see cref="IApplicationConfiguration"/>.
        /// </summary>
        /// <typeparam name="TApplicationComponent">The type of the application component.</typeparam>
        /// <returns></returns>
        /// <remarks></remarks>
        public ApplicationComponentConfigurationBuilder RegisterComponent<TApplicationComponent>()
            where TApplicationComponent : class, IApplicationComponent
        {
            Setup();

            return _ApplicationConfigurationBuilder.RegisterComponent<TApplicationComponent>();
        }

        /// <summary>
        /// Registers the component with the <see cref="IApplicationConfiguration"/>.
        /// </summary>
        /// <typeparam name="TApplicationComponent">The type of the application component.</typeparam>
        /// <param name="componentFactory">The component factory.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public ApplicationConfigurationBuilder RegisterComponent<TApplicationComponent>(Func<TApplicationComponent> componentFactory)
            where TApplicationComponent : class, IApplicationComponent
        {
            Setup();
            
            return _ApplicationConfigurationBuilder.RegisterComponent<TApplicationComponent>(componentFactory);
        }

        /// <summary>
        /// Applies the component configuration with the <see cref="IApplicationConfiguration"/>.
        /// </summary>
        /// <remarks></remarks>
        protected abstract void Setup();

        #endregion
    }
}