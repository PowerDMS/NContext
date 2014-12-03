// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpParameterBindingSanitizerFilter.cs" company="Waking Venture, Inc.">
//   Copyright (c) 2013 Waking Venture, Inc.
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