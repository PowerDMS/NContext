// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiServiceAuthorizationChannel.cs" company="IDS">
//   This file is part of NContext.
//
//   NContext is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or any later version.
//
//   NContext is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with NContext.  If not, see <http://www.gnu.org/licenses/>.
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