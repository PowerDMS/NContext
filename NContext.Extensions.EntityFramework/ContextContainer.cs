// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextContainer.cs">
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
//   Defines an implementation of IContextContainer which is responsible for ensuring that only one instance 
//   of a given <see cref="DbContext"/> exists within a <see cref="IUnitOfWork"/>, per thread.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.Entity;

using Microsoft.Practices.ServiceLocation;

namespace NContext.Extensions.EntityFramework
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