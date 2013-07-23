// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormatterParameterBindingSanitizerFilter.cs" company="Waking Venture, Inc.">
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
    using System.ComponentModel;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    /// <summary>
    /// Defines an action filter that allows for auto-sanitization of formatter parameter bindings. (Request body content & DTOs)
    /// Supports complex object graphs with circular references and navigation properties.
    /// </summary>
    public class FormatterParameterBindingSanitizerFilter : ActionFilterAttribute
    {
        private readonly ITextSanitizer _TextSanitizer;

        private readonly IEnumerable<HttpMethod> _FilterMethods;

        private readonly Lazy<ObjectGraphSanitizer> _ObjectGraphSanitizer; 

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatterParameterBindingSanitizerFilter"/> class.
        /// </summary>
        /// <param name="textSanitizer">The text sanitizer.</param>
        /// <param name="filterMethods">The filter methods.</param>
        public FormatterParameterBindingSanitizerFilter(ITextSanitizer textSanitizer, params HttpMethod[] filterMethods)
        {
            _TextSanitizer = textSanitizer;
            _FilterMethods = filterMethods;
            _ObjectGraphSanitizer = new Lazy<ObjectGraphSanitizer>(() => new ObjectGraphSanitizer(_TextSanitizer));
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (_FilterMethods.All(filterMethod => filterMethod != actionContext.Request.Method))
            {
                return;
            }

            if (actionContext.ActionDescriptor.ActionBinding.ParameterBindings.All(pb => !pb.WillReadBody))
            {
                return;
            }

            var formatterParameterBinding =
                actionContext.ActionDescriptor
                             .ActionBinding
                             .ParameterBindings
                             .SingleOrDefault(
                                binding =>
                                !binding.Descriptor.IsOptional &&
                                !CanConvertFromString(binding.Descriptor.ParameterType) &&
                                binding.WillReadBody);

            // We only want to sanitize complex objects or strings.
            if (formatterParameterBinding == null || 
                (formatterParameterBinding.Descriptor.ParameterType != typeof(String) && 
                 IsSimpleType(formatterParameterBinding.Descriptor.ParameterType)))
            {
                return;
            }

            if (formatterParameterBinding.Descriptor.ParameterType == typeof(String))
            {
                actionContext.ActionArguments[formatterParameterBinding.Descriptor.ParameterName] =
                    _TextSanitizer.Sanitize((String) actionContext.ActionArguments[formatterParameterBinding.Descriptor.ParameterName]);
            }
            else
            {
                _ObjectGraphSanitizer.Value.Sanitize(actionContext.ActionArguments[formatterParameterBinding.Descriptor.ParameterName]);
            }
        }

        private Boolean CanConvertFromString(Type parameterType)
        {
            return IsSimpleUnderlyingType(parameterType) || HasStringConverter(parameterType);
        }

        private Boolean IsSimpleUnderlyingType(Type parameterType)
        {
            Type underlyingType = Nullable.GetUnderlyingType(parameterType);
            if (underlyingType != null)
            {
                parameterType = underlyingType;
            }

            return IsSimpleType(parameterType);
        }

        private Boolean IsSimpleType(Type parameterType)
        {
            return parameterType.IsPrimitive ||
                   parameterType == typeof(String) ||
                   parameterType == typeof(DateTime) ||
                   parameterType == typeof(Decimal) ||
                   parameterType == typeof(Guid) ||
                   parameterType == typeof(DateTimeOffset) ||
                   parameterType == typeof(TimeSpan);
        }

        private Boolean HasStringConverter(Type parameterType)
        {
            return TypeDescriptor.GetConverter(parameterType).CanConvertFrom(typeof(String));
        }
    }
}