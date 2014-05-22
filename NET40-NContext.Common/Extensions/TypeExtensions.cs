namespace NContext.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading;

    public static class TypeExtensions
    {
        private static readonly ActivationStore _ActivationStore = new ActivationStore();

        public static Object CreateInstance(this Type type, params Object[] args)
        {
            return CreateInstance<Object>(type, args);
        }

        public static T CreateInstance<T>(this Type type, params Object[] args)
        {
            Type[] argTypes = args.Select(a => a.GetType()).ToArray();
            var ctor = type.GetConstructor(argTypes);

            return (T)_ActivationStore.CreateInstance(ctor, args);
        }

        private class ActivationStore
        {
            private readonly ReadWriteDictionary<ConstructorInfo, Func<Object[], Object>> _InternalStore;

            public ActivationStore()
            {
                _InternalStore = new ReadWriteDictionary<ConstructorInfo, Func<Object[], Object>>();
            }

            public Object CreateInstance(ConstructorInfo ctor, Object[] args)
            {
                return GetActivator(ctor).Invoke(args);
            }

            private Func<Object[], Object> GetActivator(ConstructorInfo ctor)
            {
                var func = _InternalStore.Get(ctor);
                if (func != null)
                {
                    return func;
                }

                ParameterInfo[] paramsInfo = ctor.GetParameters();

                //create a single param of type object[]
                ParameterExpression param = Expression.Parameter(typeof(object[]), "args");

                var argsExp = new Expression[paramsInfo.Length];

                //pick each arg from the params array
                //and create a typed expression of them
                for (int i = 0; i < paramsInfo.Length; i++)
                {
                    Expression index = Expression.Constant(i);

                    Expression paramAccessorExp = Expression.ArrayIndex(param, index);

                    Expression paramCastExp = Expression.Convert(paramAccessorExp, paramsInfo[i].ParameterType);

                    argsExp[i] = paramCastExp;
                }

                //make a NewExpression that calls the ctor with the args we just created
                NewExpression newExp = Expression.New(ctor, argsExp);

                //create a lambda with the NewExpression as body and our param object[] as arg
                LambdaExpression lambda = Expression.Lambda(typeof(Func<Object[], Object>), newExp, param);

                //compile it
                var compiled = (Func<Object[], Object>)lambda.Compile();

                _InternalStore.Set(ctor, compiled);

                return compiled;
            }
        }

        private class ReadWriteDictionary<K, V>
        {
            private readonly Dictionary<K, V> dict = new Dictionary<K, V>();
            private readonly ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();

            public V Get(K key)
            {
                return ReadLock(() => dict[key]);
            }

            public void Set(K key, V value)
            {
                WriteLock(() => dict.Add(key, value));
            }

            public IEnumerable<KeyValuePair<K, V>> GetPairs()
            {
                return ReadLock(() => dict.ToList());
            }

            private V2 ReadLock<V2>(Func<V2> func)
            {
                rwLock.EnterReadLock();
                try
                {
                    return func();
                }
                finally
                {
                    rwLock.ExitReadLock();
                }
            }

            private void WriteLock(Action action)
            {
                rwLock.EnterWriteLock();
                try
                {
                    action();
                }
                finally
                {
                    rwLock.ExitWriteLock();
                }
            }
        }
    }
}