// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenericToken.cs">
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