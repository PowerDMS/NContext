// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EndpointBinding.cs">
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
//   Defines an enumeration of service architecture types.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace NContext.Application.Services
{
    /// <summary>
    /// Defines an enumeration of service architecture types.
    /// </summary>
    [Flags]
    public enum EndpointBinding
    {
        /// <summary>
        /// Represents a REST service binding.
        /// </summary>
        Rest = 1 << 0,

        /// <summary>
        /// Represents a SOAP service binding.
        /// </summary>
        Soap = 1 << 1
    }
}
