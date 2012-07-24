// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PerRequestUnitOfWork.cs">
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
//   DESCRIPTION
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.Data.Persistence
{
    using System;
    using System.Web;
    using System.Collections.Generic;

    /// <summary>
    /// Defines a per-request transaction lifetime manager for data persistence. 
    /// Each web request will maintain its own <see cref="AmbientUnitOfWork"/> stack.
    /// </summary>
    public class PerRequestTransactionManager : AmbientTransactionManagerBase
    {
        protected const String AmbientUnitsOfWorkKey = @"AmbientUnitsOfWork";

        public PerRequestTransactionManager()
        {
            if (String.IsNullOrWhiteSpace(HttpRuntime.AppDomainAppVirtualPath))
            {
                throw new ApplicationException("PerRequestTransactionManager can only be used in a web application scenario.");
            }
        }

        public override Boolean AmbientExists
        {
            get
            {
                return HttpContext.Current != null && 
                    HttpContext.Current.Items.Contains(AmbientUnitsOfWorkKey) && 
                    AmbientUnitsOfWork.Count > 0;
            }
        }
        
        protected override Stack<AmbientUnitOfWork> AmbientUnitsOfWork
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    throw new InvalidOperationException("Cannot retrieve ambient units of work without an active HttpContext.");
                }

                var unitsOfWork = HttpContext.Current.Items[AmbientUnitsOfWorkKey] as Stack<AmbientUnitOfWork>;
                if (unitsOfWork == null)
                {
                    HttpContext.Current.Items[AmbientUnitsOfWorkKey] = unitsOfWork = new Stack<AmbientUnitOfWork>();
                }

                return unitsOfWork;
            }
        }
    }
}