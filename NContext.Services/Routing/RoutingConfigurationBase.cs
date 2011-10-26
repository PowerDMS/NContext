// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoutingConfigurationBase.cs">
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
//
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

using NContext.Application.Configuration;

namespace NContext.Application.Services.Routing
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class RoutingConfigurationBase
    {
        #region Fields

        private readonly ApplicationConfigurationBuilder _ApplicationConfigurationBuilder;

        private readonly RoutingConfigurationBuilder _RoutingConfigurationBuilder;

        #endregion

        #region Constructors

        protected RoutingConfigurationBase(
            ApplicationConfigurationBuilder applicationConfigurationBuilder,
            RoutingConfigurationBuilder routingConfigurationBuilder)
        {
            _ApplicationConfigurationBuilder = applicationConfigurationBuilder;
            _RoutingConfigurationBuilder = routingConfigurationBuilder;
        }

        #endregion
        
        #region Operator Overloads

        /// <summary>
        /// Performs an implicit conversion from <see cref="ApplicationComponentConfigurationBase"/>
        /// to <see cref="ApplicationConfiguration"/>.
        /// </summary>
        /// <param name="routingConfigurationBase">The routing configuration base.</param>
        /// <returns>The result of the conversion.</returns>
        /// <remarks></remarks>
        public static implicit operator ApplicationConfiguration(RoutingConfigurationBase routingConfigurationBase)
        {
            routingConfigurationBase.Setup();
            routingConfigurationBase.RoutingConfiguration.ConfigureInstance();

            return routingConfigurationBase.ApplicationConfigurationBuilder.ApplicationConfiguration;
        }

        public static implicit operator RoutingConfiguration(RoutingConfigurationBase routingConfigurationBase)
        {
            routingConfigurationBase.Setup();

            return routingConfigurationBase.RoutingConfigurationBuilder.RoutingConfiguration;
        }

        #endregion

        #region Properties

        public RoutingConfiguration RoutingConfiguration
        {
            get
            {
                return _RoutingConfigurationBuilder.RoutingConfiguration;
            }
        }

        protected ApplicationConfigurationBuilder ApplicationConfigurationBuilder
        {
            get
            {
                return _ApplicationConfigurationBuilder;
            }
        }

        protected RoutingConfigurationBuilder RoutingConfigurationBuilder
        {
            get
            {
                return _RoutingConfigurationBuilder;
            }
        }

        #endregion

        #region Methods

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

            return RoutingConfigurationBuilder.RoutingConfiguration.RegisterComponent<TApplicationComponent>();
        }

        /// <summary>
        /// Registers the component with the <see cref="IApplicationConfiguration"/>.
        /// </summary>
        /// <typeparam name="TApplicationComponent">The type of the application component.</typeparam>
        /// <param name="componentFactory">The component factory.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public ApplicationConfigurationBuilder RegisterComponent<TApplicationComponent>(
            Func<TApplicationComponent> componentFactory) where TApplicationComponent : class, IApplicationComponent
        {
            Setup();

            return
                RoutingConfigurationBuilder.RoutingConfiguration.RegisterComponent<TApplicationComponent>(componentFactory);
        }

        /// <summary>
        /// Setup the instance.
        /// </summary>
        /// <remarks></remarks>
        protected abstract void Setup();

        #endregion

    }
}