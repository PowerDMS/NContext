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

namespace NContext.Extensions.WebApi.Routing
{
    /// <summary>
    /// Defines a class which represents an HTTP service route.
    /// </summary>
    public class Route
    {
        #region Fields

        private readonly String _RouteName;

        private readonly String _RouteTemplate;

        private readonly Object _Defaults;

        private readonly Object _Constraints;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Route"/> class for hosting within an ASP.NET application.
        /// </summary>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeTemplate">The route template.</param>
        /// <remarks></remarks>
        public Route(String routeName, String routeTemplate)
            : this(routeName, routeTemplate, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Route"/> class for hosting within an ASP.NET application.
        /// </summary>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeTemplate">The route template.</param>
        /// <param name="defaults">The defaults.</param>
        /// <remarks></remarks>
        public Route(String routeName, String routeTemplate, Object defaults)
            : this(routeName, routeTemplate, defaults, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Route"/> class for hosting within an ASP.NET application.
        /// </summary>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeTemplate">The route template.</param>
        /// <param name="defaults">The defaults.</param>
        /// <param name="constraints">The constraints.</param>
        /// <remarks></remarks>
        public Route(String routeName, String routeTemplate, Object defaults, Object constraints)
        {
            _RouteName = routeName;
            _RouteTemplate = routeTemplate;
            _Defaults = defaults;
            _Constraints = constraints;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the route.
        /// </summary>
        /// <remarks></remarks>
        public String RouteName
        {
            get
            {
                return _RouteName;
            }
        }

        /// <summary>
        /// Gets the route template.
        /// </summary>
        /// <remarks></remarks>
        public String RouteTemplate
        {
            get
            {
                return _RouteTemplate;
            }
        }

        /// <summary>
        /// Gets the defaults.
        /// </summary>
        /// <remarks></remarks>
        public Object Defaults
        {
            get
            {
                return _Defaults;
            }
        }

        /// <summary>
        /// Gets the constraints.
        /// </summary>
        /// <remarks></remarks>
        public Object Constraints
        {
            get
            {
                return _Constraints;
            }
        }

        #endregion
    }
}