// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitOfWorkController.cs">
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
//   Defines a controller for management of active units of work.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading;

namespace NContext.Persistence.EntityFramework
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