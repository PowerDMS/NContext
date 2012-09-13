// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThreadLocalAmbientContextManager.cs" company="Waking Venture, Inc.">
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
    using System.Threading;

    /// <summary> 
    /// Defines an ambient-context manager for per-thread storage. Each thread will 
    /// maintain its own <see cref="AmbientUnitOfWorkDecorator"/> stack.
    /// </summary>
    /// <remarks></remarks>
    public class ThreadLocalAmbientContextManager : AmbientContextManagerBase
    {
        private static readonly ThreadLocal<Stack<AmbientUnitOfWorkDecorator>> _AmbientUnitsOfWork =
            new ThreadLocal<Stack<AmbientUnitOfWorkDecorator>>(() => new Stack<AmbientUnitOfWorkDecorator>());

        public override Boolean AmbientExists
        {
            get
            {
                return _AmbientUnitsOfWork.IsValueCreated && _AmbientUnitsOfWork.Value.Count > 0;
            }
        }

        protected override Stack<AmbientUnitOfWorkDecorator> AmbientUnitsOfWork
        {
            get
            {
                return _AmbientUnitsOfWork.Value;
            }
        }
    }
}