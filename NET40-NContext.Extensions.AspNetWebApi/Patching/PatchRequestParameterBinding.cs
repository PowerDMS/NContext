namespace NContext.Extensions.AspNetWebApi.Patching
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Metadata;
    using System.Web.Http.Validation;

    using NContext.Common;
    using NContext.Extensions.AspNetWebApi.Filters;

    /// <summary>
    /// Parameter binding that will read from the body to create a new <see cref="PatchRequest{T}"/>
    /// </summary>
    public class PatchRequestParameterBinding : HttpParameterBinding
    {
        private static readonly ConcurrentDictionary<Type, ConstructorInfo> _TypeConstructorCache;

        private static readonly Func<Type, ConstructorInfo> _PatchRequestConstructorFactory;

        private readonly IBodyModelValidator _BodyModelValidator;

        static PatchRequestParameterBinding()
        {
            _TypeConstructorCache = new ConcurrentDictionary<Type, ConstructorInfo>();
            _PatchRequestConstructorFactory =
            type =>
            {
                var patchRequest = typeof(PatchRequest<>).MakeGenericType(type);
                var ctor = patchRequest.GetConstructor(new[] { typeof(String) });
                if (ctor == null)
                {
                    throw new InvalidOperationException(
                        String.Format("The type: {0} must contain a default constructor.", type.FullName));
                }

                return ctor;
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatchRequestParameterBinding"/> class.
        /// </summary>
        /// <param name="descriptor">The descriptor.</param>
        /// <param name="bodyModelValidator">The body model validator.</param>
        public PatchRequestParameterBinding(HttpParameterDescriptor descriptor, IBodyModelValidator bodyModelValidator)
            : base(descriptor)
        {
            _BodyModelValidator = bodyModelValidator;
        }

        /// <summary>
        /// Executes the binding asynchronous.
        /// </summary>
        /// <param name="metadataProvider">The metadata provider.</param>
        /// <param name="actionContext">The action context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>System.Threading.Tasks.Task.</returns>
        public override async Task ExecuteBindingAsync(
            ModelMetadataProvider metadataProvider,
            HttpActionContext actionContext,
            CancellationToken cancellationToken)
        {
            PatchRequestBase model = await CreatePatchRequest(actionContext.Request, Descriptor.Configuration);

            SetValue(actionContext, model);

            if (_BodyModelValidator != null)
            {
                _BodyModelValidator.Validate(model, Descriptor.ParameterType, metadataProvider, actionContext, Descriptor.ParameterName);
            }
        }

        /// <summary>
        /// Creates the <see cref="PatchRequest{T}"/> instance.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>System.Threading.Tasks.Task&lt;NContext.Extensions.AspNetWebApi.Patching.PatchRequestBase&gt;.</returns>
        protected virtual async Task<PatchRequestBase> CreatePatchRequest(HttpRequestMessage request, HttpConfiguration configuration)
        {
            if (request.Method != HttpMethod.Put && 
                request.Method != new HttpMethod("PATCH"))
            {
                return null;
            }

            var jsonMediaTypeFormatter = configuration.Formatters.JsonFormatter;
            var supportedMediaTypes = jsonMediaTypeFormatter.SupportedMediaTypes.Select(mediaType => mediaType.MediaType);
            var content = request.Content;
            if (content == null ||
                content.Headers.ContentType == null ||
                !supportedMediaTypes.Contains(content.Headers.ContentType.MediaType, StringComparer.InvariantCultureIgnoreCase))
            {
                return null;
            }

            var patchJson = await request.Content.ReadAsStringAsync();
            if (String.IsNullOrWhiteSpace(patchJson))
            {
                return null;
            }

            var patchType = Descriptor.ParameterType.GetGenericArguments()[0];
            var patchRequest = (PatchRequestBase)_TypeConstructorCache.GetOrAdd(patchType, _PatchRequestConstructorFactory).Invoke(new Object[] { patchJson });

            HttpParameterBindingSanitizerFilter sanitizationFilter = configuration.Filters
                .MaybeSingle(filter => filter.Instance.GetType() == typeof(HttpParameterBindingSanitizerFilter))
                .Bind(filterInfo => ((HttpParameterBindingSanitizerFilter)filterInfo.Instance).ToMaybe())
                .FromMaybe(null);

            if (sanitizationFilter != null)
            {
                patchRequest.SetOnPatchedHandler(sanitizationFilter.SanitizeObjectGraph);
            }

            return patchRequest;
        }

        /// <summary>
        /// Gets the will read body.
        /// </summary>
        /// <value>The will read body.</value>
        public override Boolean WillReadBody
        {
            get { return true; }
        }
    }
}