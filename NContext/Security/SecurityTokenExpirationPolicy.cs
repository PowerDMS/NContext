// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SecurityTokenExpirationPolicy.cs" company="Waking Venture, Inc.">
//   Copyright (c) 2013 Waking Venture, Inc.
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
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.Security
{
    using System;

    public struct SecurityTokenExpirationPolicy
    {
        private readonly Boolean _Expires;

        private readonly Boolean _IsAbsolute;

        private readonly TimeSpan _ExpirationTime;

        /// <summary>
        /// Creates a policy for handling 
        /// </summary>
        /// <param name="expirationTime"></param>
        /// <param name="expirationIsAbsolute"></param>
        public SecurityTokenExpirationPolicy(TimeSpan expirationTime, Boolean expirationIsAbsolute)
        {
            _Expires = true;
            _ExpirationTime = expirationTime;
            _IsAbsolute = expirationIsAbsolute;
        }

        /// <summary>
        /// Gets whether security token expires or not.
        /// </summary>
        public Boolean Expires
        {
            get
            {
                return _Expires;
            }
        }

        /// <summary>
        /// Gets whether the <see cref="ExpirationTime"/> is used for absolute expiration or sliding. Default is Sliding.
        /// </summary>
        public Boolean IsAbsolute
        {
            get
            {
                return _IsAbsolute;
            }
        }

        public TimeSpan ExpirationTime
        {
            get
            {
                return _ExpirationTime;
            }
        }
    }
}