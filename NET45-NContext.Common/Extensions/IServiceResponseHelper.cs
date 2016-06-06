namespace NContext.Common
{
    using System;
    using System.Linq;
    using System.Reflection;

    public static class IServiceResponseHelper
    {
        internal static IServiceResponse<T> CreateGenericDataResponse<T>(this IServiceResponse<T> originalResponse, T data)
        {
            if (IsBuiltInDataResponse(originalResponse))
            {
                return new DataResponse<T>(data);
            }

            try
            {
                return Activator.CreateInstance(
                    originalResponse.GetType()
                        .GetGenericTypeDefinition()
                        .MakeGenericType(typeof(T)),
                    data) as IServiceResponse<T>;
            }
            catch (TargetInvocationException)
            {
                // No supportable constructor found! Return default.
                return new DataResponse<T>(data);
            }
        }

        internal static IServiceResponse<T2> CreateGenericDataResponse<T, T2>(this IServiceResponse<T> originalResponse, T2 data)
        {
            if (IsBuiltInDataResponse(originalResponse))
            {
                return new DataResponse<T2>(data);
            }

            try
            {
                return Activator.CreateInstance(
                    originalResponse.GetType()
                        .GetGenericTypeDefinition()
                        .MakeGenericType(typeof(T2)),
                    data) as IServiceResponse<T2>;
            }
            catch (TargetInvocationException)
            {
                // No supportable constructor found! Return default.
                return new DataResponse<T2>(data);
            }
        }

        internal static IServiceResponse<T> CreateGenericErrorResponse<T>(this IServiceResponse<T> originalResponse, Error error)
        {
            if (IsBuiltInErrorResponse(originalResponse))
            {
                return new ErrorResponse<T>(error);
            }

            try
            {
                return Activator.CreateInstance(
                    originalResponse.GetType()
                        .GetGenericTypeDefinition()
                        .MakeGenericType(typeof(T)),
                    error) as IServiceResponse<T>;
            }
            catch (TargetInvocationException)
            {
                // No supportable constructor found! Return default.
                return new ErrorResponse<T>(error);
            }
        }

        internal static IServiceResponse<T2> CreateGenericErrorResponse<T, T2>(this IServiceResponse<T> originalResponse, Error error)
        {
            if (IsBuiltInErrorResponse(originalResponse))
            {
                return new ErrorResponse<T2>(error);
            }

            try
            {
                return Activator.CreateInstance(
                    originalResponse.GetType()
                        .GetGenericTypeDefinition()
                        .MakeGenericType(typeof(T2)),
                    error) as IServiceResponse<T2>;
            }
            catch (TargetInvocationException)
            {
                // No supportable constructor found! Return default.
                return new ErrorResponse<T2>(error);
            }
        }

        internal static Error ToError(this Exception exception)
        {
            return new Error(500, exception.GetType().Name, new[] { exception.Message });
        }

        internal static Error ToError(this AggregateException aggregateException)
        {
            return new AggregateError(
                500,
                aggregateException.GetType().Name,
                aggregateException.InnerExceptions.Select(e => e.ToError()));
        }

        private static Boolean IsBuiltInDataResponse<T>(this IServiceResponse<T> originalResponse)
        {
            var typeInfo = originalResponse.GetType().GetTypeInfo();
            return originalResponse is DataResponse<T> ||
                   (typeInfo.IsGenericType && typeof(DataResponse<>)
                   .GetTypeInfo()
                   .IsAssignableFrom(typeInfo.GetGenericTypeDefinition().GetTypeInfo()));
        }

        private static Boolean IsBuiltInErrorResponse<T>(this IServiceResponse<T> originalResponse)
        {
            var typeInfo = originalResponse.GetType().GetTypeInfo();
            return originalResponse is ErrorResponse<T> ||
                   (typeInfo.IsGenericType && typeof(ErrorResponse<>)
                   .GetTypeInfo()
                   .IsAssignableFrom(typeInfo.GetGenericTypeDefinition().GetTypeInfo()));
        }
    }
}