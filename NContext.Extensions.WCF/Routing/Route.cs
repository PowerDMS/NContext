// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceRoute.cs">
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
//   Defines a class which represents a WCF service route.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ServiceModel.Activation;

using Microsoft.ApplicationServer.Http.Activation;

namespace NContext.Extensions.WCF.Routing
{
    /// <summary>
    /// Defines a class which represents a WCF service route.
    /// </summary>
    public class Route
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Route"/> class.
        /// </summary>
        /// <param name="routePrefix">The route prefix.</param>
        /// <param name="serviceContractType">Type of the service contract.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="binding">The binding.</param>
        /// <remarks></remarks>
        public Route(String routePrefix, Type serviceContractType, Type serviceType, EndpointBinding binding)
        {
            RoutePrefix = routePrefix;
            ServiceContractType = serviceContractType;
            ServiceType = serviceType;
            Binding = binding;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Route"/> class.
        /// </summary>
        /// <param name="routePrefix">The route prefix.</param>
        /// <param name="serviceContractType">Type of the service contract.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="binding">The binding.</param>
        /// <param name="serviceHostFactory">The <see cref="ServiceHostFactory"/> to use for this route.</param>
        /// <remarks></remarks>
        public Route(String routePrefix, Type serviceContractType, Type serviceType, EndpointBinding binding, Func<ServiceHostFactory> serviceHostFactory)
        {
            RoutePrefix = routePrefix;
            ServiceContractType = serviceContractType;
            ServiceType = serviceType;
            Binding = binding;
            ServiceHostFactory = serviceHostFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Route"/> class.
        /// </summary>
        /// <param name="routePrefix">The route prefix.</param>
        /// <param name="serviceContractType">Type of the service contract.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="binding">The binding.</param>
        /// <param name="httpServiceHostFactory">The <see cref="HttpServiceHostFactory"/> to use for this route.</param>
        /// <remarks></remarks>
        public Route(String routePrefix, Type serviceContractType, Type serviceType, EndpointBinding binding, Func<HttpServiceHostFactory> httpServiceHostFactory)
        {
            RoutePrefix = routePrefix;
            ServiceContractType = serviceContractType;
            ServiceType = serviceType;
            Binding = binding;
            HttpServiceHostFactory = httpServiceHostFactory;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the binding.
        /// </summary>
        /// <remarks></remarks>
        public EndpointBinding Binding { get; private set; }

        /// <summary>
        /// Gets the route prefix.
        /// </summary>
        /// <remarks></remarks>
        public String RoutePrefix { get; private set; }

        /// <summary>
        /// Gets the type of the service contract.
        /// </summary>
        /// <remarks></remarks>
        public Type ServiceContractType { get; private set; }

        /// <summary>
        /// Gets the type of the service.
        /// </summary>
        /// <remarks></remarks>
        public Type ServiceType { get; private set; }

        /// <summary>
        /// Gets the <see cref="HttpServiceHostFactory"/>.
        /// </summary>
        /// <remarks></remarks>
        public Func<HttpServiceHostFactory> HttpServiceHostFactory { get; private set; }

        /// <summary>
        /// Gets the <see cref="ServiceHostFactory"/>.
        /// </summary>
        /// <remarks></remarks>
        public Func<ServiceHostFactory> ServiceHostFactory { get; private set; } 

        #endregion
    }
}