// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityFrameworkExtensions.cs">
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
//   Defines extension methods for Entity Framework.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace NContext.Persistence.EntityFramework
{
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
    }
}