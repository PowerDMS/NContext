namespace NContext.Extensions.AspNet.WebApi.ErrorHandling
{
    using System;
    using System.Net;
    using System.Reflection;

    using NContext.ErrorHandling;

    /// <summary>
    /// Defines validation-related errors.
    /// </summary>
    internal class HttpParameterValidationError : ErrorBase
    {
        private HttpParameterValidationError(String localizationKey, HttpStatusCode httpStatusCode, String code, params Object[] errorMessageParameters)
            : base(localizationKey, httpStatusCode, code, errorMessageParameters)
        {
        }

        /// <summary>
        /// The required resource parameter '{0}' of type '{1}' could not be found within the request body for validation.
        /// </summary>
        /// <param name="parameterType">Type of the parameter.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>HttpParameterValidationError.</returns>
        public static HttpParameterValidationError RequiredParameterNotFoundInBody(Type parameterType, String parameterName)
        {
            return new HttpParameterValidationError(MethodBase.GetCurrentMethod().Name, HttpStatusCode.BadRequest, MethodBase.GetCurrentMethod().Name, parameterName, parameterType.Name);
        }

        /// <summary>
        /// The required resource parameter '{0}' with value '{1}' does not match the value set in the request body: '{2}'.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="requiredParameterValue">The required parameter value.</param>
        /// <param name="bodyPropertyValue">The body property value.</param>
        /// <returns>HttpParameterValidationError.</returns>
        public static HttpParameterValidationError ValidationFailed(String parameterName, Object requiredParameterValue, Object bodyPropertyValue)
        {
            return new HttpParameterValidationError(MethodBase.GetCurrentMethod().Name, HttpStatusCode.BadRequest, MethodBase.GetCurrentMethod().Name, parameterName, requiredParameterValue, bodyPropertyValue);
        }
    }
}