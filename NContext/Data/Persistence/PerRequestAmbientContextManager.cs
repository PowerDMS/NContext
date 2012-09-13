// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PerRequestAmbientContextManager.cs" company="Waking Venture, Inc.">
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

namespace NContext.Data.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    /// <summary>
    /// Defines an ambient-context manager for web applications. Each web request 
    /// will maintain its own <see cref="AmbientUnitOfWorkDecorator"/> stack.
    /// </summary>
    public class PerRequestAmbientContextManager : AmbientContextManagerBase
    {
        protected const String AmbientUnitsOfWorkKey = @"AmbientUnitsOfWork";

        public PerRequestAmbientContextManager()
        {
            if (String.IsNullOrWhiteSpace(HttpRuntime.AppDomainAppVirtualPath))
            {
                throw new ApplicationException("PerRequestAmbientContextManager can only be used in a web application.");
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
        
        protected override Stack<AmbientUnitOfWorkDecorator> AmbientUnitsOfWork
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    throw new InvalidOperationException("Cannot retrieve ambient units of work without an active HttpContext.");
                }

                var ambientUnitsOfWork = HttpContext.Current.Items[AmbientUnitsOfWorkKey] as Stack<AmbientUnitOfWorkDecorator>;
                if (ambientUnitsOfWork == null)
                {
                    HttpContext.Current.Items[AmbientUnitsOfWorkKey] = ambientUnitsOfWork = new Stack<AmbientUnitOfWorkDecorator>();
                }

                return ambientUnitsOfWork;
            }
        }
    }
}