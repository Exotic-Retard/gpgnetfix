namespace GPG.Multiplayer.Game.SupremeCommander
{
    using GPG.Logging;
    using System;
    using System.IO;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    public class Driver : IDisposable
    {
        private BufferedStream mBufferedStream;
        private object mMutex = new object();
        private BinaryReader mReader;
        private Socket mSocket;
        private NetworkStream mStream;
        private Thread mThread;

        public event EventHandler<InputEventArgs> OnInput;

        public Driver(Socket socket)
        {
            this.mSocket = socket;
            this.mStream = new NetworkStream(socket);
            this.mBufferedStream = new BufferedStream(this.mStream);
            this.mReader = new BinaryReader(this.mBufferedStream);
            this.mThread = new Thread(new ThreadStart(this.ThreadProc));
        }

        public void CloseOutput()
        {
            this.mSocket.Shutdown(SocketShutdown.Send);
        }

        public void Dispose()
        {
            if (this.mSocket != null)
            {
                this.mSocket.Shutdown(SocketShutdown.Both);
                this.mThread.Join();
                this.mThread = null;
                this.mReader.Close();
                this.mReader = null;
                this.mBufferedStream.Dispose();
                this.mBufferedStream = null;
                this.mStream.Dispose();
                this.mStream = null;
                this.mSocket.Close();
                this.mSocket = null;
            }
        }

        public void Send(string cmd, params object[] args)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8))
                {
                    writer.Write(cmd.Length);
                    writer.Write(cmd.ToCharArray());
                    writer.Write(args.Length);
                    foreach (object obj2 in args)
                    {
                        if (obj2 is int)
                        {
                            writer.Write((byte) 0);
                            writer.Write((int) obj2);
                        }
                        else if (obj2 is string)
                        {
                            string s = (string) obj2;
                            writer.Write((byte) 1);
                            byte[] bytes = Encoding.UTF8.GetBytes(s);
                            writer.Write(bytes.Length);
                            writer.Write(bytes);
                        }
                        else
                        {
                            if (!(obj2 is byte[]))
                            {
                                throw new Exception(string.Format("Can't send objects of type {0}", obj2.GetType().Name));
                            }
                            byte[] buffer = (byte[]) obj2;
                            writer.Write((byte) 2);
                            writer.Write(buffer.Length);
                            writer.Write(buffer);
                        }
                    }
                }
                try
                {
                    if (this.mSocket.Connected)
                    {
                        this.mSocket.Send(stream.ToArray());
                    }
                }
                catch (SocketException exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        public void Start()
        {
            this.mThread.Start();
        }

        private void ThreadProc()
        {
            InputEventArgs args;
        Label_0000:
            try
            {
                int count = this.mReader.ReadInt32();
                byte[] bytes = this.mReader.ReadBytes(count);
                string cmd = Encoding.UTF8.GetString(bytes);
                uint num2 = this.mReader.ReadUInt32();
                object[] objArray = new object[num2];
                for (int i = 0; i < num2; i++)
                {
                    switch (this.mReader.ReadByte())
                    {
                        case 0:
                            objArray[i] = this.mReader.ReadInt32();
                            break;

                        case 1:
                        {
                            int num4 = this.mReader.ReadInt32();
                            byte[] buffer2 = this.mReader.ReadBytes(num4);
                            objArray[i] = Encoding.UTF8.GetString(buffer2);
                            break;
                        }
                        case 2:
                        {
                            int num5 = this.mReader.ReadInt32();
                            objArray[i] = this.mReader.ReadBytes(num5);
                            break;
                        }
                        default:
                            throw new Exception("This is not a valid arguement type code.");
                    }
                }
                args = new InputEventArgs(cmd, objArray);
            }
            catch (IOException)
            {
                return;
            }
            lock (this.mMutex)
            {
                this.OnInput(this, args);
                goto Label_0000;
            }
        }

        public class InputEventArgs : EventArgs
        {
            public readonly object[] Args;
            public readonly string Command;

            public InputEventArgs(string cmd, object[] args)
            {
                this.Command = cmd;
                this.Args = args;
            }
        }
    }
}

