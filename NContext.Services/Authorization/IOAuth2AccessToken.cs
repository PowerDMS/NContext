// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOAuth2AccessToken.cs">
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

// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Reverb.PowerDms.Application.Services.Authorization
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOAuth2AccessToken : IComparable<IOAuth2AccessToken>
    {
        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <remarks></remarks>
        String AccessToken { get; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <remarks></remarks>
        String Type { get; }

        /// <summary>
        /// Gets the expires.
        /// </summary>
        /// <remarks></remarks>
        Int32 ExpiresIn { get; }

        /// <summary>
        /// Gets the refresh token.
        /// </summary>
        /// <remarks></remarks>
        String RefreshToken { get; }

        /// <summary>
        /// Gets the scope.
        /// </summary>
        /// <remarks></remarks>
        String Scope { get; }
    }
}