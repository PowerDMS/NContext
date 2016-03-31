namespace NContext.Extensions.AspNetWebApi.Routing
{
    using System;

    /// <summary>
    /// Defines a class which represents a Web API service route.
    /// </summary>
    public class Route
    {
        private readonly String _RouteName;

        private readonly String _RouteTemplate;

        private readonly Object _Defaults;

        private readonly Object _Constraints;

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
    }
}