using System;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;

using NContext.Application.Configuration;

namespace NContext.Application.EnterpriseLibrary
{
    public class EnterpriseLibraryManager : IApplicationComponent
    {
        private Boolean _IsConfigured;
        
        public IContainerConfigurator ContainerConfigurator { get; set; }

        public Boolean IsConfigured
        {
            get
            {
                return _IsConfigured;
            }
        }

        #region Implementation of IApplicationComponent

        public void Configure(IApplicationConfiguration applicationConfiguration)
        {
            _IsConfigured = true;
        }

        #endregion
    }
}