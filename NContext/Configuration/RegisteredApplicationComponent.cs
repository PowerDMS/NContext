// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisteredApplicationComponent.cs">
//   Copyright (c) 2012
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
//   Defines an application component that has been registered with an ApplicationConfigurationBase.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace NContext.Configuration
{
    /// <summary>
    /// Defines an application component that has been registered with an ApplicationConfigurationBase.
    /// </summary>
    /// <remarks></remarks>
    public struct RegisteredApplicationComponent
    {
        private readonly Type _RegisteredComponentType;

        private readonly Lazy<IApplicationComponent> _ApplicationComponentFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteredApplicationComponent"/> struct.
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