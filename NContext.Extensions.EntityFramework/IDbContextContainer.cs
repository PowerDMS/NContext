// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDbContextContainer.cs" company="Waking Venture, Inc.">
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

namespace NContext.Extensions.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;

    /// <summary>
    /// Defines a contract which aides in management of a distinct collection of <see cref="DbContext"/>s 
    /// based on type. It is responsible for ensuring that only one instance of a given context 
    /// exists at any given time per ambient context.
    /// </summary>
    public interface IDbContextContainer : IDisposable
    {
        Guid Id { get; }

        /// <summary>
        /// Gets all DbContexts.
        /// </summary>
        IEnumerable<DbContext> Contexts { get; }

        void Add(DbContext dbContext);

        void Add(String key, DbContext dbContext);

        Boolean Contains(String key);

        /// <summary>
        /// Gets or creates the <typeparamref name="TContext"/> context.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <returns>Instance of <typeparamref name="TContext"/>.</returns>
        /// <remarks></remarks>
        TContext GetContext<TContext>() where TContext : DbContext;

        DbContext GetContext(Type contextType);

        DbContext GetContext(String key);
    }
}