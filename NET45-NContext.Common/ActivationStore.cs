namespace NContext.Common
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// A reflection-based dynamic factory for creating instances of types.
    /// Instance creation is cached based upon ConstructorInfo for the supplied type/args combination.
    /// </summary>
    public class ActivationStore
    {
        private readonly ConcurrentDictionary<ConstructorInfo, Func<Object[], Object>> _InternalStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivationStore"/> class.
        /// </summary>
        public ActivationStore()
        {
            _InternalStore = new ConcurrentDictionary<ConstructorInfo, Func<Object[], Object>>();
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>Object.</returns>
        public Object CreateInstance(Type type, params Object[] args)
        {
            return CreateInstance<Object>(type, args);
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>T.</returns>
        public T CreateInstance<T>(Type type, params Object[] args)
        {
            Type[] argTypes = args.Select(a => a.GetType()).ToArray();

            var ctor = type.GetTypeInfo().DeclaredConstructors
                .Single(c => !c.IsStatic && ParametersMatch(c.GetParameters(), argTypes));

            return (T)CreateInstance(ctor, args);
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="ctor">The ctor.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>Object.</returns>
        public Object CreateInstance(ConstructorInfo ctor, Object[] args)
        {
            return _InternalStore.GetOrAdd(ctor, GetActivator(ctor)).Invoke(args);
        }

        private Func<Object[], Object> GetActivator(ConstructorInfo ctor)
        {
            var paramsInfo = ctor.GetParameters();
            var param = Expression.Parameter(typeof(Object[]), "args");
            var argsExp = new Expression[paramsInfo.Length];

            // Pick each arg from the params array and create a typed expression of them
            for (var i = 0; i < paramsInfo.Length; i++)
            {
                var index = Expression.Constant(i);
                var paramAccessorExp = Expression.ArrayIndex(param, index);
                var paramCastExp = Expression.Convert(paramAccessorExp, paramsInfo[i].ParameterType);

                argsExp[i] = paramCastExp;
            }

            // Make a NewExpression that calls the constructor with the args we just created
            var ctorExpression = Expression.New(ctor, argsExp);

            // Create a lambda with the NewExpression as body and our param Object[] as arg
            var factory = Expression.Lambda(typeof(Func<Object[], Object>), ctorExpression, param);

            return (Func<Object[], Object>)factory.Compile();
        }

        private static Boolean ParametersMatch(ParameterInfo[] parameters, Type[] constructorParameterTypes)
        {
            if (parameters.Length != constructorParameterTypes.Length)
            {
                return false;
            }

            for (Int32 i = 0; i < parameters.Length; i++)
            {
                Type parameterType = parameters[i].ParameterType;
                Type constructorParameterType = constructorParameterTypes[i];

                Type enumerable1, enumerable2;
                if (IsEnumerable(parameterType, out enumerable1) && IsEnumerable(constructorParameterType, out enumerable2))
                {
                    parameterType = enumerable1;
                    constructorParameterType = enumerable2;
                }

                if (parameterType != constructorParameterType)
                {
                    return false;
                }
            }

            return true;
        }

        private static Boolean IsEnumerable(Type type, out Type enumerable)
        {
            if (type == null)
            {
                enumerable = null;
                return false;
            }

            var typeInfo = type.GetTypeInfo();

            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                enumerable = type;
            }
            else
            {
                enumerable = typeInfo.ImplementedInterfaces
                    .SingleOrDefault(interfaceType =>
                        (interfaceType.GetTypeInfo().IsGenericType &&
                         interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>)));
            }

            return enumerable != null;
        }
    }
}