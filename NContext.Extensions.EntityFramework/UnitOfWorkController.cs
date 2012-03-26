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
        private static readonly ThreadLocal<Stack<Tuple<Int32, IUnitOfWork>>> _AmbientUnitsOfWork = 
            new ThreadLocal<Stack<Tuple<Int32, IUnitOfWork>>>(() => new Stack<Tuple<Int32, IUnitOfWork>>());

        /// <summary>
        /// Gets the ambient <see cref="IUnitOfWork"/>.
        /// </summary>
        /// <value>The ambient unit of work.</value>
        /// <remarks></remarks>
        public static IUnitOfWork AmbientUnitOfWork
        {
            get
            {
                return _AmbientUnitsOfWork.Value.Count > 0
                    ? _AmbientUnitsOfWork.Value.Peek().Item2
                    : null;
            }
        }

        /// <summary>
        /// Adds the unit of work.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <remarks></remarks>
        public static void AddUnitOfWork(IUnitOfWork unitOfWork)
        {
            _AmbientUnitsOfWork.Value.Push(new Tuple<Int32, IUnitOfWork>(_AmbientUnitsOfWork.Value.Count + 1, unitOfWork));
        }

        /// <summary>
        /// Retains this instance.
        /// </summary>
        /// <remarks></remarks>
        public static void Retain()
        {
            var tuple = _AmbientUnitsOfWork.Value.Pop();
            var retainCount = tuple.Item1;
            var uow = tuple.Item2;

            _AmbientUnitsOfWork.Value.Push(new Tuple<Int32, IUnitOfWork>(retainCount + 1, uow));
        }

        /// <summary>
        /// Disposes the unit of work.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Boolean DisposeUnitOfWork()
        {
            var uow = _AmbientUnitsOfWork.Value.Pop();
            if (uow.Item1 > 1)
            {
                _AmbientUnitsOfWork.Value.Push(new Tuple<Int32, IUnitOfWork>(uow.Item1 - 1, uow.Item2));

                return false;
            }

            return true;
        }
    }
}