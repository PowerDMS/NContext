// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenericToken.cs">
//   Copyright (c) 2012
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
//   Defines a generic SecurityToken using a Guid as a token identifier.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;

using NContext.Dto;

namespace NContext.Security
{
    /// <summary>
    /// Defines a generic <see cref="SecurityToken"/> using a <see cref="Guid"/>
    /// as a token identifier.
    /// </summary>
    public class GuidToken : SecurityToken, IToken
    {
        #region Fields

        private readonly Guid _Id;

        private readonly DateTime _ValidFrom;

        private readonly DateTime _ValidTo;

        private readonly ReadOnlyCollection<SecurityKey> _SecurityKeys;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.IdentityModel.Tokens.SecurityToken"/> class.
        /// </summary>
        /// <remarks></remarks>
        public GuidToken()
            : this(Guid.NewGuid())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuidToken"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <remarks></remarks>
        public GuidToken(Guid id)
        {
            _Id = id;
            _ValidFrom = DateTime.UtcNow;
            _ValidTo = DateTimeOffset.MaxValue.UtcDateTime;
            _SecurityKeys = new ReadOnlyCollection<SecurityKey>(new List<SecurityKey>());
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

        #region Implementation of IToken

        public String Value
        {
            get
            {
                return _Id.ToString();
            }
        }

        #endregion
    }
}