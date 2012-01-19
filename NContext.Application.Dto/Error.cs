// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Error.cs">
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
//   Defines a data-transfer-object which represents an error.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NContext.Application.Dto
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
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <remarks></remarks>
        [DataMember(Order = 1)]
        public String Name { get; private set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <remarks></remarks>
        [DataMember(Order = 2)]
        public IEnumerable<String> Messages { get; private set; }
    }
}