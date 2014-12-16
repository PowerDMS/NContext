namespace NContext.Extensions.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Linq;

    using NContext.Common;
    using NContext.Data.Persistence;

    /// <summary>
    /// Defines extension methods for Entity Framework.
    /// </summary>
    /// <remarks></remarks>
    public static class EntityFrameworkExtensions
    {
        /// <summary>
        /// Determines whether all the validation results are valid.
        /// </summary>
        /// <param name="validationResults">The validation results.</param>
        /// <returns><c>True</c> if all validation results are valid, else <c>false</c>.</returns>
        /// <remarks></remarks>
        public static Boolean AreValid(this IEnumerable<DbEntityValidationResult> validationResults)
        {
            return validationResults.All(validationResult => validationResult.IsValid);
        }

        /// <summary>
        /// Returns a new <see cref="IServiceResponse{TEntity}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>IServiceResponse{TEntity}.</returns>
        public static IServiceResponse<TEntity> ToServiceResponse<TEntity>(this TEntity entity)
            where TEntity : IEntity
        {
            return new DataResponse<TEntity>(entity);
        }
    }
}