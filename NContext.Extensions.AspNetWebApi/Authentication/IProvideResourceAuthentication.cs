// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProvideResourceAuthentication.cs">
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
//   Defines a provider role for resource authentication.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using System.Net.Http;
using System.Security.Principal;

namespace NContext.Extensions.AspNetWebApi.Authentication
{
    /// <summary>
    /// Defines a provider role for resource authentication.
    /// </summary>
    [InheritedExport]
    public interface IProvideResourceAuthentication
    {
        /// <summary>
        /// Determines whether this instance can authenticate the specified request message.
        /// </summary>
        /// <param name="requestMessage">The request message.</param>
        /// <returns><c>true</c> if this instance can authenticate the specified request message; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        Boolean CanAuthenticate(HttpRequestMessage requestMessage);

        /// <summary>
        /// Authenticates the specified request message.
        /// </summary>
        /// <param name="requestMessage">The request message.</param>
        /// <returns>Instance of <see cref="IPrincipal"/>.</returns>
        /// <remarks></remarks>
        IPrincipal Authenticate(HttpRequestMessage requestMessage);
    }
}