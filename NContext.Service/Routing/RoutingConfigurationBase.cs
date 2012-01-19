// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoutingConfigurationBase.cs">
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
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

using NContext.Application.Configuration;

namespace NContext.Service.Routing
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

        /// <summary>
        /// Performs an implicit conversion from <see cref="RoutingConfigurationBase"/> to <see cref="Routing.RoutingConfiguration"/>.
        /// </summary>
        /// <param name="routingConfigurationBase">The routing configuration base.</param>
        /// <returns>The result of the conversion.</returns>
        /// <remarks></remarks>
        public static implicit operator RoutingConfiguration(RoutingConfigurationBase routingConfigurationBase)
        {
            routingConfigurationBase.Setup();

            return routingConfigurationBase.RoutingConfigurationBuilder.RoutingConfiguration;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the routing configuration.
        /// </summary>
        /// <remarks></remarks>
        protected RoutingConfiguration RoutingConfiguration
        {
            get
            {
                return _RoutingConfigurationBuilder.RoutingConfiguration;
            }
        }

        /// <summary>
        /// Gets the application configuration builder.
        /// </summary>
        /// <remarks></remarks>
        protected ApplicationConfigurationBuilder ApplicationConfigurationBuilder
        {
            get
            {
                return _ApplicationConfigurationBuilder;
            }
        }

        /// <summary>
        /// Gets the routing configuration builder.
        /// </summary>
        /// <remarks></remarks>
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
        /// Configure WCF routing using the specified <typeparamref name="TRoutingConfiguration"/>.
        /// </summary>
        /// <typeparam name="TRoutingConfiguration">The type of <see cref="RoutingConfigurationBase"/> to use.</typeparam>
        /// <returns>Instance of <typeparamref name="TRoutingConfiguration"/>.</returns>
        /// <remarks></remarks>
        public TRoutingConfiguration ConfigureRouting<TRoutingConfiguration>()
            where TRoutingConfiguration : RoutingConfigurationBase
        {
            Setup();

            return (TRoutingConfiguration)Activator.CreateInstance(typeof(TRoutingConfiguration), _ApplicationConfigurationBuilder, _RoutingConfigurationBuilder);
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

            return RoutingConfigurationBuilder.RoutingConfiguration.RegisterComponent<TApplicationComponent>(componentFactory);
        }

        /// <summary>
        /// Setup the instance.
        /// </summary>
        /// <remarks></remarks>
        protected abstract void Setup();

        #endregion

    }
}