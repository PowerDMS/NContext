// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IValidatableExtensions.cs">
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
//   Defines a static class for providing IValidatable type extension methods.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.EnterpriseLibrary.Validation;

using NContext.EnterpriseLibrary.Validation;

namespace NContext.EnterpriseLibrary.Extensions
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