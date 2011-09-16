// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationError.cs">
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
//   along with NContext.  If not, see <http://www.gnu.org/licenses/>.// </copyright>
// <summary>
//   Defines a transfer object which represents results returned from Microsoft Enterprise Library Validation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using NContext.Application.ErrorHandling;

namespace NContext.Application.Validation
{
    /// <summary>
    /// Defines a transfer object which represents results returned from Microsoft Enterprise Library Validation.
    /// </summary>
    [DataContract]
    public class ValidationError : Error
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationError"/> class.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="messages">The messages.</param>
        /// <remarks></remarks>
        public ValidationError(Type entityType, IEnumerable<String> messages)
            : base(entityType.Name, messages)
        {
        }
    }
}