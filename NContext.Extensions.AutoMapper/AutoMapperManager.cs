// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoMapperManager.cs">
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
//   Defines an AutoMapper application component.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

using AutoMapper;
using AutoMapper.Mappers;

using NContext.Configuration;

namespace NContext.Extensions.AutoMapper
{
    /// <summary>
    /// Defines an AutoMapper application component.
    /// </summary>
    /// <remarks></remarks>
    public class AutoMapperManager : IManageAutoMapper
    {
        #region Fields

        private static readonly Lazy<ConfigurationStore> _ConfigurationStore =
            new Lazy<ConfigurationStore>(() => new ConfigurationStore(new TypeMapFactory(), MapperRegistry.AllMappers()));

        private static readonly Lazy<IMappingEngine> _MappingEngine =
            new Lazy<IMappingEngine>(() => new MappingEngine(_ConfigurationStore.Value));

        private Boolean _IsConfigured;

        #endregion

        #region Properties

        public IConfigurationProvider ConfigurationProvider
        {
            get
            {
                return _ConfigurationStore.Value;
            }
        }

        public IConfiguration Configuration
        {
            get
            {
                return _ConfigurationStore.Value;
            }
        }

        public IMappingEngine MappingEngine
        {
            get
            {
                return _MappingEngine.Value;
            }
        }

        #endregion

        #region Implementation of IApplicationComponent

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

            var mappingConfigurations = applicationConfiguration.CompositionContainer.GetExports<IConfigureAutoMapper>();
            mappingConfigurations.ForEach(mappingConfiguration => mappingConfiguration.Value.Configure(Configuration));
            
#if DEBUG
            ConfigurationProvider.AssertConfigurationIsValid();
#endif

            _IsConfigured = true;
        }

        #endregion
    }
}