namespace GPG.Threading
{
    using GPG.Logging;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public class ThreadQueue : IDisposable
    {
        private bool _Disposed;
        private bool mIsSuspended;
        private bool mRunning;
        private Thread WorkerThread;

        public static bool LoggingOut = false;
        public static int QueueThreadID = 0;
        private static ThreadQueue sQueue = null;
        private static ThreadQueue _Quazal = CreateQueue(true);
        private Queue InnerQueue = Queue.Synchronized(new Queue());

        public event EventHandler Emptied;

        public ThreadQueue(bool autoStart)
        {
            if (autoStart)
            {
                this.Start();
            }
        }

        public void Clear()
        {
            this.InnerQueue.Clear();
        }

        public static ThreadQueue CreateQueue(bool autostart)
        {
            if (sQueue == null)
            {
                sQueue = new ThreadQueue(autostart);
            }
            return sQueue;
        }

        public void Dispose()
        {
            this._Disposed = true;
        }

        public void Enqueue(ThreadItem item)
        {
            this.InnerQueue.Enqueue(item);
            if ((this.Running && (this.WorkerThread != null)) && ((this.WorkerThread.ThreadState & ThreadState.Unstarted) == ThreadState.Unstarted))
            {
                this.WorkerThread.Start();
            }
        }

        public void Enqueue(Delegate target, params object[] invokeParams)
        {
            this.Enqueue(new ThreadItem(target.Target, target.Method, null, invokeParams));
        }

        public void Enqueue(Delegate target, Delegate callback, params object[] invokeParams)
        {
            this.Enqueue(new ThreadItem(target.Target, target.Method, callback.Method, invokeParams));
        }

        public void Enqueue(Control control, Delegate invoke, ThreadCallback callback, params object[] invokeParams)
        {
            this.Enqueue(new ControlInvocationHandler(control.Invoke), callback, new object[] { invoke, invokeParams });
        }

        public void Enqueue(object beginTarget, string begin, object endTarget, string end, params object[] invokeParams)
        {
            try
            {
                System.Type[] typeArray;
                MethodInfo invoke = null;
                MethodInfo callback = null;
                object invokeTarget = null;
                object callbackTarget = null;
                System.Type type = null;
                System.Type type2 = null;
                if (beginTarget is System.Type)
                {
                    type = beginTarget as System.Type;
                }
                else
                {
                    invokeTarget = beginTarget;
                    type = beginTarget.GetType();
                }
                if (beginTarget is Control)
                {
                    typeArray = new System.Type[] { typeof(Delegate), typeof(object[]) };
                    begin = "Invoke";
                    invokeParams = new object[] { new MethodInvocationHandler(this.OnControlInvoke), new object[] { invokeTarget, type.GetMethod(begin, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance), invokeParams } };
                }
                else
                {
                    typeArray = new System.Type[invokeParams.Length];
                    for (int i = 0; i < invokeParams.Length; i++)
                    {
                        typeArray[i] = invokeParams[i].GetType();
                    }
                }
                invoke = type.GetMethod(begin, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance, null, typeArray, null);
                if (invoke == null)
                {
                    invoke = type.GetMethod(begin, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
                }
                if (((endTarget != null) && (end != null)) && (end.Length > 0))
                {
                    if (endTarget is System.Type)
                    {
                        type2 = endTarget as System.Type;
                    }
                    else
                    {
                        callbackTarget = endTarget;
                        type2 = endTarget.GetType();
                    }
                    System.Type[] types = new System.Type[] { invoke.ReturnType };
                    callback = type2.GetMethod(end, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance, null, types, null);
                    if (callback == null)
                    {
                        callback = type2.GetMethod(end, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
                    }
                }
                this.Enqueue(new ThreadItem(invokeTarget, callbackTarget, invoke, callback, invokeParams));
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private object OnControlInvoke(object target, MethodInfo method, params object[] invokeParameters)
        {
            try
            {
                return method.Invoke(target, invokeParameters);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return null;
            }
        }

        protected virtual void OnEmptied()
        {
            if (this.Emptied != null)
            {
                this.Emptied(this, EventArgs.Empty);
            }
        }

        private void ProcessQueue()
        {
            if (this == sQueue)
            {
                QueueThreadID = Thread.CurrentThread.ManagedThreadId;
            }
            while ((this.Running && !this.Disposed) && !LoggingOut)
            {
                bool flag = false;
                if ((this.InnerQueue.Count > 0) && !this.IsSuspended)
                {
                    ThreadItem item = this.InnerQueue.Dequeue() as ThreadItem;
                    try
                    {
                        ArrayList list = new ArrayList(item.InvokeParams.Length);
                        List<int> list2 = new List<int>();
                        int index = 0;
                        ParameterInfo[] parameters = item.InvokeMethod.GetParameters();
                        foreach (ParameterInfo info in parameters)
                        {
                            if (info.IsOut)
                            {
                                object obj2 = null;
                                list.Add(obj2);
                                list2.Add(info.Position);
                            }
                            else if (index < item.InvokeParams.Length)
                            {
                                list.Add(item.InvokeParams[index]);
                                index++;
                            }
                        }
                        while (list.Count < parameters.Length)
                        {
                            list.Add(null);
                        }
                        object[] objArray = list.ToArray();
                        EventLog.WriteLine("Begin method invoke", new object[0]);
                        EventLog.WriteLine("Method Name: " + item.InvokeMethod.Name, new object[0]);
                        EventLog.WriteLine("Param Count: " + item.InvokeMethod.GetParameters().Length.ToString(), new object[0]);
                        foreach (ParameterInfo info2 in item.InvokeMethod.GetParameters())
                        {
                            EventLog.WriteLine("Parameter: " + info2.Name + " Type:" + info2.ParameterType.ToString(), new object[0]);
                        }
                        object obj3 = item.InvokeMethod.Invoke(item.InvokeTarget, objArray);
                        EventLog.WriteLine("End method invoke", new object[0]);
                        ArrayList list3 = new ArrayList(list2.Count + 1);
                        if (item.InvokeMethod.ReturnType != typeof(void))
                        {
                            list3.Add(obj3);
                        }
                        for (int i = 0; i < list2.Count; i++)
                        {
                            list3.Add(objArray[list2[i]]);
                        }
                        if ((item.CallbackMethod != null) && (item.CallbackTarget != null))
                        {
                            if (item.CallbackTarget is Control)
                            {
                                if (!(item.CallbackTarget as Control).Disposing && !(item.CallbackTarget as Control).IsDisposed)
                                {
                                    (item.CallbackTarget as Control).BeginInvoke(new MethodInvocationHandler(this.OnControlInvoke), new object[] { item.CallbackTarget, item.CallbackMethod, list3.ToArray() });
                                }
                            }
                            else
                            {
                                item.CallbackMethod.Invoke(item.CallbackTarget, list3.ToArray());
                            }
                        }
                        item = null;
                    }
                    catch (Exception exception)
                    {
                        ErrorLog.WriteLine("An error occured while invoking the following item:\r\n{0}", new object[] { item });
                        ErrorLog.WriteLine(exception);
                    }
                    flag = true;
                }
                if ((this.InnerQueue.Count < 1) || this.IsSuspended)
                {
                    if (flag)
                    {
                        this.OnEmptied();
                    }
                    Thread.Sleep(100);
                }
            }
        }

        public static bool QueueUserWorkItem(WaitCallback callBack, params object[] data)
        {
            object obj2 = data;
            Quazal.Enqueue(callBack, new object[] { obj2 });
            return true;
        }

        public void Resume()
        {
            this.mIsSuspended = false;
        }

        public void Start()
        {
            if (!this.Running)
            {
                this.mRunning = true;
                ThreadStart start = new ThreadStart(this.ProcessQueue);
                this.WorkerThread = new Thread(start);
                this.WorkerThread.IsBackground = true;
                this.WorkerThread.Start();
                EventLog.WriteLine("ThreadQueue {0} is now running", new object[] { this.ThreadID });
            }
        }

        public void Stop()
        {
            if (this.Running)
            {
                this.mRunning = false;
                EventLog.WriteLine("ThreadQueue {0} is no longer running", new object[] { this.ThreadID });
            }
        }

        public void Suspend()
        {
            this.mIsSuspended = true;
        }

        public static bool WaitUntil(ref bool condition, bool positive, int timeout)
        {
            int tickCount = Environment.TickCount;
            while (!condition.Equals(positive))
            {
                Thread.Sleep(10);
                if ((timeout > 0) && ((Environment.TickCount - tickCount) >= timeout))
                {
                    return false;
                }
            }
            return true;
        }

        public void WaitUntilEmpty()
        {
            this.WaitUntilEmpty(0);
        }

        public bool WaitUntilEmpty(int timeout)
        {
            int tickCount = Environment.TickCount;
            while (this.Count > 0)
            {
                Thread.Sleep(100);
                if ((timeout > 0) && ((Environment.TickCount - tickCount) > timeout))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool WaitWhile(ref bool condition, bool negative, int timeout)
        {
            int tickCount = Environment.TickCount;
            while (condition.Equals(negative))
            {
                Thread.Sleep(10);
                if ((timeout > 0) && ((Environment.TickCount - tickCount) >= timeout))
                {
                    return false;
                }
            }
            return true;
        }

        public int Count
        {
            get
            {
                return this.InnerQueue.Count;
            }
        }

        public bool Disposed
        {
            get
            {
                return this._Disposed;
            }
        }

        public bool IsSuspended
        {
            get
            {
                return this.mIsSuspended;
            }
        }

        public static ThreadQueue Quazal
        {
            get
            {
                return _Quazal;
            }
        }

        public bool Running
        {
            get
            {
                return this.mRunning;
            }
        }

        public int ThreadID
        {
            get
            {
                return this.WorkerThread.ManagedThreadId;
            }
        }
    }
}

