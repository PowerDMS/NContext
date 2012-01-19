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
//   Defines a generic implementation of IToken.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

using Microsoft.Practices.EnterpriseLibrary.Security;

namespace NContext.EnterpriseLibrary.Services.Authorization
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