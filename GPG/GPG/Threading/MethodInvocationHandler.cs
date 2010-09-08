namespace GPG.Threading
{
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public delegate object MethodInvocationHandler(object target, MethodInfo method, params object[] invokeParameters);
}

