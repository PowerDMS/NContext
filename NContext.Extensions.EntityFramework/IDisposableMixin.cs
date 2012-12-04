namespace NContext.Extensions.EntityFramework
{
    using System;

    internal interface IDisposableMixin
    {
        void SetDisposePredicate(Func<Boolean> canDisposeFunc);
    }
}