namespace NContext.EventHandling
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class HandleConcurrently : Attribute
    {
    }
}