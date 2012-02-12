// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProvideResourceAuthorization.cs">
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
//   Defines a provider role for resource authorization.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Security.Principal;

using Microsoft.ApplicationServer.Http.Description;

namespace NContext.Extensions.WCF.WebApi.Authorization
{
    /// <summary>
    /// Defines a provider role for resource authorization.
    /// </summary>
    /// <remarks></remarks>
    public interface IProvideResourceAuthorization
    {
        /// <summary>
        /// Authorizes the specified <see cref="IPrincipal"/> against the specified <see cref="HttpOperationDescription"/>.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="operationDescription">The operation description.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        Boolean Authorize(IPrincipal principal, HttpOperationDescription operationDescription);
    }
}