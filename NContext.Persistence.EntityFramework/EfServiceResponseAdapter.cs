// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EfServiceResponseAdapter.cs">
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

using Omu.ValueInjecter;

namespace NContext.Persistence.EntityFramework
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

        /// <summary>
        /// Translates the data, if any, into the specified <typeparamref name="TDto"/> using <see cref="LoopValueInjection"/>.
        /// </summary>
        /// <typeparam name="TDto">The type of the dto.</typeparam>
        /// <returns>Instance of <see cref="ServiceResponse{TDto}"/>.</returns>
        /// <remarks>
        /// Use this to translate domain services / aggregates / entities into service responses using data transfer objects.
        /// </remarks>
        public virtual ServiceResponse<TDto> Translate<TDto>()
            where TDto : class, new()
        {
            return Translate<TDto, LoopValueInjection>();
        }

        /// <summary>
        /// Translates the data, if any, into the specified <typeparamref name="TDto"/> using the specified <see cref="IValueInjection"/>.
        /// </summary>
        /// <typeparam name="TDto">The type of the dto.</typeparam>
        /// <typeparam name="TValueInjection">The type of the value injection.</typeparam>
        /// <returns>Instance of <see cref="ServiceResponse{TDto}"/>.</returns>
        /// <remarks>
        /// Use this to translate domain services / aggregates / entities into service responses using data transfer objects.
        /// </remarks>
        public virtual ServiceResponse<TDto> Translate<TDto, TValueInjection>()
            where TDto : class, new()
            where TValueInjection : IValueInjection, new()
        {
            if (Errors.Any())
            {
                return new ServiceResponse<TDto>(Errors);
            }

            return (ServiceResponse<TDto>)Activator.CreateInstance(typeof(ServiceResponse<TDto>), 
                                                                   Data.Select(data => Activator.CreateInstance(typeof(TDto))
                                                                                                .InjectInto<TDto, TValueInjection>(data)));
        }

        private static IEnumerable<Error> TranslateErrorBaseToErrorCollection(IEnumerable<ErrorBase> errors)
        {
            return errors.ToMaybe()
                         .Bind(e => e.Select(error => new Error(error.Name, new List<String> { error.Message })).ToMaybe())
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