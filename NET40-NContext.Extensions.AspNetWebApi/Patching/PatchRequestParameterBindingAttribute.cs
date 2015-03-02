namespace NContext.Extensions.AspNetWebApi.Patching
{
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Validation;

    /// <summary>
    /// This attribute is only used on action parameters of type <see cref="PatchRequest{T}"/>.
    /// </summary>
    public class PatchRequestParameterBindingAttribute : ParameterBindingAttribute
    {
        /// <summary>
        /// Gets the binding.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>System.Web.Http.Controllers.HttpParameterBinding.</returns>
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            IBodyModelValidator bodyModelValidator = parameter.Configuration.Services.GetBodyModelValidator();

            return new PatchRequestParameterBinding(parameter, bodyModelValidator);
        }
    }
}