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
//
// <summary>
//   DESCRIPTION
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;

namespace NContext.Application.Security
{
    /// <summary>
    /// Defines a generic <see cref="SecurityToken"/> using a <see cref="Guid"/>
    /// as token identifier.
    /// </summary>
    public class GuidToken : SecurityToken
    {
        #region Fields

        private readonly Guid _Id;

        private readonly DateTime _ValidFrom;

        private readonly DateTime _ValidTo;

        private static readonly ReadOnlyCollection<SecurityKey> _SecurityKeys = 
            new ReadOnlyCollection<SecurityKey>(new List<SecurityKey>());

        #endregion

        #region Constructors

        public GuidToken()
        {
            _Id = Guid.NewGuid();
            _ValidFrom = DateTime.UtcNow;
            _ValidTo = DateTimeOffset.MaxValue.UtcDateTime;
        }

        #endregion

        #region Implementation of SecurityToken

        /// <summary>
        /// Gets a unique identifier of the security token.
        /// </summary>
        /// <returns>The unique identifier of the security token.</returns>
        /// <remarks></remarks>
        public override String Id
        {
            get
            {
                return _Id.ToString();
            }
        }

        /// <summary>
        /// Gets the cryptographic keys associated with the security token.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1"/> of type <see cref="T:System.IdentityModel.Tokens.SecurityKey"/> that contains the set of keys associated with the security token.</returns>
        /// <remarks></remarks>
        public override ReadOnlyCollection<SecurityKey> SecurityKeys
        {
            get
            {
                return _SecurityKeys;
            }
        }

        /// <summary>
        /// Gets the first instant in time at which this security token is valid.
        /// </summary>
        /// <returns>A <see cref="T:System.DateTime"/> that represents the instant in time at which this security token is first valid.</returns>
        /// <remarks></remarks>
        public override DateTime ValidFrom
        {
            get
            {
                return _ValidFrom;
            }
        }

        /// <summary>
        /// Gets the last instant in time at which this security token is valid.
        /// </summary>
        /// <returns>A <see cref="T:System.DateTime"/> that represents the last instant in time at which this security token is valid.</returns>
        /// <remarks></remarks>
        public override DateTime ValidTo
        {
            get
            {
                return _ValidTo;
            }
        }

        #endregion
    }
}