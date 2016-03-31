namespace NContext.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Web;

    /// <summary>
    /// Defines a fluent interface builder for application configuration.
    /// </summary>
    /// <remarks></remarks>
    public class ApplicationConfigurationBuilder
    {
        private readonly ApplicationConfiguration _ApplicationConfiguration =
            new ApplicationConfiguration();

        private readonly Dictionary<Type, ApplicationComponentBuilder> _Components =
            new Dictionary<Type, ApplicationComponentBuilder>();
        
        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        /// <remarks></remarks>
        public virtual ApplicationConfiguration ApplicationConfiguration
        {
            get
            {
                return _ApplicationConfiguration;
            }
        }

        public virtual IDictionary<Type, ApplicationComponentBuilder> Components
        {
            get { return _Components; }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ApplicationConfigurationBuilder"/> 
        /// to <see cref="Configuration.ApplicationConfiguration"/>.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration builder.</param>
        /// <returns>The result of the conversion.</returns>
        /// <remarks></remarks>
        public static implicit operator ApplicationConfiguration(ApplicationConfigurationBuilder applicationConfigurationBuilder)
        {
            return applicationConfigurationBuilder.ApplicationConfiguration;
        }

        /// <summary>
        /// Composes the application for web environments. This will use the <see cref="HttpRuntime.BinDirectory"/> for runtime composition.
        /// </summary>
        /// <param name="fileInfoConstraints">The file info constraints.</param>
        /// <returns>Current <see cref="ApplicationComponentBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public ApplicationConfigurationBuilder ComposeForWeb(params Predicate<FileInfo>[] fileInfoConstraints)
        {
            if (HttpContext.Current == null)
            {
                throw new InvalidOperationException("HttpContext is null. This method can only be used within a web application.");
            }

            ComposeWith(new[] { HttpRuntime.BinDirectory }, fileInfoConstraints);

            return this;
        }

        /// <summary>
        /// Composes the application using either the entry assembly location 
        /// or <see cref="AppDomain.CurrentDomain"/> BaseDirectory for runtime composition.
        /// </summary>
        /// <param name="fileInfoConstraints">The file info constraints.</param>
        /// <returns>Current <see cref="ApplicationComponentBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public ApplicationConfigurationBuilder ComposeWith(params Predicate<FileInfo>[] fileInfoConstraints)
        {
            var applicationLocation = Assembly.GetEntryAssembly() == null
                                          ? AppDomain.CurrentDomain.BaseDirectory
                                          : Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            ComposeWith(new[] { applicationLocation }, fileInfoConstraints);

            return this;
        }

        /// <summary>
        /// Composes the application using the specified directories only, for runtime composition. This can 
        /// be used in conjunction with its overload and <seealso cref="ComposeForWeb"/>.
        /// </summary>
        /// <param name="directories">The directories.</param>
        /// <param name="fileInfoConstraints">The file info constraints.</param>
        /// <returns>Current <see cref="ApplicationComponentBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public ApplicationConfigurationBuilder ComposeWith(IEnumerable<String> directories, params Predicate<FileInfo>[] fileInfoConstraints)
        {
            ApplicationConfiguration.AddCompositionConditions(directories, fileInfoConstraints);

            return this;
        }

        /// <summary>
        /// Registers the component with the <see cref="ApplicationConfigurationBuilder"/> components enumerator.
        /// </summary>
        /// <typeparam name="TApplicationComponent">The type of the application component.</typeparam>
        /// <returns>Current <see cref="ApplicationComponentBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public ApplicationComponentBuilder RegisterComponent<TApplicationComponent>()
            where TApplicationComponent : class, IApplicationComponent
        {
            ApplicationComponentBuilder section;
            if (Components.TryGetValue(typeof(TApplicationComponent), out section))
            {
                return section;
            }

            var componentConfigurationBuilder = (ApplicationComponentBuilder)Activator.CreateInstance(typeof(ApplicationComponentBuilder), this);
            Components.Add(typeof(TApplicationComponent), componentConfigurationBuilder);

            return componentConfigurationBuilder;
        }

        /// <summary>
        /// Registers the component with the <see cref="ApplicationConfigurationBase"/> instance.
        /// </summary>
        /// <typeparam name="TApplicationComponent">The type of the application component.</typeparam>
        /// <param name="componentFactory">The component factory.</param>
        /// <returns>Current <see cref="ApplicationComponentBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public ApplicationConfigurationBuilder RegisterComponent<TApplicationComponent>(Func<TApplicationComponent> componentFactory)
            where TApplicationComponent : class, IApplicationComponent
        {
            ApplicationConfiguration.RegisterComponent<TApplicationComponent>(componentFactory);

            return this;
        }
    }
}