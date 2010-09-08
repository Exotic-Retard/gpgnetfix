namespace GPG.Threading
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate object ControlInvocationHandler(Delegate invokeTarget, params object[] invokeParams);
}

