namespace NContext.Extensions.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Linq;

    using NContext.Common;

    /// <summary>
    /// Defines an Entity Framework <see cref="ServiceResponse{T}"/> adapter.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class EfServiceResponse<T> : ServiceResponse<T>
    {
        private readonly Error _Error;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponse{T}"/> class.
        /// </summary>
        /// <param name="validationResult">The validation result.</param>
        /// <remarks></remarks>
        public EfServiceResponse(DbEntityValidationResult validationResult)
            : this(new List<DbEntityValidationResult> { validationResult })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponse{T}"/> class.
        /// </summary>
        /// <param name="validationResults">The validation results.</param>
        /// <remarks></remarks>
        public EfServiceResponse(IEnumerable<DbEntityValidationResult> validationResults)
        {
            _Error = TranslateDbEntityValidationResultsToValidationErrors(validationResults);
        }

        private static Error TranslateDbEntityValidationResultsToValidationErrors(IEnumerable<DbEntityValidationResult> validationResults)
        {
            return new AggregateError(
                422, 
                "ValidationErrors",
                validationResults.Select(validationResult =>
                    new ValidationError(
                        validationResult.Entry.Entity.GetType(),
                        validationResult.ValidationErrors.Select(validationError => validationError.ErrorMessage))));
        }

        public override Boolean IsLeft
        {
            get { return true; }
        }

        public override Error GetLeft()
        {
            return _Error;
        }

        public override T GetRight()
        {
            return default(T);
        }
    }
}