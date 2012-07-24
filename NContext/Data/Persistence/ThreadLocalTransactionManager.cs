
namespace NContext.Data.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary> 
    /// Defines a per-thread transaction lifetime manager for data persistence. 
    /// Each thread will maintain its own <see cref="AmbientUnitOfWork"/> stack.
    /// </summary>
    /// <remarks></remarks>
    public class ThreadLocalTransactionManager : AmbientTransactionManagerBase
    {
        private static readonly ThreadLocal<Stack<AmbientUnitOfWork>> _AmbientUnitsOfWork =
            new ThreadLocal<Stack<AmbientUnitOfWork>>(() => new Stack<AmbientUnitOfWork>());

        public override Boolean AmbientExists
        {
            get
            {
                return _AmbientUnitsOfWork.IsValueCreated && _AmbientUnitsOfWork.Value.Count > 0;
            }
        }

        protected override Stack<AmbientUnitOfWork> AmbientUnitsOfWork
        {
            get
            {
                return _AmbientUnitsOfWork.Value;
            }
        }
    }
}