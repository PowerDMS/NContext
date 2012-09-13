// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SecurityConfiguration.cs" company="Waking Venture, Inc.">
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
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.Security
{
    using System;

    /// <summary>
    /// Defines application security configuration settings.
    /// </summary>
    public class SecurityConfiguration
    {
        private readonly DateTimeOffset _TokenAbsoluteExpiration;

        private readonly TimeSpan _TokenSlidingExpiration;

        private readonly TimeSpan _TokenInitialLifespan;

        public SecurityConfiguration(DateTimeOffset tokenAbsoluteExpiration, TimeSpan tokenSlidingExpiration, TimeSpan tokenInitialLifespan)
        {
            _TokenAbsoluteExpiration = tokenAbsoluteExpiration;
            _TokenSlidingExpiration = tokenSlidingExpiration;
            _TokenInitialLifespan = tokenInitialLifespan;
        }

        /// <summary>
        /// Gets the token absolute expiration.
        /// </summary>
        /// <remarks></remarks>
        public DateTimeOffset TokenAbsoluteExpiration
        {
            get
            {
                return _TokenAbsoluteExpiration;
            }
        }

        /// <summary>
        /// Gets the token sliding expiration.
        /// </summary>
        /// <remarks></remarks>
        public TimeSpan TokenSlidingExpiration
        {
            get
            {
                return _TokenSlidingExpiration;
            }
        }

        /// <summary>
        /// Gets the token initial lifetime.
        /// </summary>
        /// <remarks></remarks>
        public TimeSpan TokenInitialLifespan
        {
            get
            {
                return _TokenInitialLifespan;
            }
        }
    }
}