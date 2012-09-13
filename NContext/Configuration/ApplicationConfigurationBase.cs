// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationConfigurationBase.cs" company="Waking Venture, Inc.">
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
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Linq;

    using NContext.Extensions;

    /// <summary>
    /// Defines an application configuration abstraction.
    /// </summary>
    /// <remarks></remarks>
    public abstract class ApplicationConfigurationBase
    {
        private readonly ISet<RegisteredApplicationComponent> _Components =
            new HashSet<RegisteredApplicationComponent>();

        private readonly ISet<String> _CompositionDirectories;

        private readonly ISet<Predicate<String>> _CompositionFileNameConstraints;

        private CompositionContainer _CompositionContainer;

        private Boolean _IsConfigured;

        protected ApplicationConfigurationBase()
        {
            _CompositionDirectories = new HashSet<String>();
            _CompositionFileNameConstraints = new HashSet<Predicate<String>>
                {
                    new Predicate<String>(fileName => fileName.StartsWith("NContext", StringComparison.OrdinalIgnoreCase))
                };
        }

        /// <summary>
        /// Gets the application components.
        /// </summary>
        /// <remarks></remarks>
        public IEnumerable<RegisteredApplicationComponent> Components
        {
            get
            {
                return _Components;
            }
        }

        /// <summary>
        /// Gets the application composition container.
        /// </summary>
        public CompositionContainer CompositionContainer
        {
            get
            {
                return _CompositionContainer;
            }
        }

        /// <summary>
        ///  Gets a value indicating whether this instance is configured.
        /// </summary>
        /// <value><c>true</c> if this instance is configured; otherwise, <c>false</c>.</value>
        public Boolean IsConfigured
        {
            get
            {
                return _IsConfigured;
            }
            protected set
            {
                _IsConfigured = value;
            }
        }

        /// <summary>
        /// Adds the specified conditions for application composition.
        /// </summary>
        /// <param name="directories">The directories.</param>
        /// <param name="fileNameConstraints">The file name constraints.</param>
        /// <remarks></remarks>
        public void AddCompositionConditions(IEnumerable<String> directories, IEnumerable<Predicate<String>> fileNameConstraints)
        {
            if (!_CompositionDirectories.AddRange(directories.Distinct()))
            {
                throw new Exception("NContext was unable to add the specified composition directories.");
            }

            if (!_CompositionFileNameConstraints.AddRange(fileNameConstraints))
            {
                throw new Exception("NContext was unable to add all the specified composition file name constraints. Possible duplicate constraints?");
            }
        }

        /// <summary>
        ///  Gets the application component by type registered.
        ///  </summary>
        /// <typeparam name="TApplicationComponent">The type of the application component.</typeparam>
        /// <returns>Instance of <typeparamref name="TApplicationComponent" /> if it exists, else null.</returns>
        /// <remarks></remarks>
        public TApplicationComponent GetComponent<TApplicationComponent>()
            where TApplicationComponent : IApplicationComponent
        {
            return _Components.Where(pair => pair.RegisteredComponentType == typeof(TApplicationComponent))
                              .MaybeFirst()
                              .Bind<TApplicationComponent>(pair => ((TApplicationComponent)pair.ApplicationComponent).ToMaybe())
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
            _Components.Add(new RegisteredApplicationComponent(typeof(TApplicationComponent), new Lazy<IApplicationComponent>(componentFactory)));
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

                _CompositionContainer = CreateCompositionContainer(_CompositionDirectories, _CompositionFileNameConstraints);
                if (_CompositionContainer == null)
                {
                    throw new Exception("NContext requires a composition container.");
                }

                _CompositionContainer.ComposeExportedValue<CompositionContainer>(_CompositionContainer);

                var postComponentConfigurationActions = _CompositionContainer.GetExports<IRunWhenComponentConfigurationIsComplete>();
                Components.ForEach(component =>
                {
                    component.ApplicationComponent.Configure(this);
                    postComponentConfigurationActions.ForEach(pcca => pcca.Value.Run(component.ApplicationComponent));
                });

                _IsConfigured = true;
                _CompositionContainer.GetExportedValues<IRunWhenApplicationConfigurationIsComplete>()
                                     .OrderBy(c => c.Priority)
                                     .ForEach(c => c.Run(this));
            }
        }

        protected abstract CompositionContainer CreateCompositionContainer(ISet<String> compositionDirectories, ISet<Predicate<String>> compositionFileNameConstraints);
    }
}