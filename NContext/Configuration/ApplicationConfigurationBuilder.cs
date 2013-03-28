// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationConfigurationBuilder.cs" company="Waking Venture, Inc.">
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
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web;

    /// <summary>
    /// Defines a fluent interface builder for application configuration.
    /// </summary>
    /// <remarks></remarks>
    public class ApplicationConfigurationBuilder
    {
        private static readonly ApplicationConfiguration _ApplicationConfiguration =
            new ApplicationConfiguration();

        private static readonly Dictionary<Type, ApplicationComponentBuilder> _Components =
            new Dictionary<Type, ApplicationComponentBuilder>();
        
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
        /// Composes the application for web environments. This will use the <see cref="HttpRuntime.BinDirectory"/> for runtime composition.
        /// </summary>
        /// <param name="fileNameConstraints">The file name constraints.</param>
        /// <returns>Current <see cref="ApplicationComponentBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public ApplicationConfigurationBuilder ComposeForWeb(params Predicate<String>[] fileNameConstraints)
        {
            if (HttpContext.Current == null)
            {
                throw new InvalidOperationException("HttpContext is null. This method can only be used within a web application.");
            }

            ComposeWith(new[] { HttpRuntime.BinDirectory }, fileNameConstraints);

            return this;
        }

        /// <summary>
        /// Composes the application using either the entry assembly location 
        /// or <see cref="AppDomain.CurrentDomain"/> BaseDirectory for runtime composition.
        /// </summary>
        /// <param name="fileNameConstraints">The file name constraints.</param>
        /// <returns>Current <see cref="ApplicationComponentBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public ApplicationConfigurationBuilder ComposeWith(params Predicate<String>[] fileNameConstraints)
        {
            var applicationLocation = Assembly.GetEntryAssembly() == null
                                          ? AppDomain.CurrentDomain.BaseDirectory
                                          : Assembly.GetEntryAssembly().Location;

            ComposeWith(new[] { applicationLocation }, fileNameConstraints);

            return this;
        }

        /// <summary>
        /// Composes the application using the specified directories only, for runtime composition. This can 
        /// be used in conjunction with its overload and <seealso cref="ComposeForWeb"/>.
        /// </summary>
        /// <param name="directories">The directories.</param>
        /// <param name="fileNameConstraints">The file name constraints.</param>
        /// <returns>Current <see cref="ApplicationComponentBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public ApplicationConfigurationBuilder ComposeWith(IEnumerable<String> directories, params Predicate<String>[] fileNameConstraints)
        {
            _ApplicationConfiguration.AddCompositionConditions(directories, fileNameConstraints);

            return this;
        }

        /// <summary>
        /// Registers the component with the <see cref="ApplicationConfigurationBuilder"/> components enumerator.
        /// </summary>
        /// <typeparam name="TApplicationComponent">The type of the application component.</typeparam>
        /// <returns>Current <see cref="ApplicationComponentBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public ApplicationComponentBuilder RegisterComponent<TApplicationComponent>()
            where TApplicationComponent : class, IApplicationComponent
        {
            ApplicationComponentBuilder section;
            if (_Components.TryGetValue(typeof(TApplicationComponent), out section))
            {
                return section;
            }

            var componentConfigurationBuilder = (ApplicationComponentBuilder)Activator.CreateInstance(typeof(ApplicationComponentBuilder), this);
            _Components.Add(typeof(TApplicationComponent), componentConfigurationBuilder);

            return componentConfigurationBuilder;
        }

        /// <summary>
        /// Registers the component with the <see cref="ApplicationConfigurationBase"/> instance.
        /// </summary>
        /// <typeparam name="TApplicationComponent">The type of the application component.</typeparam>
        /// <param name="componentFactory">The component factory.</param>
        /// <returns>Current <see cref="ApplicationComponentBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public ApplicationConfigurationBuilder RegisterComponent<TApplicationComponent>(Func<TApplicationComponent> componentFactory)
            where TApplicationComponent : class, IApplicationComponent
        {
            _ApplicationConfiguration.RegisterComponent<TApplicationComponent>(componentFactory);

            return this;
        }
    }
}