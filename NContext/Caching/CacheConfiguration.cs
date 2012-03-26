// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheConfiguration.cs">
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
//   Defines configuration settings for caching.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.Caching;

namespace NContext.Caching
{
    /// <summary>
    /// Defines configuration settings for caching.
    /// </summary>
    public class CacheConfiguration
    {
        #region Fields

        private readonly Lazy<ObjectCache> _Provider;

        private readonly String _RegionName;

        private readonly DateTimeOffset _AbsoluteExpiration;

        private readonly TimeSpan _SlidingExpiration;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheConfiguration"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="regionName">Name of the region.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        /// <param name="slidingExpiration">The sliding expiration.</param>
        /// <remarks></remarks>
        public CacheConfiguration(Lazy<ObjectCache> provider, String regionName, DateTimeOffset absoluteExpiration, TimeSpan slidingExpiration)
        {
            _Provider = provider;
            _RegionName = regionName;
            _AbsoluteExpiration = absoluteExpiration;
            _SlidingExpiration = slidingExpiration;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the provider.
        /// </summary>
        /// <remarks></remarks>
        public Lazy<ObjectCache> Provider
        {
            get
            {
                return _Provider;
            }
        }

        /// <summary>
        /// Gets the name of the region.
        /// </summary>
        /// <remarks></remarks>
        public String RegionName
        {
            get
            {
                return _RegionName;
            }
        }

        /// <summary>
        /// Gets the absolute expiration.
        /// </summary>
        /// <remarks></remarks>
        public DateTimeOffset AbsoluteExpiration
        {
            get
            {
                return _AbsoluteExpiration;
            }
        }

        /// <summary>
        /// Gets the sliding expiration.
        /// </summary>
        /// <remarks></remarks>
        public TimeSpan SlidingExpiration
        {
            get
            {
                return _SlidingExpiration;
            }
        }

        #endregion
    }
}