namespace NContext.EventHandling
{
    using System;
    using System.ComponentModel.Composition;

    [InheritedExport]
    public interface IHandleEvent<TEvent>
    {
        void Handle(TEvent @event);

        void HandleException(TEvent @event, Exception exception);
    }
}