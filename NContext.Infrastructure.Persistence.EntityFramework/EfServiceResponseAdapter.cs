// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EfServiceResponseAdapter.cs">
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
//   Defines an entity framework ServiceResponse<T> adapter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

using NContext.Application.Dto;
using NContext.Application.ErrorHandling;
using NContext.Application.Extensions;

namespace NContext.Infrastructure.Persistence.EntityFramework
{
    /// <summary>
    /// Defines an Entity Framework <see cref="ServiceResponse{T}"/> adapter.
    /// </summary>
    public class EfServiceResponseAdapter<T> : ServiceResponse<T>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponse{T}"/> class.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <remarks></remarks>
        public EfServiceResponseAdapter(ErrorBase error)
            : this(new List<ErrorBase> { error })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponse{T}"/> class.
        /// </summary>
        /// <param name="errors">The errors.</param>
        /// <remarks></remarks>
        public EfServiceResponseAdapter(IEnumerable<ErrorBase> errors)
            : base(TranslateErrorBaseToErrorCollection(errors))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponse{T}"/> class.
        /// </summary>
        /// <param name="validationResult">The validation result.</param>
        /// <remarks></remarks>
        public EfServiceResponseAdapter(DbEntityValidationResult validationResult)
            : this(new List<DbEntityValidationResult> { validationResult })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponse{T}"/> class.
        /// </summary>
        /// <param name="validationResults">The validation results.</param>
        /// <remarks></remarks>
        public EfServiceResponseAdapter(IEnumerable<DbEntityValidationResult> validationResults)
            : base(TranslateDbEntityValidationResultsToValidationErrors(validationResults))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EfServiceResponseAdapter&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <remarks></remarks>
        public EfServiceResponseAdapter(T data)
            : base(data)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EfServiceResponseAdapter&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <remarks></remarks>
        public EfServiceResponseAdapter(IEnumerable<T> data)
            : base(data)
        {
        }

        #endregion

        #region Methods

        private static IEnumerable<Error> TranslateErrorBaseToErrorCollection(IEnumerable<ErrorBase> errors)
        {
            return errors.ToMaybe()
                         .Bind(e => e.Select(error => new Error(error.ErrorType.Name, new List<String> { error.Message })).ToMaybe())
                         .FromMaybe(Enumerable.Empty<Error>());
        }

        private static IEnumerable<ValidationError> TranslateDbEntityValidationResultsToValidationErrors(IEnumerable<DbEntityValidationResult> validationResults)
        {
            return validationResults.ToMaybe()
                                    .Bind(results => 
                                          results.Select(validationResult => 
                                                         new ValidationError(validationResult.Entry.Entity.GetType(),
                                                                             validationResult.ValidationErrors
                                                                                             .Select(validationError => validationError.ErrorMessage))).ToMaybe())
                                    .FromMaybe(Enumerable.Empty<ValidationError>());
        }

        #endregion
    }
}