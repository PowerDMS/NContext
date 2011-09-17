// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationBlock.cs">
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
//   Defines a cross-cutting validation block for validating any objects which implement <see cref="IValidatable"/>.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace NContext.Application.Validation
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