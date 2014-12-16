namespace NContext.Extensions.AspNetWebApi.Patching
{
    using System;

    /// <summary>
    /// Defines a transfer object for HTTP PATCH support.
    /// </summary>
    public abstract class PatchRequestBase
    {
        private Action<Object> _OnPatchedHandler;

        /// <summary>
        /// Gets the on patched handler.
        /// </summary>
        /// <value>The on patched handler.</value>
        internal Action<Object> OnPatchedHandler
        {
            get { return _OnPatchedHandler; }
        }

        /// <summary>
        /// Sets the on patched handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        internal void SetOnPatchedHandler(Action<Object> handler)
        {
            _OnPatchedHandler = handler;
        }
    }
}