namespace NContext.Data.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary> 
    /// Defines an ambient-context manager for per-thread storage. Each thread will 
    /// maintain its own <see cref="AmbientUnitOfWorkDecorator"/> stack.
    /// </summary>
    /// <remarks></remarks>
    public class PerThreadAmbientContextManager : AmbientContextManagerBase
    {
        private static ThreadLocal<Stack<AmbientUnitOfWorkDecorator>> _AmbientUnitsOfWork =
            new ThreadLocal<Stack<AmbientUnitOfWorkDecorator>>(() => new Stack<AmbientUnitOfWorkDecorator>());

        /// <summary>
        /// Gets whether the ambient context exists.
        /// </summary>
        /// <value>The ambient exists.</value>
        public override Boolean AmbientExists
        {
            get
            {
                return _AmbientUnitsOfWork.IsValueCreated && _AmbientUnitsOfWork.Value.Count > 0;
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
        protected internal override Stack<AmbientUnitOfWorkDecorator> AmbientUnitsOfWork
        {
            get
            {
                return _AmbientUnitsOfWork.Value;
            }
        }
    }
}