// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationComponentConfigurationBase.cs" company="Waking Venture, Inc.">
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
    /// Defines an abstraction for creating application component configurations which 
    /// can in turn be used with an <see cref="ApplicationConfigurationBuilder"/>.
    /// </summary>
    /// <remarks></remarks>
    public abstract class ApplicationComponentConfigurationBase
    {
        private readonly ApplicationConfigurationBuilder _ApplicationConfigurationBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationComponentConfigurationBase"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        /// <remarks></remarks>
        protected ApplicationComponentConfigurationBase(ApplicationConfigurationBuilder applicationConfigurationBuilder)
        {
            if (applicationConfigurationBuilder == null)
            {
                throw new ArgumentNullException("applicationConfigurationBuilder");
            }

            _ApplicationConfigurationBuilder = applicationConfigurationBuilder;
        }

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

        /// <summary>
        /// Registers the component with the <see cref="ApplicationConfigurationBase" />.
        /// </summary>
        /// <typeparam name="TApplicationComponent">The type of the application component.</typeparam>
        /// <returns>ApplicationComponentConfigurationBuilder.</returns>
        public ApplicationComponentConfigurationBuilder RegisterComponent<TApplicationComponent>()
            where TApplicationComponent : class, IApplicationComponent
        {
            Setup();

            return _ApplicationConfigurationBuilder.RegisterComponent<TApplicationComponent>();
        }

        /// <summary>
        /// Registers the component with the <see cref="ApplicationConfigurationBase" />.
        /// </summary>
        /// <typeparam name="TApplicationComponent">The type of the application component.</typeparam>
        /// <param name="componentFactory">The component factory.</param>
        /// <returns>ApplicationConfigurationBuilder.</returns>
        public ApplicationConfigurationBuilder RegisterComponent<TApplicationComponent>(Func<TApplicationComponent> componentFactory)
            where TApplicationComponent : class, IApplicationComponent
        {
            Setup();
            
            return _ApplicationConfigurationBuilder.RegisterComponent<TApplicationComponent>(componentFactory);
        }

        /// <summary>
        /// Applies the component configuration with the <see cref="ApplicationConfigurationBase"/>.
        /// </summary>
        /// <remarks></remarks>
        protected abstract void Setup();
    }
}