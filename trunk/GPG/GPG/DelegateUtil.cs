namespace GPG
{
    using System;
    using System.Reflection;

    public static class DelegateUtil
    {
        public static Delegate CreateGeneric(MethodInfo method)
        {
            Type type = typeof(GenericDelegate<>);
            return Delegate.CreateDelegate(type.MakeGenericType(new Type[] { method.ReturnType }), method);
        }
    }
}

