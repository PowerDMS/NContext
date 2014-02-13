// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Error.cs" company="Waking Venture, Inc.">
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
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.Common
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Runtime.Serialization;

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
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <param name="code">The code representing the reason for the error.</param>
        /// <param name="messages">The messages.</param>
        public Error(Int32 httpStatusCode, String code, IEnumerable<String> messages)
        {
            HttpStatusCode = httpStatusCode;
            Code = code;
            Messages = messages;
        }

        /// <summary>
        /// Gets the error code. It is best practice to use an HTTP Status Code to represent the error.
        /// </summary>
        /// <remarks></remarks>
        [DataMember(Order = 0, EmitDefaultValue = false)]
        public Int32 HttpStatusCode { get; private set; }

        /// <summary>
        /// Gets the code which represents the reason for the error.
        /// </summary>
        /// <value>The reason.</value>
        [DataMember(Order = 1)]
        public String Code { get; private set; }
        
        /// <summary>
        /// Gets the error's messages.
        /// </summary>
        /// <remarks></remarks>
        [DataMember(Order = 2)]
        public IEnumerable<String> Messages { get; private set; }
    }
}