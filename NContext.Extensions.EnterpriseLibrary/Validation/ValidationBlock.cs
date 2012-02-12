// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationBlock.cs">
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
//   Defines a cross-cutting validation block for validating any objects which implement <see cref="IValidatable"/>.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace NContext.Extensions.EnterpriseLibrary.Validation
{
    /// <summary>
    /// Defines a cross-cutting validation block for validating any objects which implement <see cref="IValidatable"/>.
    /// </summary>
    /// <remarks></remarks>
    public class ValidationBlock : IDisposable
    {
        #region Fields

        private readonly Lazy<ValidatorFactory> _ValidatorFactory = 
            new Lazy<ValidatorFactory>(() => EnterpriseLibraryContainer.Current.GetInstance<ValidatorFactory>());

        #endregion

        #region Validation

        /// <summary>
        /// Validates the specified validation object.
        /// </summary>
        /// <typeparam name="TValidatable">The type of the validatable.</typeparam>
        /// <param name="validationObject">The validation object.</param>
        /// <returns>The <see cref="ValidationResults"/>.</returns>
        /// <remarks></remarks>
        public virtual ValidationResults Validate<TValidatable>(TValidatable validationObject)
            where TValidatable : IValidatable
        {
            if (_ValidatorFactory.Value != null)
            {
                var validator = _ValidatorFactory.Value.CreateValidator(typeof(TValidatable));
                return validator.Validate(validationObject);
            }

            return null;
        }

        #endregion

        #region Implementation of IDisposable

        private Boolean _IsDisposed;

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the <see cref="ValidationBlock"/> is reclaimed by garbage collection.
        /// </summary>
        /// <remarks></remarks>
        ~ValidationBlock()
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
        private void Dispose(Boolean disposeManagedResources)
        {
            if (_IsDisposed)
            {
                return;
            }

            if (disposeManagedResources)
            {
                // Add custom dispose logic.
            }

            _IsDisposed = true;
        }

        #endregion
    }
}