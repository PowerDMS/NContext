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
        /// <returns>IServiceResponse{T}.</returns>
        public virtual IServiceResponse<T> Patch(T original)
        {
            JsonConvert.PopulateObject(_JsonRepresentation, original);

            if (OnPatchedHandler != null)
            {
                OnPatchedHandler.Invoke(original);
            }

            return new DataResponse<T>(original);
        }
    }
}