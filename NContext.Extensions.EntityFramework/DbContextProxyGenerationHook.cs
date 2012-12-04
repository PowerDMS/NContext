namespace NContext.Extensions.EntityFramework
{
    using System;
    using System.Reflection;

    using Castle.DynamicProxy;

    internal class DbContextProxyGenerationHook : IProxyGenerationHook
    {
        public void MethodsInspected()
        {
        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
        }

        public Boolean ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return methodInfo.IsFamily && methodInfo.Name.Equals("Dispose", StringComparison.OrdinalIgnoreCase);
        }
    }
}