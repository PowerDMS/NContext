namespace NContext.Extensions.EntityFramework
{
    using System;

    using Castle.DynamicProxy;

    internal class DisposeInterceptor : IInterceptor, IDisposableMixin
    {
        private Func<bool> _CanDispose;

        public void SetDisposePredicate(Func<Boolean> canDisposeFunc)
        {
            if (canDisposeFunc == null)
            {
                throw new ArgumentNullException("canDisposeFunc");
            }

            _CanDispose = canDisposeFunc;
        }

        public void Intercept(IInvocation invocation)
        {
            if (_CanDispose == null) return;

            if (_CanDispose())
            {
                invocation.Proceed();
            }
        }
    }
}