namespace GPG
{
    using GPG.Logging;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class GlobalHook
    {
        private static GlobalHook ControlHook = new GlobalHook();
        private static Dictionary<HookTypes, GlobalHook> DisabledHooks = new Dictionary<HookTypes, GlobalHook>();
        private IntPtr mHandle;
        private HookTypes mHookType;
        private bool mIsInstalled;
        private static GlobalMouse mMouse = null;
        private HookProc mProc;

        public event HookInvoke Proc;

        protected GlobalHook()
        {
            this.mHookType = HookTypes.NONE;
            this.mHandle = IntPtr.Zero;
            this.mProc = new HookProc(this.OnProc);
        }

        protected GlobalHook(HookTypes hookType)
        {
            this.mHookType = HookTypes.NONE;
            this.mHandle = IntPtr.Zero;
            this.mProc = new HookProc(this.OnProc);
            this.mHookType = hookType;
        }

        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(IntPtr hhook, int code, IntPtr wParam, IntPtr lParam);
        public static GlobalHook Create(HookTypes type)
        {
            GlobalHook hook = new GlobalHook {
                mHookType = type
            };
            hook.Install();
            EventLog.WriteLine("Created global hook of type: {0}", new object[] { type });
            return hook;
        }

        public static void Disable(HookTypes type)
        {
            GlobalHook hook = Create(type);
            hook.Proc = (HookInvoke) Delegate.Combine(hook.Proc, new HookInvoke(GlobalHook.IgnoreGlobalHook));
            DisabledHooks[type] = hook;
            EventLog.WriteLine("Global hooks of type: {0} have been disabled.", new object[] { type });
        }

        public static void DisableAll()
        {
        }

        public static void Enable(HookTypes type)
        {
            if (DisabledHooks.ContainsKey(type))
            {
                DisabledHooks[type].Uninstall();
                DisabledHooks.Remove(type);
                EventLog.WriteLine("Global hooks of type: {0} have been enabled.", new object[] { type });
            }
        }

        private static void IgnoreGlobalHook(GlobalHook sender, HookEventArgs e)
        {
            EventLog.WriteLine("Ignored hook: {0}, WP: {1}, LP: {2}", new object[] { e.Code, e.WParam, e.LParam });
            e.Continue = false;
        }

        public bool Install()
        {
            if (this.IsInstalled)
            {
                return false;
            }
            this.mHandle = SetWindowsHookEx(this.HookType, this.mProc, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
            if (this.Handle.ToInt32() < 1)
            {
                ErrorLog.WriteLine("Error creating global event of type: {0}, failed with code: {1}", new object[] { this.HookType, this.Handle });
                return false;
            }
            this.mIsInstalled = true;
            return true;
        }

        protected virtual int OnProc(int code, IntPtr wParam, IntPtr lParam)
        {
            HookEventArgs e = new HookEventArgs(code, wParam, lParam);
            if (this.Proc != null)
            {
                this.Proc(this, e);
            }
            if (e.Continue)
            {
                return CallNextHookEx(this.Handle, code, wParam, lParam);
            }
            return this.mProc.Method.MethodHandle.Value.ToInt32();
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(HookTypes code, HookProc func, IntPtr hInstance, int threadID);
        [DllImport("user32.dll")]
        private static extern int UnhookWindowsHookEx(IntPtr hhook);
        public void Uninstall()
        {
            UnhookWindowsHookEx(this.Handle);
            this.mIsInstalled = false;
        }

        public IntPtr Handle
        {
            get
            {
                return this.mHandle;
            }
        }

        public HookTypes HookType
        {
            get
            {
                return this.mHookType;
            }
        }

        public bool IsInstalled
        {
            get
            {
                return this.mIsInstalled;
            }
        }

        public static GlobalMouse Mouse
        {
            get
            {
                if (mMouse == null)
                {
                    mMouse = new GlobalMouse();
                }
                return mMouse;
            }
        }
    }
}

