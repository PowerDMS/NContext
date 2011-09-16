using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace NContext.Persistence.EntityFramework
{
    public static class EntityFrameworkExtensions
    {
        public static Boolean IsValid(this IEnumerable<DbEntityValidationResult> validationResults)
        {
            return validationResults.All(validationResult => validationResult.IsValid);
        }
    }
}