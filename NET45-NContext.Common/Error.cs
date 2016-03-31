namespace NContext.Common
{
    using System;
    using System.Collections.Generic;
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