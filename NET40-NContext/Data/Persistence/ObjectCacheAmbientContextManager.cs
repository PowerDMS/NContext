namespace NContext.Data.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Caching;

    /// <summary> 
    /// Defines an ambient-context manager for cache-level storage. The cache maintains the <see cref="AmbientUnitOfWorkDecorator"/> stack.
    /// </summary>
    public class ObjectCacheAmbientContextManager : AmbientContextManagerBase
    {
        private const String _AmbientUnitsOfWorkCacheKey = @"NContextAmbientUnitsOfWork";

        private readonly ObjectCache _Cache;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectCacheAmbientContextManager" /> class.
        /// </summary>
        /// <param name="cache">The cache.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="cache"/> is null.</exception>
        public ObjectCacheAmbientContextManager(ObjectCache cache)
        {
            if (cache == null)
            {
                throw new ArgumentNullException("cache");
            }

            _Cache = cache;
        }

        /// <summary>
        /// Gets the ambient exists.
        /// </summary>
        /// <value>The ambient exists.</value>
        public override Boolean AmbientExists
        {
            get
            {
                return AmbientUnitsOfWork.Count > 0;
            }
        }

        /// <summary>
        /// Gets whether the <see cref="AmbientContextManagerBase"/> instance supports concurrency. This is 
        /// required if you set <see cref="PersistenceOptions.MaxDegreeOfParallelism"/> greater than one.
        /// </summary>
        /// <returns>
        ///   <c>true</c>, however, this should only be used in scenarios where only one 
        ///   active session or user is using the application (ie. a desktop application)
        /// </returns>
        protected internal override Boolean IsThreadSafe
        {
            get
            {
                return true;
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
                var ambientUnitsOfWork = _Cache[_AmbientUnitsOfWorkCacheKey] as Stack<AmbientUnitOfWorkDecorator>;
                if (ambientUnitsOfWork == null)
                {
                    _Cache[_AmbientUnitsOfWorkCacheKey] = ambientUnitsOfWork = new Stack<AmbientUnitOfWorkDecorator>();
                }

                return ambientUnitsOfWork;
            }
        }
    }
}