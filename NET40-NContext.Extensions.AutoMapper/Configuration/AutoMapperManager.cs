// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoMapperManager.cs" company="Waking Venture, Inc.">
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

namespace NContext.Extensions.AutoMapper.Configuration
{
    using System;
    using System.ComponentModel.Composition;

    using NContext.Configuration;

    using global::AutoMapper;

    /// <summary>
    /// Defines an AutoMapper application component.
    /// </summary>
    /// <remarks></remarks>
    public class AutoMapperManager : IManageAutoMapper
    {
        private static readonly Lazy<IConfiguration> _ConfigurationStore =
            new Lazy<IConfiguration>(() => Mapper.Configuration);

        private static readonly Lazy<IMappingEngine> _MappingEngine =
            new Lazy<IMappingEngine>(() => Mapper.Engine);

        private Boolean _IsConfigured;

        /// <summary>
        /// Gets the configuration provider.
        /// </summary>
        /// <remarks></remarks>
        public IConfigurationProvider ConfigurationProvider
        {
            get
            {
                return (IConfigurationProvider)_ConfigurationStore.Value;
            }
        }

        /// <summary>
        /// Gets the AutoMapper configuration.
        /// </summary>
        /// <remarks></remarks>
        public IConfiguration Configuration
        {
            get
            {
                return _ConfigurationStore.Value;
            }
        }

        /// <summary>
        /// Gets the AutoMapper mapping engine.
        /// </summary>
        /// <remarks></remarks>
        public IMappingEngine MappingEngine
        {
            get
            {
                return _MappingEngine.Value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is configured.
        /// </summary>
        /// <remarks></remarks>
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
        /// Configures the component instance. This method should set <see cref="IApplicationComponent.IsConfigured"/>.
        /// </summary>
        /// <param name="applicationConfiguration">The application configuration.</param>
        /// <remarks>
        /// </remarks>
        public virtual void Configure(ApplicationConfigurationBase applicationConfiguration)
        {
            if (IsConfigured)
            {
                return;
            }

            applicationConfiguration.CompositionContainer.ComposeExportedValue<IManageAutoMapper>(this);
            var mappingConfigurations = applicationConfiguration.CompositionContainer.GetExports<IConfigureAutoMapper>();
            mappingConfigurations.ForEach(mappingConfiguration => mappingConfiguration.Value.Configure(Configuration));

            _IsConfigured = true;
        }
    }
}