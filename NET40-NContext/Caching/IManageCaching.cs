// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IManageCaching.cs" company="Waking Venture, Inc.">
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

namespace NContext.Caching
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Caching;

    using NContext.Configuration;

    /// <summary>
    /// Defines a contract for managing the application's cache provider.
    /// </summary>
    public interface IManageCaching : IApplicationComponent
    {
        /// <summary>
        /// Gets the cache providers.
        /// </summary>
        /// <remarks></remarks>
        IEnumerable<ObjectCache> Providers { get; }

        /// <summary>
        /// Gets the provider by unique name.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <returns>ObjectCache.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">providerName;There is no cache provider registered with name: providerName</exception>
        ObjectCache GetProvider(String providerName);

        /// <summary>
        /// Gets the provider by unique name.
        /// </summary>
        /// <typeparam name="TProvider">The type of the cache provider.</typeparam>
        /// <param name="providerName">Name of the provider.</param>
        /// <returns>ObjectCache.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">providerName;There is no cache provider registered with name: providerName</exception>
        /// <exception cref="System.InvalidOperationException">
        /// Occurs when the cache provider associated with <paramref name="providerName"/> 
        /// is not of type <typeparamref name="TProvider"/>
        /// </exception>
        TProvider GetProvider<TProvider>(String providerName) where TProvider : ObjectCache;
    }
}