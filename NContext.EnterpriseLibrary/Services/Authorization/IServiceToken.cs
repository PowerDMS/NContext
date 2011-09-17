// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IServiceToken.cs">
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
//   Defines a generic contract for web service token used in authentication.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

using Microsoft.Practices.EnterpriseLibrary.Security;

namespace Reverb.PowerDms.Application.Services.Authorization
{
    /// <summary>
    /// Defines a generic contract for web service token used in authentication.
    /// </summary>
    /// <remarks></remarks>
    public interface IServiceToken : IToken
    {
        /// <summary>
        /// Gets the token creation date.
        /// </summary>
        /// <remarks></remarks>
        DateTimeOffset Created { get; }

        /// <summary>
        /// Gets the token expiration date.
        /// </summary>
        /// <remarks></remarks>
        DateTimeOffset Expires { get; }
    }
}