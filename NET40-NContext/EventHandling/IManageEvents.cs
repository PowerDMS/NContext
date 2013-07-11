namespace NContext.EventHandling
{
    using System.Threading.Tasks;

    using NContext.Configuration;

    public interface IManageEvents : IApplicationComponent
    {
        Task Raise<TEvent>(TEvent @event);
    }
}