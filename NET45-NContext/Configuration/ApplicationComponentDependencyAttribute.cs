namespace NContext.Configuration
{
    using System;

    using NContext.Extensions;

    // TODO: (DG) Implement NContext Dependency Tree Support
    public class ApplicationComponentDependencyAttribute : Attribute
    {
        public ApplicationComponentDependencyAttribute(Type dependency)
        {
            if (!dependency.Implements<IApplicationComponent>())
            {
                throw new InvalidOperationException("Application component dependency type must implement IApplicationComponent.");
            }
        }
    }
}