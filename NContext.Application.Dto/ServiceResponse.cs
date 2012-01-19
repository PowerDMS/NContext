// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceResponse.cs">
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
//   Defines a service response implementation of ResponseTransferObjectBase<T>.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace NContext.Application.Dto
{
    /// <summary>
    /// Defines a service response implementation of <see cref="IResponseTransferObject{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <remarks></remarks>
    [DataContract(Name = "ServiceResponseOf{0}")]
    public class ServiceResponse<T> : IResponseTransferObject<T>
    {
        #region Constructors
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponse{T}"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <remarks></remarks>
        public ServiceResponse(T data)
            : this(new List<T> { data }, null)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponse{T}"/> class.
        /// </summary>
        /// <param name="data">The response data.</param>
        /// <remarks></remarks>
        public ServiceResponse(IEnumerable<T> data)
            : this(data, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponse&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <remarks></remarks>
        public ServiceResponse(Error error)
            : this(new List<Error> { error })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponse&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="errors">The response errors.</param>
        /// <remarks></remarks>
        public ServiceResponse(IEnumerable<Error> errors)
            : this(null, errors)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IResponseTransferObject{T}"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="errors">The errors.</param>
        /// <remarks></remarks>
        private ServiceResponse(IEnumerable<T> data, IEnumerable<Error> errors)
        {
            Data = (data != null) ? data.ToList() : Enumerable.Empty<T>();
            Errors = errors ?? Enumerable.Empty<Error>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <typeparam name="T"/> data.
        /// </summary>
        [DataMember(Order = 1)]
        public IEnumerable<T> Data { get; private set; }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <remarks></remarks>
        [DataMember(Order = 2)]
        public IEnumerable<Error> Errors { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets an empty <see cref="ServiceResponse{T}"/>.
        /// </summary>
        /// <remarks></remarks>
        public static ServiceResponse<T> Empty
        {
            get
            {
                return new ServiceResponse<T>(Enumerable.Empty<T>());
            }
        }

        #endregion

        #region Implementation of IResponseTransferObject<T>

        /// <summary>
        /// Binds the specified <see cref="IEnumerable{T}"/> into the function which returns the specified <see cref="ServiceResponse{T}"/>.
        /// </summary>
        /// <typeparam name="T2">The type of the next <see cref="T"/> to return.</typeparam>
        /// <param name="func">The func.</param>
        /// <returns>Instance of <see cref="ServiceResponse{T}"/>.</returns>
        /// <remarks></remarks>
        public virtual IResponseTransferObject<T2> Bind<T2>(Func<IEnumerable<T>, IResponseTransferObject<T2>> func)
        {
            if (Data.Any())
            {
                return func.Invoke(Data);
            }

            if (Errors.Any())
            {
                return new ServiceResponse<T2>(Errors);
            }

            return ServiceResponse<T2>.Empty;
        }

        /// <summary>
        /// Invokes the specified action if there are any errors. 
        /// Returns the current <see cref="IResponseTransferObject{T}"/>.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The current <see cref="IResponseTransferObject{T}"/> instance.</returns>
        /// <remarks></remarks>
        public virtual IResponseTransferObject<T> Catch(Action<IEnumerable<Error>> action)
        {
            if (Errors.Any())
            {
                action.Invoke(Errors);
            }

            return this;
        }

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
        /// A combination of <see cref="Bind{T2}"/> and <see cref="IResponseTransferObject{T}.Catch"/>. 
        /// It will invoke data function if there is any data, or errors function if there any errors exist.
        /// </summary>
        /// <typeparam name="T2">The type of the next data transfer object to return.</typeparam>
        /// <param name="data">The function to call if there is data and there are no errors.</param>
        /// <param name="errors">The function to call if there are any errors.</param>
        /// <returns>Instance of <see cref="IResponseTransferObject{T}"/>.</returns>
        public virtual IResponseTransferObject<T2> Either<T2>(Func<IEnumerable<T>, IResponseTransferObject<T2>> data,
                                                              Func<IEnumerable<Error>, IResponseTransferObject<T2>> errors)
        {
            if (Data.Any())
            {
                return data.Invoke(Data);
            }

            if (Errors.Any())
            {
                return errors.Invoke(Errors);
            }

            return ServiceResponse<T2>.Empty;
        }

        /// <summary>
        /// Invokes the specified action if there is data. 
        /// Returns the current <see cref="IResponseTransferObject{T}"/>.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The current <see cref="IResponseTransferObject{T}"/> instance.</returns>
        /// <remarks></remarks>
        public virtual IResponseTransferObject<T> Let(Action<IEnumerable<T>> action)
        {
            if (Data.Any() && !Errors.Any())
            {
                action.Invoke(Data);
            }

            return this;
        }

        #endregion

        #region Implementation of IDisposable

        protected Boolean _IsDisposed;

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the <see cref="ServiceResponse&lt;T&gt;"/> is reclaimed by garbage collection.
        /// </summary>
        /// <remarks></remarks>
        ~ServiceResponse()
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