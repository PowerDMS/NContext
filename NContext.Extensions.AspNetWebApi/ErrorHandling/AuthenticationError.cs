namespace NContext.Extensions.AspNetWebApi.ErrorHandling
{
    using System;
    using System.Net;
    using System.Reflection;
    using NContext.ErrorHandling;

    public class AuthenticationError : ErrorBase
    {
        private AuthenticationError(String localizationKey, HttpStatusCode httpStatusCode, params Object[] errorMessageParameters) 
            : base(localizationKey, httpStatusCode, errorMessageParameters)
        {
        }

        /// <summary>
        /// No providers could be found to authenticate the request.
        /// </summary>
        /// <returns>AuthenticationError.</returns>
        public static AuthenticationError ProviderNotFound()
        {
            return new AuthenticationError(MethodBase.GetCurrentMethod().Name, HttpStatusCode.InternalServerError);
        }
    }
}