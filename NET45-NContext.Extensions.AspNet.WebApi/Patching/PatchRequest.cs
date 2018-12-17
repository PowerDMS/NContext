namespace NContext.Extensions.AspNet.WebApi.Patching
{
    using System;

    using Common;

    using Extensions;

    using Newtonsoft.Json;

    /// <summary>
    /// Delegate PatchedAction
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="patched">The patched object.</param>
    public delegate void PatchedAction<in T>(T patched);

    /// <summary>
    /// Delegate ClonedPatchedAction.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="original">The original object.</param>
    /// <param name="patched">The patched object.</param>
    public delegate void ClonedPatchedAction<in T>(T original, T patched);

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
        /// Gets the json representation.
        /// </summary>
        /// <value>The json representation.</value>
        protected String JsonRepresentation
        {
            get { return _JsonRepresentation; }
        }

        /// <summary>
        /// Patches the specified object using Json.net.
        /// </summary>
        /// <param name="original">The original object to patch.</param>
        /// <returns>IServiceResponse{T}.</returns>
        public virtual IServiceResponse<T> Patch(T original)
        {
            return Patch(original, (PatchedAction<T>) null);
        }

        /// <summary>
        /// Patches the specified object using Json.net.
        /// </summary>
        /// <param name="original">The original object to patch.</param>
        /// <param name="afterPatch">The after patch action.</param>
        /// <param name="settings">Json serializer settings for use</param>
        /// <returns>IServiceResponse{T}.</returns>
        public virtual IServiceResponse<T> Patch(T original, PatchedAction<T> afterPatch, JsonSerializerSettings settings = null)
        {
            JsonConvert.PopulateObject(JsonRepresentation, original, settings ?? new JsonSerializerSettings());

            if (OnPatchedHandler != null)
            {
                OnPatchedHandler.Invoke(original);
            }

            if (afterPatch != null)
            {
                afterPatch(original);
            }

            return new DataResponse<T>(original);
        }

        /// <summary>
        /// Patches the specified object using Json.net.
        /// </summary>
        /// <param name="original">The original object to patch.</param>
        /// <param name="afterPatch">The after patch action.</param>
        /// <param name="settings">Json serializer settings for use</param>
        /// <returns>IServiceResponse{T}.</returns>
        public virtual IServiceResponse<T> Patch(T original, ClonedPatchedAction<T> afterPatch, JsonSerializerSettings settings = null)
        {
            T clone = null;
            if (afterPatch != null)
            {
                clone = original.Copy();
            }

            JsonConvert.PopulateObject(JsonRepresentation, original, settings ?? new JsonSerializerSettings());

            if (OnPatchedHandler != null)
            {
                OnPatchedHandler.Invoke(original);
            }

            if (afterPatch != null)
            {
                afterPatch(clone, original);
            }

            return new DataResponse<T>(original);
        }
    }
}