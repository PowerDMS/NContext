namespace NContext.Extensions.AspNetWebApi.Filters
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using System.Web.Http.ModelBinding;

    using ErrorHandling;
    using Extensions;

    /// <summary>
    /// Defines an action filter for validating required route parameter arguments are equal
    /// to values found in the content or body of a request - typically in the form of a DTO.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class HttpParameterBindingValidationActionFilterAttribute : ActionFilterAttribute
    {
        private readonly IEnumerable<String> _FilterableMethods;

        private Boolean _BodyParameterNotFoundReturnsError;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpParameterBindingValidationActionFilterAttribute" /> class.
        /// </summary>
        public HttpParameterBindingValidationActionFilterAttribute()
            : this(Enumerable.Empty<String>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpParameterBindingValidationActionFilterAttribute" /> class.
        /// </summary>
        /// <param name="filterableMethods">The filterable methods.</param>
        public HttpParameterBindingValidationActionFilterAttribute(params HttpMethod[] filterableMethods)
            : this(filterableMethods.Select(method => method.Method).ToList())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpParameterBindingValidationActionFilterAttribute" /> class.
        /// </summary>
        /// <param name="httpMethods">The filterable methods.</param>
        public HttpParameterBindingValidationActionFilterAttribute(IEnumerable<String> httpMethods)
        {
            httpMethods = httpMethods as IList<String> ?? httpMethods.ToList();
            var methodsToFilter = !httpMethods.Any()
                                      ? new[] {"post", "put", "patch"}
                                      : httpMethods.Select(method => method.ToLower(CultureInfo.InvariantCulture));

            _FilterableMethods = new HashSet<String>(methodsToFilter);
            _BodyParameterNotFoundReturnsError = true;
        }

        /// <summary>
        /// Gets or sets the desired behavior when a required URI parameter is not found within the request body. Default 
        /// behavior returns a <see cref="NContext.Common.ServiceResponse{T}"/> with error <see cref="HttpParameterValidationError.RequiredParameterNotFoundInBody"/>.
        /// </summary>
        /// <value>The body parameter not found returns error.</value>
        public Boolean BodyParameterNotFoundReturnsError
        {
            get { return _BodyParameterNotFoundReturnsError; }
            set { _BodyParameterNotFoundReturnsError = value; }
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!_FilterableMethods.Contains(actionContext.Request.Method.Method) ||
                !actionContext.ModelState.IsValid ||
                actionContext.ActionDescriptor.ActionBinding.ParameterBindings.All(binding => !(binding.WillReadBody)))
            {
                return;
            }

            var parameterDescriptors = actionContext.ActionDescriptor.GetParameters();

            /* If the descriptor count equals one, then there is only a single parameter that is bound by
             * the request body. In this case, there is nothing to validate against in the request URI.
             * */
            if (parameterDescriptors.Count == 1)
            {
                return;
            }

            Validate(actionContext, parameterDescriptors);
        }

        protected virtual void Validate(HttpActionContext actionContext, IEnumerable<HttpParameterDescriptor> parameterDescriptors)
        {
            Contract.Requires(actionContext != null);
            Contract.Requires(parameterDescriptors != null && parameterDescriptors.Any());

            var requiredParameterDescriptors =
                parameterDescriptors.Where(
                    descriptor =>
                    !descriptor.IsOptional &&
                    (descriptor.ParameterType.IsPrimitive || descriptor.ParameterBinderAttribute is ModelBinderAttribute));

            var requiredBodyParameterDescriptor = actionContext.ActionDescriptor.ActionBinding.ParameterBindings.Single(binding => binding.WillReadBody).Descriptor;
            var requiredBodyParameterArgumentValue = actionContext.ActionArguments[requiredBodyParameterDescriptor.ParameterName];
            var requiredBodyProperties = TypeDescriptor.GetProperties(requiredBodyParameterArgumentValue).Cast<PropertyDescriptor>().ToList();

            foreach (var parameterDescriptor in requiredParameterDescriptors)
            {
                var requiredParameterValue = actionContext.ActionArguments[parameterDescriptor.ParameterName];
                var bodyProperty = requiredBodyProperties.SingleOrDefault(propertyDescriptor => IsMatch(parameterDescriptor, propertyDescriptor));
                if (bodyProperty == null)
                {
                    if (BodyParameterNotFoundReturnsError)
                    {
                        actionContext.Response = 
                            ErrorBaseExtensions.ToServiceResponse<Object>(
                                HttpParameterValidationError.RequiredParameterNotFoundInBody(parameterDescriptor.ParameterType, parameterDescriptor.ParameterName))
                                               .ToHttpResponseMessage(actionContext.Request);
                        return;
                    }

                    continue;
                }

                var bodyPropertyValue = bodyProperty.GetValue(requiredBodyParameterArgumentValue);
                if (bodyPropertyValue == null || !bodyPropertyValue.Equals(requiredParameterValue))
                {
                    actionContext.Response =
                        ErrorBaseExtensions.ToServiceResponse<Object>(
                            HttpParameterValidationError.ValidationFailed(parameterDescriptor.ParameterName, requiredParameterValue, bodyPropertyValue))
                                           .ToHttpResponseMessage(actionContext.Request);
                    return;
                }
            }
        }

        protected virtual Boolean IsMatch(HttpParameterDescriptor parameterDescriptor, PropertyDescriptor propertyDescriptor)
        {
            return TypesMatch(propertyDescriptor.PropertyType, parameterDescriptor.ParameterType) && 
                   NamesMatch(propertyDescriptor.Name, parameterDescriptor.ParameterName);
        }

        protected virtual Boolean TypesMatch(Type modelBinderPropertyType, Type bodyParameterPropertyType)
        {
            return modelBinderPropertyType == bodyParameterPropertyType;
        }

        protected virtual Boolean NamesMatch(String modelBinderPropertyName, String bodyParameterPropertyName)
        {
            return modelBinderPropertyName.Equals(bodyParameterPropertyName, StringComparison.OrdinalIgnoreCase);
        }
    }
}