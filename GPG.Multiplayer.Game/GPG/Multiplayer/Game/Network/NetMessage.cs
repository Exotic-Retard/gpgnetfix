namespace GPG.Multiplayer.Game.Network
{
    using GPG.Multiplayer.Quazal;
    using System;
    using System.Net;
    using System.Text;

    public class NetMessage
    {
        public byte[] Buffer;
        public IPEndPoint EndPoint;
        public bool IsAck;
        public bool IsValid;
        public NetMessageTypes MessageType;
        private bool mFormatted;
        private static int mSerial = 0;
        private static object mSerialLock = new object();
        public int Serial;

        public NetMessage()
        {
            this.Serial = -1;
            this.Buffer = new byte[0];
            this.IsValid = true;
            this.MessageType = NetMessageTypes.Unknown;
        }

        public NetMessage(byte[] receiveBytes, IPEndPoint endpoint)
        {
            this.Serial = -1;
            this.Buffer = new byte[0];
            this.IsValid = true;
            this.MessageType = NetMessageTypes.Unknown;
            this.EndPoint = endpoint;
            if ((receiveBytes != null) && (receiveBytes.Length > 0))
            {
                this.Buffer = receiveBytes;
                if (((receiveBytes.Length > 4) && (receiveBytes[0] == 8)) && ((receiveBytes[1] == 0) && (receiveBytes[2] == 0)))
                {
                    if (receiveBytes[3] == 0)
                    {
                        this.MessageType = NetMessageTypes.Nat;
                    }
                    else if ((receiveBytes.Length > 11) && (receiveBytes[3] == 1))
                    {
                        this.MessageType = NetMessageTypes.GPGText;
                        this.ReadMessage(receiveBytes);
                    }
                    else if ((receiveBytes.Length > 11) && (receiveBytes[3] == 2))
                    {
                        this.MessageType = NetMessageTypes.GPGBinary;
                        this.ReadMessage(receiveBytes);
                    }
                    else if ((receiveBytes.Length > 11) && (receiveBytes[3] == 3))
                    {
                        this.MessageType = NetMessageTypes.GPGBinary;
                        this.ReadMessage(receiveBytes);
                    }
                }
            }
        }

        private byte[] CheckIntegrity(byte[] data)
        {
            byte[] destinationArray = new byte[3];
            Array.Copy(BitConverter.GetBytes(Convert.ToInt16(data.Length)), 0, destinationArray, 0, 2);
            int num = 0;
            foreach (byte num2 in data)
            {
                if ((num + num2) > 0x100)
                {
                    num += num2;
                }
                else
                {
                    num -= num2;
                }
            }
            destinationArray[2] = (byte) num;
            return destinationArray;
        }

        public byte[] FormatMessage()
        {
            if (this.mFormatted || (((this.MessageType != NetMessageTypes.GPGBinary) && (this.MessageType != NetMessageTypes.GPGText)) && (this.MessageType != NetMessageTypes.GPGAck)))
            {
                return this.Buffer;
            }
            byte[] destinationArray = new byte[this.Buffer.Length + 11];
            destinationArray[0] = 8;
            destinationArray[1] = 0;
            destinationArray[2] = 0;
            destinationArray[3] = (byte) this.MessageType;
            Array.Copy(this.CheckIntegrity(this.Buffer), 0, destinationArray, 4, 3);
            this.Serial = GetNextSerial();
            Array.Copy(BitConverter.GetBytes(this.Serial), 0, destinationArray, 7, 4);
            Array.Copy(this.Buffer, 0, destinationArray, 11, this.Buffer.Length);
            this.Buffer = destinationArray;
            this.mFormatted = true;
            return destinationArray;
        }

        public static int GetNextSerial()
        {
            lock (mSerialLock)
            {
                mSerial++;
                return mSerial;
            }
        }

        private void ReadMessage(byte[] receiveBytes)
        {
            this.Buffer = new byte[receiveBytes.Length - 11];
            Array.Copy(receiveBytes, 11, this.Buffer, 0, receiveBytes.Length - 11);
            byte[] buffer = this.CheckIntegrity(this.Buffer);
            this.IsValid = true;
            if (((buffer[0] != receiveBytes[4]) || (buffer[1] != receiveBytes[5])) || (buffer[2] != receiveBytes[6]))
            {
                this.IsValid = false;
            }
            this.Serial = BitConverter.ToInt32(receiveBytes, 7);
        }

        public string Text
        {
            get
            {
                if (this.MessageType == NetMessageTypes.Nat)
                {
                    return Encoding.ASCII.GetString(this.Buffer);
                }
                string str = "";
                if ((User.GetProtocol() != null) || !this.mFormatted)
                {
                    str = Encoding.ASCII.GetString(this.Buffer);
                }
                else
                {
                    str = Encoding.ASCII.GetString(this.Buffer).Substring(7);
                }
                if (str.IndexOf("/ASKREPLY") > 0)
                {
                    str = str.Substring(str.IndexOf("/ASKREPLY"));
                }
                if (str.IndexOf("/PLAYERID") > 0)
                {
                    str = str.Substring(str.IndexOf("/PLAYERID"));
                }
                return str;
            }
            set
            {
                this.Buffer = Encoding.ASCII.GetBytes(value);
            }
        }
    }
}

