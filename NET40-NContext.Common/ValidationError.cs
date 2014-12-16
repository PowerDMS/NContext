namespace NContext.Common
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines a transfer object which represents validation errors.
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
            : base(422, entityType.Name, messages)
        {
        }
    }
}