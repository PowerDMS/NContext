// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationConfiguration.cs">
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
using System.Web;

using NContext.Extensions;

namespace NContext.Configuration
{
    /// <summary>
    /// Defines a class for application configuration.
    /// </summary>
    public class ApplicationConfiguration : IApplicationConfiguration
    {
        #region Fields

        private readonly IDictionary<Type, Lazy<IApplicationComponent>> _Components =
            new Dictionary<Type, Lazy<IApplicationComponent>>();

        private readonly HashSet<String> _CompositionDirectories;

        private readonly HashSet<Predicate<String>> _CompositionFileNameConstraints; 

        private CompositionContainer _CompositionContainer;

        private Boolean _IsConfigured;

        #endregion

        #region Constructors

        public ApplicationConfiguration()
        {
            _CompositionDirectories = new HashSet<String>();
            _CompositionFileNameConstraints = new HashSet<Predicate<String>>
                {
                    new Predicate<String>(fileName => fileName.StartsWith("NContext", StringComparison.OrdinalIgnoreCase))
                };
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
        /// Adds the specified conditions for application composition.
        /// </summary>
        /// <param name="directories">The directories.</param>
        /// <param name="fileNameConstraints">The file name constraints.</param>
        /// <remarks></remarks>
        public void AddCompositionConditions(IEnumerable<String> directories, IEnumerable<Predicate<String>> fileNameConstraints)
        {
            _CompositionDirectories.AddRange(directories);
            _CompositionFileNameConstraints.AddRange(fileNameConstraints);
        }

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
        public virtual void Setup()
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
                    postConfigurationActions.ForEach(postConfigurationAction => postConfigurationAction.Value.Run(component.Value.Value));
                });

                _IsConfigured = true;
                _CompositionContainer.GetExportedValues<IRunWhenApplicationConfigurationIsComplete>()
                                     .OrderBy(c => c.Priority)
                                     .ForEach(c => c.Run(this));
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
                throw new Exception("Could not create composition container. Invalid directory path.");
            }

            var directoryCatalog = new SafeDirectoryCatalog(_CompositionDirectories, _CompositionFileNameConstraints);
            var compositionContainer = new CompositionContainer(directoryCatalog);
            if (compositionContainer == null)
            {
                throw new Exception("Could not create composition container. Container cannot be null.");
            }

            return compositionContainer;
        }

        #endregion

        #region Unit Testing

        internal void Reset()
        {
            _IsConfigured = false;
            _CompositionContainer = null;

            if (_Components != null)
            {
                GetType().GetField("_Components", BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic)
                         .SetValue(this, new Dictionary<Type, Lazy<IApplicationComponent>>());
            }
        }

        #endregion
    }
}