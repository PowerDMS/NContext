// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceResponse.cs" company="Waking Venture, Inc.">
//   Copyright (c) 2014 Waking Venture, Inc.
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
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
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

        private Error _Error;
        
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
            : this(default(T), error)
        {
        }
        
        /// <summary>
        /// For deserialization purposes only.
        /// </summary>
        /// <remarks></remarks>
        protected ServiceResponse()
        {
            Data = default(T);
            Error = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IResponseTransferObject{T}"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="errors">The errors.</param>
        /// <remarks></remarks>
        private ServiceResponse(T data, Error error)
        {
            Error = error;

            var materializedData = MaterializeDataIfNeeded(data);
            Data = (materializedData == null) ? default(T) : materializedData;
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
        public Error Error
        {
            get
            {
                return _Error;
            }
            protected set
            {
                _Error = value;
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

            if (serviceResponse.Error != null)
            {
                return false;
            }

            if (typeof(T) == typeof(Boolean))
            {
                return Convert.ToBoolean(serviceResponse.Data);
            }

            return serviceResponse.Data != null;
        }

        private static T MaterializeDataIfNeeded(T data)
        {
            if (typeof(T).GetTypeInfo().IsValueType || data == null)
            {
                return data;
            }

            var dataType = data.GetType();
            var dataTypeInfo = dataType.GetTypeInfo();
            if (!(data is IEnumerable) ||
                !dataTypeInfo.IsGenericType || 
                IsDictionary(dataType))
            {
                return data;
            }

            if (!IsQueryable(dataType) && !dataTypeInfo.IsNestedPrivate)
            {
                return data;
            }

            // Get the last generic argument.
            // .NET has several internal iterable types in LINQ that have multiple generic
            // arguments.  The last is reserved for the actual type used for projection.
            // ex. WhereSelectArrayIterator, WhereSelectEnumerableIterator, WhereSelectListIterator
            var genericType = dataType.GenericTypeArguments.Last();
            if (dataType.GetGenericTypeDefinition() == typeof(Collection<>))
            {
                var collectionType = typeof(Collection<>).MakeGenericType(genericType);
                return (T)collectionType.CreateInstance(data);
            }

            var listType = typeof(List<>).MakeGenericType(genericType);
            return (T)listType.CreateInstance(data);
        }

        private static Boolean IsDictionary(Type type)
        {
            if (type == null) return false;

            var typeInfo = type.GetTypeInfo();

            return (typeInfo.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>)) ||
                    typeInfo.ImplementedInterfaces
                        .Any(interfaceType => interfaceType.GetTypeInfo().IsGenericType && 
                             interfaceType.GetGenericTypeDefinition() == typeof(IDictionary<,>));
        }

        private static Boolean IsQueryable(Type type)
        {
            if (type == null) return false;

            var typeInfo = type.GetTypeInfo();

            return
                (typeInfo.IsGenericType && type.GetGenericTypeDefinition() == typeof(IQueryable<>)) ||
                 typeInfo.ImplementedInterfaces
                     .Any(interfaceType => interfaceType.GetTypeInfo().IsGenericType && 
                          interfaceType.GetGenericTypeDefinition() == typeof(IQueryable<>));
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