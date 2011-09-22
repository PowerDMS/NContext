// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISecurityManager.cs">
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
//   Defines interface contract which encapsulates logic for application security-related operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.IdentityModel.Tokens;
using System.Security.Principal;

using NContext.Application.Caching;
using NContext.Application.Configuration;

namespace NContext.Application.Security
{
    /// <summary>
    /// Defines interface contract which encapsulates logic for application security-related operations.
    /// </summary>
    /// <remarks></remarks>
    public interface ISecurityManager : IApplicationComponent
    {
        /// <summary>
        /// Saves the principal to cache using the application's <see cref="ICacheManager"/>.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>A token used to retrieve the <see cref="IPrincipal"/> from cache.</returns>
        /// <remarks></remarks>
        SecurityToken SavePrincipal(IPrincipal principal);

        /// <summary>
        /// Saves the principal to cache using the application's <see cref="ICacheManager"/>.
        /// </summary>
        /// <typeparam name="TToken">The type of the token.</typeparam>
        /// <param name="principal">The principal.</param>
        /// <returns>A <typeparamref name="TToken"/> token used to retrieve the <see cref="IPrincipal"/> from cache.</returns>
        /// <remarks></remarks>
        TToken SavePrincipal<TToken>(IPrincipal principal) where TToken : SecurityToken;

        /// <summary>
        /// Updates the cached principal associated with the specified <see cref="SecurityToken"/>, using the application's <see cref="ICacheManager"/>.
        /// </summary>
        /// <param name="principal">The cached <see cref="IPrincipal"/> instance.</param>
        /// <param name="token">The <see cref="SecurityToken"/> to associate with <paramref name="principal"/>.</param>
        /// <remarks></remarks>
        void SavePrincipal(IPrincipal principal, SecurityToken token);

        /// <summary>
        /// Expires the principal associated with the specified <see cref="SecurityToken"/>.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <remarks></remarks>
        void ExpirePrincipal(SecurityToken token);

        /// <summary>
        /// Gets the cached <see cref="IPrincipal"/> instance associated with the specified <see cref="SecurityToken"/>.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>Instance of <see cref="IPrincipal"/> if found in cache, else null.</returns>
        /// <remarks></remarks>
        IPrincipal GetPrincipal(SecurityToken token);

        /// <summary>
        /// Gets the cached <typeparamref name="TPrincipal"/> instance associated with the specified <see cref="SecurityToken"/>.
        /// </summary>
        /// <typeparam name="TPrincipal">The type of the principal.</typeparam>
        /// <param name="token">The token.</param>
        /// <returns>Instance of <typeparamref name="TPrincipal"/> if found in cache, else null.</returns>
        /// <remarks></remarks>
        TPrincipal GetPrincipal<TPrincipal>(SecurityToken token) where TPrincipal : class, IPrincipal;
    }
}