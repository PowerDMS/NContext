// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IManageWebApiRouting.cs">
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
//   Defines interface contract which encapsulates logic for creating ASP.NET Web API service routes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using NContext.Configuration;

namespace NContext.Extensions.AspNetWebApi.Routing
{
    /// <summary>
    /// Defines interface contract which encapsulates logic for creating ASP.NET Web API service routes.
    /// </summary>
    public interface IManageWebApiRouting : IApplicationComponent
    {
        /// <summary>
        /// Gets the service routes registered.
        /// </summary>
        /// <remarks></remarks>
        ICollection<Route> ServiceRoutes { get; }

        /// <summary>
        /// Registers the service route.
        /// </summary>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeTemplate">The route template.</param>
        /// <remarks></remarks>
        void RegisterServiceRoute(String routeName, String routeTemplate);

        /// <summary>
        /// Registers the service route.
        /// </summary>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeTemplate">The route template.</param>
        /// <param name="defaults">The defaults.</param>
        /// <remarks></remarks>
        void RegisterServiceRoute(String routeName, String routeTemplate, Object defaults);

        /// <summary>
        /// Registers the service route.
        /// </summary>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeTemplate">The route template.</param>
        /// <param name="defaults">The defaults.</param>
        /// <param name="constraints">The constraints.</param>
        /// <remarks></remarks>
        void RegisterServiceRoute(String routeName, String routeTemplate, Object defaults, Object constraints);
    }
}