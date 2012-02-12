// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApplicationConfiguration.cs">
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
//   Defines an application configuration contract.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;

namespace NContext.Configuration
{
    /// <summary>
    /// Defines an application configuration contract.
    /// </summary>
    /// <remarks></remarks>
    public interface IApplicationConfiguration
    {
        #region Properties

        /// <summary>
        /// Gets a value indicating whether this instance is configured.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is configured; otherwise, <c>false</c>.
        /// </value>
        Boolean IsConfigured { get; }

        /// <summary>
        /// Gets the application composition container.
        /// </summary>
        CompositionContainer CompositionContainer { get; }

        /// <summary>
        /// Gets the application components.
        /// </summary>
        /// <remarks></remarks>
        IEnumerable<IApplicationComponent> Components { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the application component by type registered.
        /// </summary>
        /// <typeparam name="TApplicationComponent">The type of the application component.</typeparam>
        /// <returns>Instance of <typeparamref name="TApplicationComponent"/> if it exists, else null.</returns>
        /// <remarks></remarks>
        TApplicationComponent GetComponent<TApplicationComponent>()
            where TApplicationComponent : IApplicationComponent;

        /// <summary>
        /// Registers an application component.
        /// </summary>
        /// <typeparam name="TApplicationComponent">The type of the application component.</typeparam>
        /// <param name="componentFactory">The component factory.</param>
        /// <remarks></remarks>
        void RegisterComponent<TApplicationComponent>(Func<IApplicationComponent> componentFactory)
            where TApplicationComponent : IApplicationComponent;

        /// <summary>
        /// Creates all application components and configures them.
        /// </summary>
        /// <remarks>Should only be called once from application startup.</remarks>
        void Setup();

        #endregion
    }
}