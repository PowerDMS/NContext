// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PatchRequestParameterBindingAttribute.cs" company="Waking Venture, Inc.">
//   Copyright (c) 2014 Waking Venture, Inc.
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