namespace GPGnetCommunicationsLib
{
    using System;
    using System.IO;
    using System.Net.Sockets;

    public class StateObject
    {
        public byte[] buffer = new byte[0x1000];
        public const int BufferSize = 0x1000;
        public bool ConnectionError;
        public int ConnectionID = -1;
        public Credentials credentials;
        public MemoryStream Stream = new MemoryStream();
        public int UserID = -1;
        public Socket workSocket;

        public CommandMessage GetNextMessage()
        {
            if (this.Stream.Length < 2L)
            {
                return null;
            }
            this.Stream.Position = 0L;
            BinaryReader reader = new BinaryReader(this.Stream);
            reader.ReadInt16();
            int count = 0;
            if (this.Stream.Length == 2L)
            {
                count = 2;
            }
            else
            {
                count = reader.ReadInt32();
            }
            reader = null;
            if (count > this.Stream.Length)
            {
                return null;
            }
            this.Stream.Position = 0L;
            byte[] buffer = new byte[count];
            byte[] buffer2 = new byte[this.Stream.Length - count];
            MemoryStream stream = new MemoryStream();
            this.Stream.Read(buffer, 0, count);
            if (buffer2.Length > 0)
            {
                this.Stream.Read(buffer2, 0, buffer2.Length);
                stream.Write(buffer2, 0, buffer2.Length);
            }
            this.Stream = stream;
            CommandMessage message = new CommandMessage();
            message.UnformatMessage(buffer);
            return message;
        }

        public void ResetState()
        {
            this.Stream = null;
            this.Stream = new MemoryStream();
        }
    }
}

