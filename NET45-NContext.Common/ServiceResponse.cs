namespace NContext.Common
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines a data-transfer-object used for functional composition.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract(Name = "ServiceResponseOf{0}")]
    [KnownType(typeof(ErrorResponse<>))]
    [KnownType(typeof(DataResponse<>))]
    public abstract class ServiceResponse<T> : Either<Error, T>, IServiceResponse<T>
    {
        /// <summary>
        /// Performs an implicit conversion from <see cref="ServiceResponse{T}"/> to <see cref="System.Boolean"/>.
        /// </summary>
        /// <param name="serviceResponse">The service response.</param>
        /// <returns>The result of the conversion.</returns>
        /// <remarks></remarks>
        public static implicit operator Boolean(ServiceResponse<T> serviceResponse)
        {
            if (serviceResponse == null)
            {
                return false;
            }

            if (serviceResponse.Error != null)
            {
                return false;
            }

            if (typeof(T) == typeof(Boolean))
            {
                return Convert.ToBoolean(serviceResponse.Data);
            }

            return serviceResponse.Data != null;
        }

        /// <summary>
        /// Gets the error.
        /// </summary>
        /// <value>The error.</value>
        [DataMember]
        public virtual Error Error { get { return null; } }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>The data.</value>
        [DataMember]
        public virtual T Data { get { return default(T); } }
    }
}