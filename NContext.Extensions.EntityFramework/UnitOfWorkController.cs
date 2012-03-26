// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitOfWorkController.cs">
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
//   Defines a controller for management of active units of work.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading;

namespace NContext.Extensions.EntityFramework
{
    /// <summary>
    /// Defines a controller for management of active units of work.
    /// </summary>
    /// <remarks></remarks>
    internal static class UnitOfWorkController
    {
        private static readonly ThreadLocal<Stack<AmbientUnitOfWork>> _AmbientUnitsOfWork =
            new ThreadLocal<Stack<AmbientUnitOfWork>>(() => new Stack<AmbientUnitOfWork>());

        /// <summary>
        /// Gets the ambient <see cref="IEfUnitOfWork"/>.
        /// </summary>
        /// <value>The ambient unit of work.</value>
        /// <remarks></remarks>
        public static IEfUnitOfWork AmbientUnitOfWork
        {
            get
            {
                return _AmbientUnitsOfWork.Value.Count > 0
                    ? _AmbientUnitsOfWork.Value.Peek().UnitOfWork
                    : null;
            }
        }

        /// <summary>
        /// Adds the unit of work.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <remarks></remarks>
        public static void AddUnitOfWork(IEfUnitOfWork unitOfWork)
        {
            _AmbientUnitsOfWork.Value.Push(new AmbientUnitOfWork(unitOfWork));
        }

        /// <summary>
        /// Retains this instance.
        /// </summary>
        /// <remarks></remarks>
        public static void Retain()
        {
            var ambientUnitOfWork = _AmbientUnitsOfWork.Value.Peek();
            ambientUnitOfWork.Increment();
        }

        /// <summary>
        /// Disposes the unit of work.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Boolean DisposeUnitOfWork()
        {
            var ambientUnitOfWork = _AmbientUnitsOfWork.Value.Peek();
            if (ambientUnitOfWork.ActiveSessions > 1)
            {
                ambientUnitOfWork.Decrement();

                return false;
            }

            _AmbientUnitsOfWork.Value.Pop();

            return true;
        }
    }
}