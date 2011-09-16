using System;
using System.Collections.Generic;
using System.Threading;

namespace NContext.Persistence.EntityFramework
{
    public static class UnitOfWorkController
    {
        private static readonly ThreadLocal<Stack<Tuple<Int32, IUnitOfWork>>> _AmbientUnitsOfWork = 
            new ThreadLocal<Stack<Tuple<Int32, IUnitOfWork>>>(() => new Stack<Tuple<Int32, IUnitOfWork>>());

        /// <summary>
        /// Gets the ambient <see cref="IUnitOfWork"/>.
        /// </summary>
        /// <value>The ambient unit of work.</value>
        /// <remarks></remarks>
        public static IUnitOfWork AmbientUnitOfWork
        {
            get
            {
                return _AmbientUnitsOfWork.Value.Count > 0
                    ? _AmbientUnitsOfWork.Value.Peek().Item2
                    : null;
            }
        }

        public static void AddUnitOfWork(IUnitOfWork unitOfWork)
        {
            _AmbientUnitsOfWork.Value.Push(new Tuple<Int32, IUnitOfWork>(_AmbientUnitsOfWork.Value.Count + 1, unitOfWork));
        }
        
        public static void Retain()
        {
            var tuple = _AmbientUnitsOfWork.Value.Pop();
            var retainCount = tuple.Item1;
            var uow = tuple.Item2;

            _AmbientUnitsOfWork.Value.Push(new Tuple<Int32, IUnitOfWork>(retainCount + 1, uow));
        }

        public static Boolean DisposeUnitOfWork()
        {
            var uow = _AmbientUnitsOfWork.Value.Pop();
            if (uow.Item1 > 1)
            {
                _AmbientUnitsOfWork.Value.Push(new Tuple<Int32, IUnitOfWork>(uow.Item1 - 1, uow.Item2));

                return false;
            }

            return true;
        }
    }
}