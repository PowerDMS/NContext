// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationConfiguration.cs">
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
//   Defines a class for application configuration.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;

using NContext.Application.Extensions;

namespace NContext.Application.Configuration
{
    /// <summary>
    /// Defines a class for application configuration.
    /// </summary>
    public class ApplicationConfiguration : IApplicationConfiguration
    {
        #region Fields

        private static Boolean _IsConfigured;

        private static CompositionContainer _CompositionContainer;

        private static readonly Dictionary<Type, Lazy<IApplicationComponent>> _Components;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        static ApplicationConfiguration()
        {
            _Components = new Dictionary<Type, Lazy<IApplicationComponent>>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this instance is configured.
        /// </summary>
        /// <value><c>true</c> if this instance is configured; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public Boolean IsConfigured
        {
            get
            {
                return _IsConfigured;
            }
        }

        /// <summary>
        /// Gets the application composition container.
        /// </summary>
        /// <remarks></remarks>
        public CompositionContainer CompositionContainer
        {
            get
            {
                return _CompositionContainer;
            }
        }

        /// <summary>
        /// Gets the application components.
        /// </summary>
        /// <remarks></remarks>
        public IEnumerable<IApplicationComponent> Components
        {
            get
            {
                return _Components.Select(c => c.Value.Value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the application component by type registered.
        /// </summary>
        /// <typeparam name="TApplicationComponent">The type of the application component.</typeparam>
        /// <returns>Instance of <typeparamref name="TApplicationComponent"/> if it exists, else null.</returns>
        /// <remarks></remarks>
        public TApplicationComponent GetComponent<TApplicationComponent>()
            where TApplicationComponent : IApplicationComponent
        {
            return _Components.Where(kp => kp.Key == typeof(TApplicationComponent))
                              .MaybeFirst()
                              .Bind<TApplicationComponent>(kp => ((TApplicationComponent)kp.Value.Value).ToMaybe())
                              .FromMaybe(default(TApplicationComponent));
        }

        /// <summary>
        /// Registers an application component.
        /// </summary>
        /// <typeparam name="TApplicationComponent">The type of the application component.</typeparam>
        /// <param name="componentFactory">The component factory.</param>
        /// <remarks></remarks>
        public void RegisterComponent<TApplicationComponent>(Func<IApplicationComponent> componentFactory)
            where TApplicationComponent : IApplicationComponent
        {
            _Components[typeof(TApplicationComponent)] = new Lazy<IApplicationComponent>(componentFactory);
        }

        /// <summary>
        /// Creates all application components and configures them.
        /// </summary>
        /// <remarks>Should only be called once from application startup.</remarks>
        public void Setup()
        {
            if (_IsConfigured)
            {
                return;
            }

            lock (this)
            {
                if (_IsConfigured)
                {
                    return;
                }

                _CompositionContainer = CreateCompositionContainer();
                _CompositionContainer.ComposeExportedValue<CompositionContainer>(_CompositionContainer);
                var postConfigurationActions = _CompositionContainer.GetExports<IRunWhenComponentConfigurationIsComplete>();

                _Components.ForEach(component =>
                    {
                        component.Value.Value.Configure(this);
                        postConfigurationActions.ForEach(postConfigurationAction => 
                                                         postConfigurationAction.Value.Run(component.Value.Value));
                    });

                _CompositionContainer.GetExports<IRunWhenApplicationConfigurationIsComplete>()
                                     .ForEach(c => c.Value.Run(this));

                _IsConfigured = true;
            }
        }

        /// <summary>
        /// Creates the composition container from the executing assembly.
        /// </summary>
        /// <returns>
        /// Application's <see cref="CompositionContainer"/> instance.
        /// </returns>
        protected virtual CompositionContainer CreateCompositionContainer()
        {
            if (_IsConfigured)
            {
                return _CompositionContainer;
            }

            var directoryPath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            if (String.IsNullOrWhiteSpace(directoryPath))
            {
                return null;
            }

            var directoryCatalog = new SafeDirectoryCatalog(directoryPath);

            return new CompositionContainer(directoryCatalog);
        }

        #endregion
    }
}