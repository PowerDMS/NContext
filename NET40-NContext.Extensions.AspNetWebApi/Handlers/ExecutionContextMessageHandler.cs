namespace NContext.Extensions.AspNetWebApi.Handlers
{
    using System;
    using System.Net.Http;
    using System.Runtime.Remoting.Messaging;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines a message handler which creates an identifier for the incoming ExecutionContext flow.
    /// This can be later used across asyncronous operations and threads to uniquely identify the execution context.
    /// This may be useful if you wish to cache data per-request (ie. HttpContext.Current.Items). Since a single 
    /// request/response may execute across multiple threads we need to identify the data somehow. This is similar to how
    /// Thread.CurrentPrincipal and Culture work.
    /// </summary>
    public class ExecutionContextMessageHandler : DelegatingHandler
    {
        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>Returns <see cref="T:System.Threading.Tasks.Task`1" />. The task object representing the asynchronous operation.</returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            SetExecutionContextId(request.GetCorrelationId());

            return base.SendAsync(request, cancellationToken);
        }

        private void SetExecutionContextId(Guid executionContextId)
        {
            CallContext.LogicalSetData("ecid", executionContextId.ToString());
        }
    }
}