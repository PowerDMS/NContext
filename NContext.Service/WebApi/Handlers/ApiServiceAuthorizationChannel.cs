// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiServiceAuthorizationChannel.cs" company="IDS">
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
//   Defines a WebApi delegating channel which handles session authentication.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Reverb.Application.Configuration;
using Reverb.Application.Extensions;
using Reverb.Application.Services.Authentication;

using Microsoft.Practices.Unity;

namespace Reverb.Application.Services.Channels
{
    /// <summary>
    /// Defines a WebApi delegating channel which handles session authentication.
    /// </summary>
    /// <remarks></remarks>
    internal class ApiServiceAuthorizationChannel : DelegatingChannel
    {
        #region Fields

        private readonly IUnityContainer _Container;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiServiceAuthorizationChannel"/> class.
        /// </summary>
        /// <param name="innerChannel">The inner channel.</param>
        /// <param name="container">The container.</param>
        /// <remarks></remarks>
        public ApiServiceAuthorizationChannel(HttpMessageChannel innerChannel, IUnityContainer container)
            : base(innerChannel)
        {
            _Container = container;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sends the async request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><see cref="Task&lt;HttpResponseMessage&gt;"/> instance to send.</returns>
        /// <remarks></remarks>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AuthenticateRequest(request);

            return base.SendAsync(request, cancellationToken);
        }

        private void AuthenticateRequest(HttpRequestMessage request)
        {
            if (request.Headers.Authorization == null)
            {
                return;
            }

            var authenticationScheme = request.Headers.Authorization.Scheme;
            if (!String.IsNullOrWhiteSpace(authenticationScheme))
            {
                
            }
        }

        #endregion
    }
}