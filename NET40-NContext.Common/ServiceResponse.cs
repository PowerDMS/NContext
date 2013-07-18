// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceResponse.cs" company="Waking Venture, Inc.">
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

namespace NContext.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines a service response implementation of <see cref="IResponseTransferObject{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <remarks></remarks>
    [DataContract(Name = "ServiceResponseOf{0}")]
    public class ServiceResponse<T> : IResponseTransferObject<T>
    {
        private T _Data;

        private IEnumerable<Error> _Errors;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponse{T}"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <remarks></remarks>
        public ServiceResponse(T data)
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
            : this(default(T), errors)
        {
        }

        /// <summary>
        /// For deserialization purposes only.
        /// </summary>
        /// <remarks></remarks>
        protected ServiceResponse()
        {
            Data = default(T);
            Errors = Enumerable.Empty<Error>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IResponseTransferObject{T}"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="errors">The errors.</param>
        /// <remarks></remarks>
        private ServiceResponse(T data, IEnumerable<Error> errors)
        {
            Errors = errors ?? Enumerable.Empty<Error>();

            if (data is IEnumerable)
            {
                Data = CreateMaterializedEnumerable(data);
            }
            else
            {
                Data = (data != null) ? data : default(T);
            }
        }
        
        /// <summary>
        /// Gets an empty <see cref="ServiceResponse{T}"/>.
        /// </summary>
        /// <remarks></remarks>
        public static IResponseTransferObject<T> Empty
        {
            get
            {
                return new ServiceResponse<T>(default(T));
            }
        }

        /// <summary>
        /// Gets or sets the <typeparam name="T"/> data.
        /// </summary>
        [DataMember(Order = 1)]
        public T Data
        {
            get
            {
                return _Data;
            }
            protected set
            {
                _Data = value;
            }
        }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <remarks></remarks>
        [DataMember(Order = 2)]
        public IEnumerable<Error> Errors
        {
            get
            {
                return _Errors;
            }
            protected set
            {
                _Errors = value;
            }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ServiceResponse{T}"/> to <see cref="System.Boolean"/>.
        /// </summary>
        /// <param name="serviceResponse">The service response.</param>
        /// <returns>The result of the conversion.</returns>
        /// <remarks></remarks>
        public static implicit operator Boolean(ServiceResponse<T> serviceResponse)
        {
            if (serviceResponse == null)
            {
                return false;
            }

            if (serviceResponse.Errors.Any())
            {
                return false;
            }

            if (typeof(T) == typeof(Boolean))
            {
                return Convert.ToBoolean(serviceResponse.Data);
            }

            return serviceResponse.Data != null;
        }

        private static T CreateMaterializedEnumerable<T>(T data)
        {
            var dataType = data.GetType();
            if (typeof(ICollection).IsAssignableFrom(dataType))
            {
                return data;
            }

            var innerType = dataType.GetGenericArguments()[0];
            var listType = typeof(List<>).MakeGenericType(innerType);

            return (T)Activator.CreateInstance(listType, data);
        }

        #region Implementation of IDisposable

        protected Boolean IsDisposed { get; set; }

        /// <summary>
        /// Finalizes an instance of the <see cref="ServiceResponse{T}" /> class.
        /// </summary>
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
            if (IsDisposed)
            {
                return;
            }

            if (disposeManagedResources)
            {
            }

            IsDisposed = true;
        }

        #endregion
    }
}