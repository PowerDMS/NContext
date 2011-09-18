// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Error.cs">
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
//   Defines a data-transfer-object which represents an error.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NContext.Application.ErrorHandling
{
    /// <summary>
    /// Defines a data-transfer-object which represents an error.
    /// </summary>
    /// <remarks></remarks>
    [DataContract]
    public class Error
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Error"/> class.
        /// </summary>
        /// <param name="name">The name of the error which occurred.</param>
        /// <param name="messages">The messages describing the error.</param>
        /// <remarks></remarks>
        public Error(String name, IEnumerable<String> messages)
        {
            Name = name;
            Messages = messages;
            Type = GetType().Name;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <remarks></remarks>
        [DataMember(Order = 2)]
        public String Name { get; private set; }

        /// <summary>
        /// Gets the type of the error.
        /// </summary>
        /// <remarks></remarks>
        [DataMember(Order = 1)]
        public String Type { get; private set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <remarks></remarks>
        [DataMember(Order = 3)]
        public IEnumerable<String> Messages { get; private set; }
    }
}