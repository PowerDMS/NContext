// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PatchRequest.cs" company="Waking Venture, Inc.">
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
    using System;

    using NContext.Common;

    using Newtonsoft.Json;

    /// <summary>
    /// Defines a transfer object for HTTP PATCH support.
    /// </summary>
    public class PatchRequest<T> : PatchRequestBase where T : class
    {
        private readonly String _JsonRepresentation;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatchRequest{T}"/> class.
        /// </summary>
        /// <param name="jsonRepresentation">The json representation.</param>
        public PatchRequest(String jsonRepresentation)
        {
            _JsonRepresentation = jsonRepresentation;
        }

        /// <summary>
        /// Patches the specified object using Json.net.
        /// </summary>
        /// <param name="original">The original object to patch.</param>
        /// <returns>IResponseTransferObject&lt;TDto&gt;.</returns>
        public virtual IResponseTransferObject<T> Patch(T original)
        {
            JsonConvert.PopulateObject(_JsonRepresentation, original);

            if (OnPatchedHandler != null)
            {
                OnPatchedHandler.Invoke(original);
            }

            return new ServiceResponse<T>(original);
        }
    }
}