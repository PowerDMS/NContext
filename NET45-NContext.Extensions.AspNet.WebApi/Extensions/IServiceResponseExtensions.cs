namespace NContext.Extensions.AspNet.WebApi.Extensions
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Common;

    /// <summary>
    /// Defines extension methods for <see cref="IServiceResponse{T}"/>.
    /// </summary>
    public static class IServiceResponseExtensions
    {
        /// <summary>
        /// Returns a new <see cref="HttpResponseMessage"/> with the <see cref="HttpResponseMessage.Content"/> set to <paramref name="serviceResponse"/>. If 
        /// <paramref name="serviceResponse"/> contains an error, it will attempt to parse the <see cref="Error.Code"/> as an <see cref="HttpStatusCode"/> 
        /// and assign it to the response message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResponse">The content contained in the HTTP response.</param>
        /// <param name="httpRequestMessage">The active <see cref="HttpRequestMessage"/>.</param>
        /// <param name="statusCode">The <see cref="HttpStatusCode"/> to set if <paramref name="serviceResponse"/> has no errors.</param>
        /// <returns>HttpResponseMessage instance.</returns>
        public static HttpResponseMessage ToHttpResponseMessage<T>(
            this IServiceResponse<T> serviceResponse, 
            HttpRequestMessage httpRequestMessage,
            HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            if (serviceResponse == null)
            {
                throw new ArgumentNullException("serviceResponse");
            }

            if (httpRequestMessage == null)
            {
                throw new ArgumentNullException("httpRequestMessage");
            }

            var responseStatusCode = statusCode;
            if (serviceResponse.Error != null)
            {
                responseStatusCode = (HttpStatusCode) serviceResponse.Error.HttpStatusCode;
            }

            return ShouldSetResponseContent(httpRequestMessage, responseStatusCode)
                ? httpRequestMessage.CreateResponse(responseStatusCode, serviceResponse)
                : httpRequestMessage.CreateResponse(responseStatusCode);
        }

        /// <summary>
        /// Returns a new <see cref="HttpResponseMessage"/> with the <see cref="HttpResponseMessage.Content"/> set to <paramref name="serviceResponse"/>. If 
        /// <paramref name="serviceResponse"/> contains an error, it will attempt to parse the <see cref="Error.Code"/> as an <see cref="HttpStatusCode"/> 
        /// and assign it to the response message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResponse">The content contained in the HTTP response.</param>
        /// <param name="httpRequestMessage">The active <see cref="HttpRequestMessage"/>.</param>
        /// <param name="statusCode">The <see cref="HttpStatusCode"/> to set if <paramref name="serviceResponse"/> has no errors.</param>
        /// <returns>HttpResponseMessage instance.</returns>
        public static Task<HttpResponseMessage> ToHttpResponseMessageAsync<T>(
            this IServiceResponse<T> serviceResponse,
            HttpRequestMessage httpRequestMessage,
            HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            if (serviceResponse == null)
            {
                throw new ArgumentNullException("serviceResponse");
            }

            if (httpRequestMessage == null)
            {
                throw new ArgumentNullException("httpRequestMessage");
            }

            var responseStatusCode = statusCode;
            if (serviceResponse.Error != null)
            {
                responseStatusCode = (HttpStatusCode)serviceResponse.Error.HttpStatusCode;
            }

            return Task.FromResult(
                ShouldSetResponseContent(httpRequestMessage, responseStatusCode)
                    ? httpRequestMessage.CreateResponse(responseStatusCode, serviceResponse)
                    : httpRequestMessage.CreateResponse(responseStatusCode));
        }

        /// <summary>
        /// Returns a new <see cref="HttpResponseMessage"/> with the <see cref="HttpResponseMessage.Content"/> set to <paramref name="serviceResponseFuture"/>. If 
        /// <paramref name="serviceResponseFuture"/> contains an error, it will attempt to parse the <see cref="Error.Code"/> as an <see cref="HttpStatusCode"/> 
        /// and assign it to the response message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResponseFuture">The content contained in the HTTP response.</param>
        /// <param name="httpRequestMessage">The active <see cref="HttpRequestMessage"/>.</param>
        /// <param name="statusCode">The <see cref="HttpStatusCode"/> to set if <paramref name="serviceResponseFuture"/> has no errors.</param>
        /// <returns>HttpResponseMessage instance.</returns>
        public static async Task<HttpResponseMessage> AwaitToHttpResponseMessageAsync<T>(
            this Task<IServiceResponse<T>> serviceResponseFuture,
            HttpRequestMessage httpRequestMessage,
            HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            if (serviceResponseFuture == null)
            {
                throw new ArgumentNullException("serviceResponseFuture");
            }

            if (httpRequestMessage == null)
            {
                throw new ArgumentNullException("httpRequestMessage");
            }

            var serviceResponse = await serviceResponseFuture;
            return await serviceResponse.ToHttpResponseMessageAsync(httpRequestMessage);
        }

        /// <summary>
        /// Invokes the specified <paramref name="responseBuilder" /> action if <paramref name="serviceResponse" /> does not contain an error - returning the configured <see cref="HttpResponseMessage" />.
        /// If <paramref name="serviceResponse" /> contains errors, the returned response with contain the error content and will attempt to parse the <see cref="Error.Code" /> as an
        /// <see cref="HttpStatusCode" /> and assign it to the response message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResponse">The <see cref="IServiceResponse{t}" /> used to build the <see cref="HttpResponseMessage" />.</param>
        /// <param name="httpRequestMessage">The active <see cref="HttpRequestMessage" />.</param>
        /// <param name="responseBuilder">The response builder method to invoke when no errors exist.</param>
        /// <param name="setResponseContent">Determines whether to set the <param name="serviceResponse.Data"></param> as the response content.</param>
        /// <returns>HttpResponseMessage instance.</returns>
        public static HttpResponseMessage ToHttpResponseMessage<T>(
            this IServiceResponse<T> serviceResponse, 
            HttpRequestMessage httpRequestMessage, 
            Action<T, HttpResponseMessage> responseBuilder,
            Boolean setResponseContent = true)
        {
            if (serviceResponse == null)
            {
                throw new ArgumentNullException("serviceResponse");
            }

            if (httpRequestMessage == null)
            {
                throw new ArgumentNullException("httpRequestMessage");
            }

            var responseStatusCode = HttpStatusCode.OK;
            if (serviceResponse.Error != null)
            {
                responseStatusCode = (HttpStatusCode) serviceResponse.Error.HttpStatusCode;
            }

            var response = setResponseContent && ShouldSetResponseContent(httpRequestMessage, responseStatusCode)
                ? httpRequestMessage.CreateResponse(responseStatusCode, serviceResponse)
                : httpRequestMessage.CreateResponse(responseStatusCode);

            if (serviceResponse.IsRight && responseBuilder != null)
            {
                responseBuilder.Invoke(serviceResponse.Data, response);
            }

            return response;
        }

        /// <summary>
        /// Invokes the specified <paramref name="responseBuilder" /> action if <paramref name="serviceResponse" /> does not contain an error - returning the configured <see cref="HttpResponseMessage" />.
        /// If <paramref name="serviceResponse" /> contains errors, the returned response with contain the error content and will attempt to parse the <see cref="Error.Code" /> as an
        /// <see cref="HttpStatusCode" /> and assign it to the response message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResponse">The <see cref="IServiceResponse{t}" /> used to build the <see cref="HttpResponseMessage" />.</param>
        /// <param name="httpRequestMessage">The active <see cref="HttpRequestMessage" />.</param>
        /// <param name="responseBuilder">The response builder method to invoke when no errors exist.</param>
        /// <param name="setResponseContent">Determines whether to set the <param name="serviceResponse.Data"></param> as the response content.</param>
        /// <returns>HttpResponseMessage instance.</returns>
        public static Task<HttpResponseMessage> ToHttpResponseMessageAsync<T>(
            this IServiceResponse<T> serviceResponse,
            HttpRequestMessage httpRequestMessage,
            Action<T, HttpResponseMessage> responseBuilder,
            Boolean setResponseContent = true)
        {
            if (serviceResponse == null)
            {
                throw new ArgumentNullException("serviceResponse");
            }

            if (httpRequestMessage == null)
            {
                throw new ArgumentNullException("httpRequestMessage");
            }

            var responseStatusCode = HttpStatusCode.OK;
            if (serviceResponse.Error != null)
            {
                responseStatusCode = (HttpStatusCode)serviceResponse.Error.HttpStatusCode;
            }

            var response = setResponseContent && ShouldSetResponseContent(httpRequestMessage, responseStatusCode)
                ? httpRequestMessage.CreateResponse(responseStatusCode, serviceResponse)
                : httpRequestMessage.CreateResponse(responseStatusCode);

            if (serviceResponse.IsRight && responseBuilder != null)
            {
                responseBuilder.Invoke(serviceResponse.Data, response);
            }

            return Task.FromResult(response);
        }

        public static async Task<HttpResponseMessage> AwaitToHttpResponseMessageAsync<T>(
            this Task<IServiceResponse<T>> serviceResponseFuture,
            HttpRequestMessage httpRequestMessage,
            Action<T, HttpResponseMessage> responseBuilder,
            bool setResponseContent = true)
        {
            var serviceResponse = await serviceResponseFuture;
            return await serviceResponse.ToHttpResponseMessageAsync(httpRequestMessage, responseBuilder, setResponseContent);
        }

        public static async Task<HttpResponseMessage> ToHttpResponseMessageAsync<T>(
            this IServiceResponse<T> serviceResponse,
            HttpRequestMessage httpRequestMessage,
            Func<T, HttpResponseMessage, Task> responseBuilder,
            Boolean setResponseContent = true)
        {
            if (serviceResponse == null)
            {
                throw new ArgumentNullException("serviceResponse");
            }

            if (httpRequestMessage == null)
            {
                throw new ArgumentNullException("httpRequestMessage");
            }

            var responseStatusCode = HttpStatusCode.OK;
            if (serviceResponse.Error != null)
            {
                responseStatusCode = (HttpStatusCode)serviceResponse.Error.HttpStatusCode;
            }

            var response = setResponseContent && ShouldSetResponseContent(httpRequestMessage, responseStatusCode)
                ? httpRequestMessage.CreateResponse(responseStatusCode, serviceResponse)
                : httpRequestMessage.CreateResponse(responseStatusCode);

            if (serviceResponse.IsRight && responseBuilder != null)
            {
                await responseBuilder.Invoke(serviceResponse.Data, response);
            }

            return response;
        }
        
        public static async Task<HttpResponseMessage> AwaitToHttpResponseMessageAsync<T>(
            this Task<IServiceResponse<T>> serviceResponseFuture, 
            HttpRequestMessage httpRequestMessage,
            Func<T, HttpResponseMessage, Task> responseBuilder, 
            bool setResponseContent = true)
        {
            var serviceResponse = await serviceResponseFuture;
            return await serviceResponse.ToHttpResponseMessageAsync(httpRequestMessage, responseBuilder, setResponseContent);
        }

        private static Boolean ShouldSetResponseContent(HttpRequestMessage httpRequestMessage, HttpStatusCode responseStatusCode)
        {
            return httpRequestMessage.Method != HttpMethod.Head &&
                   responseStatusCode != HttpStatusCode.NoContent &&
                   responseStatusCode != HttpStatusCode.ResetContent &&
                   responseStatusCode != HttpStatusCode.NotModified &&
                   responseStatusCode != HttpStatusCode.Continue;
        }
    }
}