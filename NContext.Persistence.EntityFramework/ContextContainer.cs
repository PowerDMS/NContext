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
    public class ContextContainer : IContextContainer
    {
        #region Fields

        private readonly Dictionary<Type, DbContext> _Contexts = new Dictionary<Type, DbContext>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        protected internal ContextContainer()
        {
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

        #endregion

        #region Implementation of IContextContainer

        /// <summary>
        /// Gets the application's default context.
        /// </summary>
        /// <returns>Instance of the application's default DbContext.</returns>
        /// <remarks></remarks>
        public DbContext GetDefaultContext()
        {
            // TODO: (DG) Find a better way of getting the MappedToType Type object given a ServiceType and Key.
            // Currently we have to resolve an instance even though we may already have it in our dictionary.
            // http://commonservicelocator.codeplex.com/workitem/14336

            var defaultContext = ServiceLocator.Current.GetInstance<DbContext>("default");
            if (defaultContext == null)
            {
                throw new InvalidOperationException(
                    @"No default context has been set by the application. The default context must be registered using your preferred dependency injection container using typeof 'DbContext' with a named key 'default'.");
            }

            var defaultContextType = defaultContext.GetType();
            if (_Contexts.ContainsKey(defaultContextType))
            {
                return _Contexts[defaultContextType];
            }

            _Contexts.Add(defaultContextType, defaultContext);

            return defaultContext;
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
            if (_Contexts.ContainsKey(typeof(TContext)))
            {
                return _Contexts[typeof(TContext)] as TContext;
            }

            var context = ServiceLocator.Current.GetInstance<TContext>();
            if (context == null)
            {
                throw new ArgumentException(String.Format("Context type '{0}' counld not be found and is not registered for service location.", typeof(TContext).Name));
            }

            _Contexts.Add(typeof(TContext), context);

            return context;
        }

        /// <summary>
        /// Gets the context from the application's service locator.
        /// </summary>
        /// <param name="registeredNameForServiceLocation">The context's registered name with the dependency injection container.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public DbContext GetContextFromServiceLocation(String registeredNameForServiceLocation)
        {
            var context = ServiceLocator.Current.GetInstance<DbContext>(registeredNameForServiceLocation);
            if (context == null)
            {
                throw new ArgumentException("Context type is not registered for service location.");
            }

            _Contexts.Add(context.GetType(), context);

            return context;
        }

        #endregion
    }
}