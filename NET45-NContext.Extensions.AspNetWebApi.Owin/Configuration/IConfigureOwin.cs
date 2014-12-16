namespace NContext.Extensions.AspNetWebApi.Owin.Configuration
{
    using System.ComponentModel.Composition;

    using global::Owin;

    /// <summary>
    /// Defines an extensibility point for configuring <see cref="IAppBuilder"/>.
    /// </summary>
    [InheritedExport]
    public interface IConfigureOwin
    {
        /// <summary>
        /// Gets the priority based upon the <see cref="PipelineStage"/>. By default, OMCs run at the last event (<see cref="PipelineStage.PreHandlerExecute"/>).
        /// </summary>
        /// <value>The priority.</value>
        PipelineStage Priority { get; }

        /// <summary>
        /// Called by NContext to support OWIN configuration.
        /// </summary>
        /// <param name="appBuilder">The application builder.</param>
        void Configure(IAppBuilder appBuilder);
    }
}