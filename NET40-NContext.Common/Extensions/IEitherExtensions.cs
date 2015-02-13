namespace NContext.Common
{
    using System;
    using System.Threading.Tasks;

    public static class IEitherExtensions
    {
        public static T3 Fold<T, T2, T3>(this IEither<T, T2> either, Func<T, T3> leftFunc, Func<T2, T3> rightFunc)
        {
            if (either.IsLeft) return leftFunc(either.GetLeft());

            return rightFunc(either.GetRight());
        }

        public static Task<T3> FoldAsync<T, T2, T3>(this IEither<T, T2> either, Func<T, Task<T3>> leftFunc, Func<T2, Task<T3>> rightFunc)
        {
            if (either.IsLeft) return leftFunc(either.GetLeft());

            return rightFunc(either.GetRight());
        }

        public static IEither<T, T2> JoinLeft<T, T2>(this IEither<IEither<T, T2>, T2> either)
        {
            if (either.IsRight) return new Right<T, T2>(either.GetRight());

            return either.GetLeft();
        }

        public static IEither<T, T2> JoinRight<T, T2>(this IEither<T, IEither<T, T2>> either)
        {
            if (either.IsLeft) return new Left<T, T2>(either.GetLeft());

            return either.GetRight();
        }
    }
}