namespace NContext.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition.Hosting;
    using System.Linq;

    /// <summary>
    /// Defines extension methods for <see cref="CompositionContainer"/>.
    /// </summary>
    public static class CompositionContainerExtensions
    {
        private static readonly Object _SyncLock = new Object();

        private static volatile IEnumerable<Type> _ExportedTypes;

        /// <summary>
        /// Gets all types within the <see cref="CompositionContainer"/>'s <see cref="CompositionContainer.Catalog"/>.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>Enumeration of <see cref="Type"/>s.</returns>
        /// <remarks></remarks>
        public static IEnumerable<Type> GetExportTypes(this CompositionContainer container)
        {
            if (_ExportedTypes == null)
            {
                lock (_SyncLock)
                {
                    if (_ExportedTypes == null)
                    {
                        _ExportedTypes = container.Catalog.Parts
                            .Select(part => part.GetType().GetMethod("GetLazyPartType").Invoke(part, null))
                            .OfType<Lazy<Type>>()
                            .Select(lazyPart => lazyPart.Value)
                            .ToList();
                    }
                }
            }

            return _ExportedTypes;
        }

        /// <summary>
        /// Gets all types within the <see cref="CompositionContainer"/>'s <see cref="CompositionContainer.Catalog"/> 
        /// which implement the specified <typeparamref name="TExport"/>.
        /// </summary>
        /// <typeparam name="TExport">The type of the export.</typeparam>
        /// <param name="container">The container.</param>
        /// <returns>Enumeration of derived / implementing <see cref="Type"/>s.</returns>
        /// <remarks></remarks>
        public static IEnumerable<Type> GetExportTypesThatImplement<TExport>(this CompositionContainer container)
        {
            return container.GetExportTypes()
                .Where(typePart => typePart.IsAssignableToType(typeof(TExport))).ToList();
        }

        public static IEnumerable<Type> GetExportTypesThatImplement(this CompositionContainer container, Type type)
        {
            return container.GetExportTypes()
                .Where(typePart => typePart.IsAssignableToType(type));
        } 

        private static Boolean IsAssignableToType(this Type type, Type assignableType)
        {
            return 
                type.GetInterfaces()
                .Any(
                @interface => 
                    @interface == assignableType || 
                    @interface.IsGenericType && @interface.GetGenericTypeDefinition() == assignableType) || 
                    (type.IsGenericType && type.GetGenericTypeDefinition() == assignableType) || 
                    (type.BaseType != null && IsAssignableToType(type.BaseType, assignableType));
        }
    }
}