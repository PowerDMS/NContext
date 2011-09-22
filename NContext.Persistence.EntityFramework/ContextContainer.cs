// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextContainer.cs">
//   This file is part of NContext.
//
//   NContext is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or any later version.
//
//   NContext is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with NContext.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
//
// <summary>
//   Defines an implementation of IContextContainer which is responsible for ensuring that only one instance 
//   of a given <see cref="DbContext"/> exists within a <see cref="IUnitOfWork"/>, per thread.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.Entity;

using Microsoft.Practices.ServiceLocation;

namespace NContext.Persistence.EntityFramework
{
    /// <summary>
    /// Defines an implementation of IContextContainer which is responsible for ensuring that only one instance 
    /// of a given <see cref="DbContext"/> exists within a <see cref="IUnitOfWork"/>, per thread.
    /// </summary>
    /// <remarks></remarks>
    internal class ContextContainer : IContextContainer
    {
        #region Fields

        private readonly Dictionary<Type, DbContext> _Contexts = new Dictionary<Type, DbContext>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets all contexts.
        /// </summary>
        public IEnumerable<DbContext> Contexts
        {
            get { return _Contexts.Values; }
        }

        #endregion

        #region Implementation of IContextContainer

        /// <summary>
        /// Gets the application's default context.
        /// </summary>
        /// <returns>Instance of the application's default DbContext.</returns>
        /// <remarks></remarks>
        public DbContext GetDefaultContext()
        {
            var defaultContext = ServiceLocator.Current.GetInstance<DbContext>("Default");
            if (defaultContext == null)
            {
                throw new InvalidOperationException(
                    @"No default context has been set by the application. The default context must be registered using your preferred DI container using typeof 'DbContext' with a named key 'Default'.");
            }

            return defaultContext;
        }

        /// <summary>
        /// Gets or creates the <typeparamref name="TContext"/> context.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <returns>Instance of <typeparamref name="TContext"/>.</returns>
        /// <remarks></remarks>
        public TContext GetContext<TContext>() where TContext : DbContext
        {
            if (_Contexts.ContainsKey(typeof(TContext)))
            {
                return _Contexts[typeof(TContext)] as TContext;
            }

            var context = ServiceLocator.Current.GetInstance<TContext>();
            if (context == null)
            {
                throw new ArgumentException("Context type is not registered for service location.");
            }

            _Contexts.Add(typeof(TContext), context);

            return context;
        }

        #endregion
    }
}