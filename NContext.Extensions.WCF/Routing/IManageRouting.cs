// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IManageRouting.cs">
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
//   Defines interface contract which encapsulates logic for creating WCF service routes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ServiceModel.Activation;

using Microsoft.ApplicationServer.Http.Activation;

using NContext.Configuration;

namespace NContext.Extensions.WCF.Routing
{
    /// <summary>
    /// Defines interface contract which encapsulates logic for creating WCF service routes.
    /// </summary>
    public interface IManageRouting : IApplicationComponent
    {
        /// <summary>
        /// Registers the service route.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="routePrefix">The route prefix.</param>
        /// <remarks></remarks>
        void RegisterServiceRoute<TService>(String routePrefix);

        /// <summary>
        /// Registers the service route with the specified <see cref="HttpServiceHostFactory"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="routePrefix">The route prefix.</param>
        /// <param name="httpServiceHostFactory">The <see cref="HttpServiceHostFactory"/> to use with the service.</param>
        /// <remarks></remarks>
        void RegisterServiceRoute<TService>(String routePrefix, Func<HttpServiceHostFactory> httpServiceHostFactory);

        /// <summary>
        /// Registers the service route with the specified <see cref="ServiceHostFactory"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="routePrefix">The route prefix.</param>
        /// <param name="serviceHostFactory">The <see cref="ServiceHostFactory"/> to use with the service.</param>
        /// <remarks></remarks>
        void RegisterServiceRoute<TService>(String routePrefix, Func<ServiceHostFactory> serviceHostFactory);

        /// <summary>
        /// Registers the service route.
        /// </summary>
        /// <typeparam name="TServiceContract">The type of the service contract.</typeparam>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="routePrefix">The route prefix.</param>
        /// <remarks></remarks>
        void RegisterServiceRoute<TServiceContract, TService>(String routePrefix);

        /// <summary>
        /// Registers the service route with the specified <see cref="HttpServiceHostFactory"/>.
        /// </summary>
        /// <typeparam name="TServiceContract">The type of the service contract.</typeparam>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="routePrefix">The route prefix.</param>
        /// <param name="httpServiceHostFactory">The <see cref="HttpServiceHostFactory"/> to use with the service.</param>
        /// <remarks></remarks>
        void RegisterServiceRoute<TServiceContract, TService>(String routePrefix, Func<HttpServiceHostFactory> httpServiceHostFactory);

        /// <summary>
        /// Registers the service route with the specified <see cref="ServiceHostFactory"/>.
        /// </summary>
        /// <typeparam name="TServiceContract">The type of the service contract.</typeparam>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="routePrefix">The route prefix.</param>
        /// <param name="serviceHostFactory">The service host factory.</param>
        /// <remarks></remarks>
        void RegisterServiceRoute<TServiceContract, TService>(String routePrefix, Func<ServiceHostFactory> serviceHostFactory);

        /// <summary>
        /// Registers the service route with the specified <see cref="HttpServiceHostFactory"/> and <see cref="ServiceHostFactory"/>.
        /// </summary>
        /// <typeparam name="TServiceContract">The type of the service contract.</typeparam>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="routePrefix">The route prefix.</param>
        /// <param name="httpServiceHostFactory">The <see cref="HttpServiceHostFactory"/> to use with the service.</param>
        /// <param name="serviceHostFactory">The <see cref="ServiceHostFactory"/> to use with the service.</param>
        /// <remarks></remarks>
        void RegisterServiceRoute<TServiceContract, TService>(String routePrefix, Func<HttpServiceHostFactory> httpServiceHostFactory, Func<ServiceHostFactory> serviceHostFactory);
    }
}