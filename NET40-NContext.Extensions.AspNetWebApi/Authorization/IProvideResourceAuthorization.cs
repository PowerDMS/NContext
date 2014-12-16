namespace NContext.Extensions.AspNetWebApi.Authorization
{
    using System;
    using System.Security.Principal;
    using System.Web.Http.Controllers;

    /// <summary>
    /// Defines a provider role for resource authorization.
    /// </summary>
    /// <remarks></remarks>
    public interface IProvideResourceAuthorization
    {
        /// <summary>
        /// Authorizes the specified <see cref="IPrincipal" /> against the specified <see cref="HttpActionDescriptor" />.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="actionContext">The action context.</param>
        /// <returns>Boolean.</returns>
        Boolean Authorize(IPrincipal principal, HttpActionContext actionContext);
    }
}