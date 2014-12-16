namespace NContext.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents one or more errors that occur during application execution.
    /// </summary>
    [DataContract]
    public class AggregateError : Error
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateError"/> class.
        /// </summary>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <param name="code">The code.</param>
        /// <param name="errors">The errors.</param>
        public AggregateError(Int32 httpStatusCode, String code, IEnumerable<Error> errors) 
            : base(
                httpStatusCode, 
                code, 
                errors.ToMaybe()
                    .Bind(
                        errorCollection => 
                            errorCollection.SelectMany(e => e.Messages).ToMaybe())
                    .FromMaybe(Enumerable.Empty<String>()))
        {
            Errors = errors;
        }

        [DataMember]
        public IEnumerable<Error> Errors { get; private set; }
    }
}