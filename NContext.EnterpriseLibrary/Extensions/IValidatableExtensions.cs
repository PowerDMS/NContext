// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IValidatableExtensions.cs">
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

// <summary>
//   Defines a static class for providing IValidatable type extension methods.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.EnterpriseLibrary.Validation;

using NContext.Application.Validation;

namespace NContext.Application.Extensions
{
    /// <summary>
    /// Defines a static class for providing IValidatable type extension methods.
    /// </summary>
    public static class IValidatableExtensions
    {
        /// <summary>
        /// Validates the specified <see cref="IValidatable"/> validation object.
        /// </summary>
        /// <typeparam name="TValidatable">The type of the validatable object.</typeparam>
        /// <param name="validationObject">The validation object.</param>
        /// <returns>The <see cref="ValidationResults"/>.</returns>
        public static ValidationResults Validate<TValidatable>(this TValidatable validationObject) where TValidatable : IValidatable
        {
            using (var validationBlock = new ValidationBlock())
            {
                return validationBlock.Validate(validationObject);
            }
        }

        /// <summary>
        /// Validates the specified <see cref="IValidatable"/> validation object.
        /// </summary>
        /// <typeparam name="TValidatable">The type of the validatable object.</typeparam>
        /// <param name="validationObject">The validation object.</param>
        /// <param name="validationResults">The validation results.</param>
        /// <returns><c>True</c> if <paramref name="validationObject"/> is valid, else <c>false</c>.</returns>
        public static Boolean Validate<TValidatable>(this TValidatable validationObject, out ValidationResults validationResults) where TValidatable : IValidatable
        {
            validationResults = Validate(validationObject);
            return validationResults.IsValid;
        }

        /// <summary>
        /// Validates the specified <see cref="IValidatable"/> validation objects.
        /// </summary>
        /// <typeparam name="TValidatable">The type of the validatable object.</typeparam>
        /// <param name="validationObjects">The validation objects.</param>
        /// <param name="validationResults">The validation results.</param>
        /// <returns><c>True</c> if <paramref name="validationObjects"/> is valid, else <c>false</c>.</returns>
        public static Boolean Validate<TValidatable>(this List<TValidatable> validationObjects, out ValidationResults validationResults) where TValidatable : IValidatable
        {
            return Validate(validationObjects.AsEnumerable(), out validationResults);
        }

        /// <summary>
        /// Validates the specified <see cref="IValidatable"/> validation objects.
        /// </summary>
        /// <typeparam name="TValidatable">The type of the validatable object.</typeparam>
        /// <param name="validationObjects">The validation objects.</param>
        /// <param name="validationResults">The validation results.</param>
        /// <returns><c>True</c> if <paramref name="validationObjects"/> is valid, else <c>false</c>.</returns>
        public static Boolean Validate<TValidatable>(this IEnumerable<TValidatable> validationObjects, out ValidationResults validationResults) where TValidatable : IValidatable
        {
            validationResults = new ValidationResults();
            foreach (var validationObject in validationObjects)
            {
                validationResults.AddAllResults(validationObject.Validate());
            }

            return validationResults.IsValid;
        }
    }
}