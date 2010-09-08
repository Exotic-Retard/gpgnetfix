namespace GPG.Multiplayer.Quazal
{
    using GPG.Logging;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class MatchmakingHelper
    {
        private string mContainerName = "default";
        private NatMessageDelegatePtr mNatMessageDelegatePtr;
        public static int NatHeader = 8;

        public event NatMessageDelegate NatMessageHandler;

        public MatchmakingHelper(string containerName)
        {
            this.mContainerName = containerName;
        }

        [DllImport("MultiplayerBackend.dll", EntryPoint="AddPrivateURL", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool _AddPrivateURL([MarshalAs(UnmanagedType.LPStr)] string address, int port, [MarshalAs(UnmanagedType.LPStr)] string containerName);
        [DllImport("MultiplayerBackend.dll", EntryPoint="GetNatMessage", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr _GetNatMessage([MarshalAs(UnmanagedType.LPStr)] string containerName);
        [DllImport("MultiplayerBackend.dll", EntryPoint="LaunchSession", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool _LaunchSession([MarshalAs(UnmanagedType.LPStr)] string stationURL, [MarshalAs(UnmanagedType.LPStr)] string containerName);
        [DllImport("MultiplayerBackend.dll", EntryPoint="NatMessageCallback", CallingConvention=CallingConvention.Cdecl)]
        private static extern void _NatMessageCallback([MarshalAs(UnmanagedType.FunctionPtr)] NatMessageDelegatePtr natMessageEvent, [MarshalAs(UnmanagedType.LPStr)] string containerName);
        [DllImport("MultiplayerBackend.dll", EntryPoint="ProbeStations", CallingConvention=CallingConvention.Cdecl)]
        private static extern int _ProbeStations([MarshalAs(UnmanagedType.LPStr)] string containerName);
        [DllImport("MultiplayerBackend.dll", EntryPoint="ReceiveMessage", CallingConvention=CallingConvention.Cdecl)]
        private static extern void _ReceiveMessage([MarshalAs(UnmanagedType.LPStr)] string address, IntPtr buffer, int size, [MarshalAs(UnmanagedType.LPStr)] string containerName);
        [DllImport("MultiplayerBackend.dll", EntryPoint="RegisterAdapter", CallingConvention=CallingConvention.Cdecl)]
        private static extern void _RegisterAdapter(int header, [MarshalAs(UnmanagedType.LPStr)] string containerName);
        [DllImport("MultiplayerBackend.dll", EntryPoint="RegisterUDPConnection", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool _RegisterUDPConnection([MarshalAs(UnmanagedType.LPStr)] string containerName);
        public bool AddPrivateURL(string address, int port)
        {
            bool flag;
            Lobby.Lock();
            try
            {
                Lobby.IncDBCallCount(new object[0]);
                if (Lobby.sProtocol != null)
                {
                    return Lobby.sProtocol.AddPrivateURL(address, port, this.mContainerName);
                }
                flag = _AddPrivateURL(address, port, this.mContainerName);
            }
            finally
            {
                Lobby.Unlock();
            }
            return flag;
        }

        public SNatMessage GetNatMessage()
        {
            SNatMessage message;
            Lobby.Lock();
            try
            {
                if (Lobby.sProtocol != null)
                {
                    return Lobby.sProtocol.GetNatMessage();
                }
                IntPtr natPtr = _GetNatMessage(this.mContainerName);
                message = this.NatMessageFromPtr(natPtr);
            }
            finally
            {
                Lobby.Unlock();
            }
            return message;
        }

        public bool LaunchSession(string stationURL)
        {
            bool flag;
            Lobby.Lock();
            try
            {
                Lobby.IncDBCallCount(new object[0]);
                if (Lobby.sProtocol != null)
                {
                    return Lobby.sProtocol.LaunchSession(stationURL);
                }
                flag = _LaunchSession(stationURL, this.mContainerName);
            }
            finally
            {
                Lobby.Unlock();
            }
            return flag;
        }

        public void MatchmakingHelper_NatMessagePtrHandler(IntPtr NatMessage)
        {
            EventLog.WriteLine("Begin NAT Message", new object[0]);
            SNatMessage natMessage = this.NatMessageFromPtr(NatMessage);
            if (this.NatMessageHandler != null)
            {
                this.NatMessageHandler(natMessage);
            }
            EventLog.WriteLine("End NAT Message", new object[0]);
        }

        public SNatMessage NatMessageFromPtr(IntPtr natPtr)
        {
            SNatMessage message;
            SNatMessagePtr ptr = (SNatMessagePtr) Marshal.PtrToStructure(natPtr, typeof(SNatMessagePtr));
            message = new SNatMessage {
                Address = Marshal.PtrToStringAnsi(ptr.Address),
                Port = ptr.Port,
                MessageSize = ptr.MessageSize,
                Message = new byte[message.MessageSize]
            };
            Marshal.Copy(ptr.Message, message.Message, 0, message.MessageSize);
            return message;
        }

        public int ProbeStations()
        {
            int num2;
            Lobby.Lock();
            try
            {
                if (Lobby.sProtocol != null)
                {
                    return Lobby.sProtocol.ProbeStations();
                }
                EventLog.WriteLine("Begin Station Probe", new object[0]);
                int num = _ProbeStations(this.mContainerName);
                EventLog.WriteLine("End Station Probe", new object[0]);
                num2 = num;
            }
            finally
            {
                Lobby.Unlock();
            }
            return num2;
        }

        public void ReceiveMessage(string address, byte[] message, int size)
        {
            if (Lobby.sProtocol != null)
            {
                Lobby.sProtocol.ReceiveNatMessage(address, message, size);
            }
            else
            {
                EventLog.WriteLine("Begin NAT Receive Message", new object[0]);
                IntPtr destination = Marshal.AllocCoTaskMem(size);
                Marshal.Copy(message, 0, destination, size);
                _ReceiveMessage(address, destination, size, this.mContainerName);
                EventLog.WriteLine("End NAT Message Received", new object[0]);
            }
        }

        public void RegisterAdapter(int header)
        {
            Lobby.Lock();
            try
            {
                if (Lobby.sProtocol != null)
                {
                    Lobby.sProtocol.RegisterNatResponse(new NatMessageDelegatePtr(this.MatchmakingHelper_NatMessagePtrHandler));
                    Lobby.sProtocol.RegisterAdapter(header);
                }
                else
                {
                    _RegisterAdapter(header, this.mContainerName);
                    this.mNatMessageDelegatePtr = new NatMessageDelegatePtr(this.MatchmakingHelper_NatMessagePtrHandler);
                    _NatMessageCallback(this.mNatMessageDelegatePtr, this.mContainerName);
                }
            }
            finally
            {
                Lobby.Unlock();
            }
        }

        public bool RegisterUDPConnection()
        {
            bool flag;
            Lobby.Lock();
            try
            {
                if (Lobby.sProtocol != null)
                {
                    return Lobby.sProtocol.RegisterUDPConnection();
                }
                flag = _RegisterUDPConnection(this.mContainerName);
            }
            finally
            {
                Lobby.Unlock();
            }
            return flag;
        }

        public delegate void NatMessageDelegatePtr(IntPtr NatMessage);
    }
}

