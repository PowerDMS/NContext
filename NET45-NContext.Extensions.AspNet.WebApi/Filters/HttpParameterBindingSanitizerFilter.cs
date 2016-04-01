namespace NContext.Extensions.AspNetWebApi.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    using Microsoft.FSharp.Core;

    using NContext.Common;
    using NContext.Exceptions;
    using NContext.Text;

    /// <summary>
    /// Defines an action filter that allows for auto-sanitization of HTTP parameter bindings.
    /// Supports complex object graphs with circular references and navigation properties.
    /// </summary>
    public class HttpParameterBindingSanitizerFilter : ActionFilterAttribute
    {
        private readonly ISanitizeText _TextSanitizer;

        private readonly Int32 _MaxDegreeOfParallelism;

        private readonly IEnumerable<HttpMethod> _FilterMethods;

        private readonly Lazy<ObjectGraphSanitizer> _ObjectGraphSanitizer; 
        
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpParameterBindingSanitizerFilter"/> class.
        /// </summary>
        /// <param name="textSanitizer">The text sanitizer.</param>
        /// <param name="filterMethods">The filter methods.</param>
        public HttpParameterBindingSanitizerFilter(ISanitizeText textSanitizer, params HttpMethod[] filterMethods)
            : this(textSanitizer, Environment.ProcessorCount, filterMethods)
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpParameterBindingSanitizerFilter"/> class.
        /// </summary>
        /// <param name="textSanitizer">The text sanitizer.</param>
        /// <param name="maxDegreeOfParallelism">The max degree of parallelism to invoke sanitization.</param>
        /// <param name="filterMethods">The filter methods.</param>
        public HttpParameterBindingSanitizerFilter(ISanitizeText textSanitizer, Int32 maxDegreeOfParallelism, params HttpMethod[] filterMethods)
        {
            _TextSanitizer = textSanitizer;
            _MaxDegreeOfParallelism = maxDegreeOfParallelism <= 0 ? Environment.ProcessorCount : maxDegreeOfParallelism;
            _FilterMethods = filterMethods == null || !filterMethods.Any() ? new[] { HttpMethod.Post, HttpMethod.Put, new HttpMethod("PATCH") } : filterMethods;
            _ObjectGraphSanitizer = new Lazy<ObjectGraphSanitizer>(() => new ObjectGraphSanitizer(_TextSanitizer, _MaxDegreeOfParallelism));
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (_FilterMethods.All(filterMethod => filterMethod != actionContext.Request.Method))
            {
                return;
            }

            try
            {
                actionContext.ActionDescriptor
                    .ActionBinding
                    .ParameterBindings
                    .Where(pb => pb.Descriptor.ParameterType == typeof (String) || pb.WillReadBody)
                    .ForEach(parameterBinding =>
                    {
                        if (parameterBinding.Descriptor.ParameterType == typeof (String))
                        {
                            if (
                                !String.IsNullOrWhiteSpace(
                                    (String) actionContext.ActionArguments[parameterBinding.Descriptor.ParameterName]))
                            {
                                actionContext.ActionArguments[parameterBinding.Descriptor.ParameterName] =
                                    SanitizeString(
                                        (String)
                                            actionContext.ActionArguments[parameterBinding.Descriptor.ParameterName]);
                            }
                        }
                        else
                        {
                            SanitizeObjectGraph(actionContext.ActionArguments[parameterBinding.Descriptor.ParameterName]);
                        }
                    });
            }
            catch (SanitizationException sanitizationException)
            {
                var response = actionContext.Request.CreateResponse(
                    (HttpStatusCode) 422,
                    new ErrorResponse<Unit>(sanitizationException.ToError()));

                actionContext.Response = response;
            }
            catch (Exception e)
            {
                var response = actionContext.Request.CreateResponse(
                    HttpStatusCode.InternalServerError,
                    new ErrorResponse<Unit>(e.ToError()));

                actionContext.Response = response;
            }
        }

        public virtual String SanitizeString(String textToSanitize)
        {
            return _TextSanitizer.SanitizeHtmlFragment(textToSanitize);
        }

        public virtual void SanitizeObjectGraph(Object objectToSanitize)
        {
            _ObjectGraphSanitizer.Value.Sanitize(objectToSanitize);
        }
    }
}