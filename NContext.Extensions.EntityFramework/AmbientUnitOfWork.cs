// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmbientUnitOfWork.cs">
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
//   Defines a unit of work in active use and keeps track of the number of sessions associated with it.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace NContext.Extensions.EntityFramework
{
    /// <summary>
    /// Defines a unit of work in active use and keeps track of the number of sessions associated with it.
    /// </summary>
    /// <remarks></remarks>
    internal struct AmbientUnitOfWork
    {
        private readonly IEfUnitOfWork _UnitOfWork;

        private Int32 _ActiveSessions;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmbientUnitOfWork"/> struct.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <remarks></remarks>
        public AmbientUnitOfWork(IEfUnitOfWork unitOfWork)
            : this()
        {
            _ActiveSessions = 1;
            _UnitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the number of active sessions for the <seealso cref="UnitOfWork"/> instance.
        /// </summary>
        /// <remarks></remarks>
        public Int32 ActiveSessions
        {
            get
            {
                return _ActiveSessions;
            }
        }

        /// <summary>
        /// Gets the unit of work.
        /// </summary>
        /// <remarks></remarks>
        public IEfUnitOfWork UnitOfWork
        {
            get
            {
                return _UnitOfWork;
            }
        }

        /// <summary>
        /// Decrements the active sessions for this <seealso cref="UnitOfWork"/> instance.
        /// </summary>
        /// <remarks></remarks>
        public void Decrement()
        {
            _ActiveSessions -= 1;
        }

        /// <summary>
        /// Increments the active sessions for this <seealso cref="UnitOfWork"/> instance.
        /// </summary>
        /// <remarks></remarks>
        public void Increment()
        {
            _ActiveSessions += 1;
        }
    }
}