// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceResponse.cs">
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
    /// Defines a service response implementation of <see cref="ResponseTransferObjectBase{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <remarks></remarks>
    [DataContract(Name = "ServiceResponseOf{0}")]
    public class ServiceResponse<T> : ResponseTransferObjectBase<T>
    {
        #region Constructors
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponse{T}"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <remarks></remarks>
        public ServiceResponse(T data)
            : base(data)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponse{T}"/> class.
        /// </summary>
        /// <param name="data">The response data.</param>
        /// <remarks></remarks>
        public ServiceResponse(IEnumerable<T> data)
            : base(data)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponse&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="errors">The response errors.</param>
        /// <remarks></remarks>
        public ServiceResponse(IEnumerable<Error> errors)
            : base(errors)
        {
        }

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

        #region Implementation of ResponseTransferObject<T>

        /// <summary>
        /// Binds the specified <see cref="IEnumerable{T}"/> into the function which returns the specified <see cref="ServiceResponse{T}"/>.
        /// </summary>
        /// <typeparam name="T2">The type of the next <see cref="T"/> to return.</typeparam>
        /// <param name="func">The func.</param>
        /// <returns>Instance of <see cref="ServiceResponse{T}"/>.</returns>
        /// <remarks></remarks>
        public override ResponseTransferObjectBase<T2> Bind<T2>(Func<IEnumerable<T>, ResponseTransferObjectBase<T2>> func)
        {
            if (Data.Any())
            {
                return func(Data);
            }

            if (Errors.Any())
            {
                return new ServiceResponse<T2>(Errors);
            }

            return ServiceResponse<T2>.Empty;
        }

        /// <summary>
        /// A combination of <see cref="Bind{T2}"/> and <see cref="ResponseTransferObjectBase{T}.Catch"/>. 
        /// It will invoke data function if there is any data, or errors function if there any errors exist.
        /// </summary>
        /// <typeparam name="T2">The type of the next data transfer object to return.</typeparam>
        /// <param name="data">The function to call if there is data and there are no errors.</param>
        /// <param name="errors">The function to call if there are any errors.</param>
        /// <returns>Instance of <see cref="ResponseTransferObjectBase{T}"/>.</returns>
        public override ResponseTransferObjectBase<T2> Either<T2>(Func<IEnumerable<T>, ResponseTransferObjectBase<T2>> data, 
                                                                  Func<IEnumerable<Error>, ResponseTransferObjectBase<T2>> errors)
        {
            if (Errors.Any())
            {
                return errors(Errors);
            }

            if (Data.Any())
            {
                return data(Data);
            }

            return ServiceResponse<T2>.Empty;
        }

        #endregion
    }
}