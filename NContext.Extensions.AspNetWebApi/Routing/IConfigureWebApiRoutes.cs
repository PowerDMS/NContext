// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConfigureWebApiRoutes.cs">
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
//   Defines a role-interface used by the routing manger to register service routes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.Composition;

namespace NContext.Extensions.AspNetWebApi.Routing
{
    /// <summary>
    /// Defines a role-interface used by the <see cref="IManageWebApiRouting"/> to register service routes.
    /// </summary>
    [InheritedExport(typeof(IConfigureWebApiRoutes))]
    public interface IConfigureWebApiRoutes
    {
        /// <summary>
        /// Creates the WCF service routes.
        /// </summary>
        /// <param name="webApiRoutingManager">The routing manager.</param>
        void RegisterServiceRoutes(IManageWebApiRouting webApiRoutingManager);
    }
}