// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IContextContainer.cs">
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
//   Defines a contract which aides in management of a distinct collection of <see cref="DbContext"/>s 
//   based on type. It is responsible for ensuring that only one instance of a given context 
//   exists at any given point, per thread. This is done through Unity LifetimeManagers.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace NContext.Extensions.EntityFramework
{
    /// <summary>
    /// Defines a contract which aides in management of a distinct collection of <see cref="DbContext"/>s 
    /// based on type. It is responsible for ensuring that only one instance of a given context 
    /// exists at any given point, per thread. This is done through Unity LifetimeManagers.
    /// </summary>
    public interface IContextContainer
    {
        /// <summary>
        /// Gets all contexts.
        /// </summary>
        IEnumerable<DbContext> Contexts { get; }

        /// <summary>
        /// Gets the application's default context.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        DbContext GetDefaultContext();

        /// <summary>
        /// Gets or creates the <typeparamref name="TContext"/> context.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <returns>Instance of <typeparamref name="TContext"/>.</returns>
        /// <remarks></remarks>
        TContext GetContext<TContext>() where TContext : DbContext;

        /// <summary>
        /// Gets the context from the application's service locator.
        /// </summary>
        /// <param name="registeredNameForServiceLocation">The context's registered name with the dependency injection container.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        DbContext GetContextFromServiceLocation(String registeredNameForServiceLocation);
    }
}