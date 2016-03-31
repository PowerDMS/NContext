namespace NContext.Configuration
{
    using System;

    /// <summary>
    /// Defines an application component that has been registered with an ApplicationConfigurationBase.
    /// </summary>
    /// <remarks></remarks>
    public class RegisteredApplicationComponent
    {
        private readonly Type _RegisteredComponentType;

        private readonly Lazy<IApplicationComponent> _ApplicationComponentFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteredApplicationComponent"/> class.
        /// </summary>
        /// <param name="registeredComponentType">Type of the registered component.</param>
        /// <param name="applicationComponentFactory">The application component.</param>
        /// <remarks></remarks>
        public RegisteredApplicationComponent(Type registeredComponentType, Lazy<IApplicationComponent> applicationComponentFactory)
        {
            _RegisteredComponentType = registeredComponentType;
            _ApplicationComponentFactory = applicationComponentFactory;
        }

        /// <summary>
        /// Gets the type of the registered component.
        /// </summary>
        /// <remarks></remarks>
        public Type RegisteredComponentType
        {
            get
            {
                return _RegisteredComponentType;
            }
        }

        /// <summary>
        /// Gets the application component.
        /// </summary>
        /// <remarks></remarks>
        public IApplicationComponent ApplicationComponent
        {
            get
            {
                return _ApplicationComponentFactory.Value;
            }
        }

        /// <summary>
        /// Gets the application component factory.
        /// </summary>
        /// <remarks></remarks>
        internal Lazy<IApplicationComponent> ApplicationComponentFactory
        {
            get
            {
                return _ApplicationComponentFactory;
            }
        }
    }
}