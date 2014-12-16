namespace NContext.Data.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    /// <summary>
    /// Defines an ambient-context manager for web applications. Each web request 
    /// will maintain its own <see cref="AmbientUnitOfWorkDecorator"/> stack.
    /// </summary>
    public class PerRequestAmbientContextManager : AmbientContextManagerBase
    {
        protected const String AmbientUnitsOfWorkKey = @"AmbientUnitsOfWork";

        /// <summary>
        /// Initializes a new instance of the <see cref="PerRequestAmbientContextManager" /> class.
        /// </summary>
        /// <exception cref="System.ApplicationException"></exception>
        public PerRequestAmbientContextManager()
        {
            if (String.IsNullOrWhiteSpace(HttpRuntime.AppDomainAppVirtualPath))
            {
                throw new ApplicationException("PerRequestAmbientContextManager can only be used in a web application.");
            }
        }

        /// <summary>
        /// Gets whether the ambient context exists.
        /// </summary>
        /// <value>The ambient exists.</value>
        public override Boolean AmbientExists
        {
            get
            {
                return HttpContext.Current != null && 
                       HttpContext.Current.Items.Contains(AmbientUnitsOfWorkKey) && 
                       AmbientUnitsOfWork.Count > 0;
            }
        }

        /// <summary>
        /// Gets whether the <see cref="AmbientContextManagerBase"/> instance supports concurrency. This is 
        /// required if you set <see cref="PersistenceOptions.MaxDegreeOfParallelism"/> greater than one.
        /// </summary>
        protected internal override Boolean IsThreadSafe
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the ambient units of work.
        /// </summary>
        /// <value>The ambient units of work.</value>
        /// <exception cref="System.InvalidOperationException"></exception>
        protected internal override Stack<AmbientUnitOfWorkDecorator> AmbientUnitsOfWork
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    throw new InvalidOperationException("Cannot retrieve ambient units of work without an active HttpContext.");
                }

                var ambientUnitsOfWork = HttpContext.Current.Items[AmbientUnitsOfWorkKey] as Stack<AmbientUnitOfWorkDecorator>;
                if (ambientUnitsOfWork == null)
                {
                    HttpContext.Current.Items[AmbientUnitsOfWorkKey] = ambientUnitsOfWork = new Stack<AmbientUnitOfWorkDecorator>();
                }

                return ambientUnitsOfWork;
            }
        }
    }
}