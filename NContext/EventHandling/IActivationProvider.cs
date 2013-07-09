namespace NContext.EventHandling
{
    using System;

    public interface IActivationProvider
    {
        IHandleEvent<TEvent> CreateInstance<TEvent>(Type handler);
    }
}