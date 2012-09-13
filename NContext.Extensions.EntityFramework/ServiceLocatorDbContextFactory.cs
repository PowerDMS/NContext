// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceLocatorDbContextFactory.cs" company="Waking Venture, Inc.">
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
    using System.Data.Entity;

    using Microsoft.Practices.ServiceLocation;

    public class ServiceLocatorDbContextFactory : IDbContextFactory
    {
        public DbContext Create()
        {
            return GetContextFromServiceLocation<DbContext>(null);
        }

        public DbContext Create(String key)
        {
            return GetContextFromServiceLocation<DbContext>(key);
        }

        public TDbContext Create<TDbContext>() where TDbContext : DbContext
        {
            return GetContextFromServiceLocation<TDbContext>(null);
        }

        protected TDbContext GetContextFromServiceLocation<TDbContext>(String registeredNameForServiceLocation) where TDbContext : DbContext
        {
            var context = String.IsNullOrWhiteSpace(registeredNameForServiceLocation)
                              ? ServiceLocator.Current.GetInstance<TDbContext>()
                              : ServiceLocator.Current.GetInstance<TDbContext>(registeredNameForServiceLocation);

            if (context == null)
            {
                throw new ArgumentException("Context is not registered for service location.");
            }

            return context;
        }
    }
}