// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenericToken.cs">
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
//   Defines a generic implementation of IToken.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

using Microsoft.Practices.EnterpriseLibrary.Security;

namespace NContext.Application.Services.Authorization
{
    /// <summary>
    /// Defines a generic implementation of <see cref="IToken"/>.
    /// </summary>
    /// <remarks></remarks>
    public sealed class GenericToken : IToken
    {
        #region Fields

        private readonly String _Value;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericToken"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <remarks></remarks>
        public GenericToken(String value)
        {
            _Value = value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the token contents as a string.
        /// </summary>
        /// <remarks></remarks>
        public String Value
        {
            get
            {
                return _Value;
            }
        }

        #endregion
    }
}