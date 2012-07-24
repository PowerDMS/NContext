// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextContainer.cs">
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
//   Defines an implementation of IContextContainer which is responsible for ensuring that only one instance 
//   of a given <see cref="DbContext"/> exists within a <see cref="IEfUnitOfWork"/>, per thread.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.Extensions.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    /// <summary>
    /// Defines an implementation of IContextContainer which is responsible for ensuring that only one instance 
    /// of a given <see cref="DbContext"/> exists within a <see cref="IEfUnitOfWork"/>.
    /// </summary>
    /// <remarks></remarks>
    public class ContextContainer : IContextContainer
    {
        #region Fields

        private readonly Guid _Id;

        private readonly Dictionary<String, DbContext> _Contexts = new Dictionary<String, DbContext>();

        #endregion

        #region Constructors
        
        protected internal ContextContainer()
        {
            _Id = Guid.NewGuid();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets all contexts.
        /// </summary>
        public IEnumerable<DbContext> Contexts
        {
            get { return _Contexts.Values; }
        }

        public Guid Id
        {
            get
            {
                return _Id;
            }
        }

        #endregion

        #region Implementation of IContextContainer

        public void Add(DbContext dbContext)
        {
            if (Contains(dbContext.GetType().Name))
            {
                return;
            }

            _Contexts.Add(dbContext.GetType().Name, dbContext);
        }

        public void Add(String key, DbContext dbContext)
        {
            _Contexts.Add(key, dbContext);
        }

        public Boolean Contains(String key)
        {
            return _Contexts.ContainsKey(key);
        }

        /// <summary>
        /// Gets or creates the <typeparamref name="TContext"/> context.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <returns>Instance of <typeparamref name="TContext"/>.</returns>
        /// <remarks></remarks>
        public TContext GetContext<TContext>() 
            where TContext : DbContext
        {
            if (_Contexts.ContainsKey(typeof(TContext).Name))
            {
                return _Contexts[typeof(TContext).Name] as TContext;
            }

            return null;
        }

        public DbContext GetContext(Type contextType)
        {
            return GetContext(contextType.Name);
        }

        public DbContext GetContext(String key)
        {
            return Contains(key) ? _Contexts.Single(c => c.Key == key).Value : null;
        }

        #endregion
    }
}