// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IResponseTransferObject.cs">
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
//   Defines an abstract, composable, response-transfer-object.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace NContext.Application.Dto
{
    /// <summary>
    /// Defines an abstract, composable, response-transfer-object.
    /// </summary>
    /// <typeparam name="T">The type of object.</typeparam>
    public abstract class ResponseTransferObjectBase<T> : IDisposable
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseTransferObjectBase{T}"/> class.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <remarks></remarks>
        protected ResponseTransferObjectBase(Error error)
            : this(new List<Error> { error })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseTransferObjectBase{T}"/> class.
        /// </summary>
        /// <param name="errors">The errors.</param>
        /// <remarks></remarks>
        protected ResponseTransferObjectBase(IEnumerable<Error> errors)
            : this(null, errors)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseTransferObjectBase{T}"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <remarks></remarks>
        protected ResponseTransferObjectBase(T data)
            : this(new List<T> { data }, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseTransferObjectBase{T}"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <remarks></remarks>
        protected ResponseTransferObjectBase(IEnumerable<T> data)
            : this(data, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseTransferObjectBase{T}"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="errors">The errors.</param>
        /// <remarks></remarks>
        private ResponseTransferObjectBase(IEnumerable<T> data, IEnumerable<Error> errors)
        {
            Data = (data != null) ? data.ToList() : Enumerable.Empty<T>();
            Errors = errors ?? Enumerable.Empty<Error>();
        }

        #endregion

        /// <summary>
        /// Gets the <typeparam name="T"/> data.
        /// </summary>
        public IEnumerable<T> Data { get; private set; }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <remarks></remarks>
        public IEnumerable<Error> Errors { get; private set; }

        /// <summary>
        /// Binds the specified <see cref="IEnumerable{T}"/> into the function which returns the specified <see cref="ResponseTransferObjectBase{T}"/>.
        /// </summary>
        /// <typeparam name="T2">The type of the next <see cref="T"/> to return.</typeparam>
        /// <param name="func">The func.</param>
        /// <returns>Instance of <see cref="ResponseTransferObjectBase{T}"/>.</returns>
        /// <remarks></remarks>
        public abstract ResponseTransferObjectBase<T2> Bind<T2>(Func<IEnumerable<T>, ResponseTransferObjectBase<T2>> func);

        /// <summary>
        /// A combination of <see cref="Bind{T2}"/> and <see cref="ResponseTransferObjectBase{T}.Catch"/>. 
        /// It will invoke data function if there is any data, or errors function if there any errors exist.
        /// </summary>
        /// <typeparam name="T2">The type of the next data transfer object to return.</typeparam>
        /// <param name="data">The function to call if there is data and there are no errors.</param>
        /// <param name="errors">The function to call if there are any errors.</param>
        /// <returns>Instance of <see cref="ResponseTransferObjectBase{T}"/>.</returns>
        public abstract ResponseTransferObjectBase<T2> Either<T2>(Func<IEnumerable<T>, ResponseTransferObjectBase<T2>> data, 
                                                                  Func<IEnumerable<Error>, ResponseTransferObjectBase<T2>> errors);

        /// <summary>
        /// Invokes the specified action if there are no errors or validation results.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <remarks></remarks>
        public virtual void Do(Action<IEnumerable<T>> action)
        {
            if (Data.Any() && !Errors.Any())
            {
                action.Invoke(Data);
            }
        }

        /// <summary>
        /// Invokes the specified action if there are any errors. 
        /// Returns the current <see cref="ResponseTransferObjectBase{T}"/>.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The current <see cref="ResponseTransferObjectBase{T}"/> instance.</returns>
        /// <remarks></remarks>
        public virtual ResponseTransferObjectBase<T> Catch(Action<IEnumerable<Error>> action)
        {
            if (Errors.Any())
            {
                action(Errors);
            }

            return this;
        }
        
        #region Implementation of IDisposable

        protected Boolean _IsDisposed;

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the <see cref="ResponseTransferObjectBase{T}"/> is reclaimed by garbage collection.
        /// </summary>
        /// <remarks></remarks>
        ~ResponseTransferObjectBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposeManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(Boolean disposeManagedResources)
        {
            if (_IsDisposed)
            {
                return;
            }

            if (disposeManagedResources)
            {
            }

            _IsDisposed = true;
        }

        #endregion
    }
}