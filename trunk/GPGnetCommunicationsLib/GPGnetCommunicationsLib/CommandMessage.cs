namespace GPGnetCommunicationsLib
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;

    [Serializable]
    public class CommandMessage : ICommandMessage
    {
        private Commands mCommandName;
        private byte[] mData;

        public byte[] FormatMessage()
        {
            MemoryStream output = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(output);
            short num = Convert.ToInt16(this.CommandName);
            writer.Write(num);
            if (this.Data != null)
            {
                writer.Write((int) (this.Data.Length + 6));
                writer.Write(this.Data);
            }
            output.Position = 0L;
            byte[] buffer = new byte[output.Length];
            output.Read(buffer, 0, (int) output.Length);
            return buffer;
        }

        public object[] GetParams()
        {
            if (this.Data == null)
            {
                return null;
            }
            List<object> list = new List<object>();
            MemoryStream input = new MemoryStream(this.Data);
            BinaryReader reader = new BinaryReader(input);
            while (reader.PeekChar() >= 0)
            {
                switch (((ParamTypes) reader.ReadByte()))
                {
                    case ParamTypes.Null:
                    {
                        list.Add(null);
                        continue;
                    }
                    case ParamTypes.String:
                    {
                        short count = reader.ReadInt16();
                        string item = Encoding.UTF8.GetString(reader.ReadBytes(count));
                        list.Add(item);
                        continue;
                    }
                    case ParamTypes.Double:
                    {
                        list.Add(reader.ReadDouble());
                        continue;
                    }
                    case ParamTypes.Float:
                    {
                        list.Add(reader.ReadSingle());
                        continue;
                    }
                    case ParamTypes.Int32:
                    {
                        list.Add(reader.ReadInt32());
                        continue;
                    }
                    case ParamTypes.Bool:
                    {
                        list.Add(reader.ReadBoolean());
                        continue;
                    }
                    case ParamTypes.Object:
                    {
                        short num2 = reader.ReadInt16();
                        Encoding.UTF8.GetString(reader.ReadBytes(num2));
                        int num3 = reader.ReadInt32();
                        MemoryStream serializationStream = new MemoryStream(reader.ReadBytes(num3));
                        object obj2 = new BinaryFormatter().Deserialize(serializationStream);
                        list.Add(obj2);
                        continue;
                    }
                }
                string str2 = "";
                while (reader.PeekChar() >= 0)
                {
                    str2 = str2 + reader.ReadChar();
                }
                throw new Exception("Unrecognized param in CommandMessage: " + str2);
            }
            return list.ToArray();
        }

        public void SetParams(params object[] data)
        {
            MemoryStream stream = new MemoryStream();
            foreach (object obj2 in data)
            {
                if (obj2 == null)
                {
                    stream.WriteByte(5);
                }
                else if (obj2 is string)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(obj2.ToString());
                    short num = Convert.ToInt16(bytes.Length);
                    stream.WriteByte(0);
                    stream.Write(BitConverter.GetBytes(num), 0, 2);
                    stream.Write(bytes, 0, bytes.Length);
                }
                else if (obj2 is int)
                {
                    stream.WriteByte(1);
                    stream.Write(BitConverter.GetBytes((int) obj2), 0, 4);
                }
                else if (obj2 is double)
                {
                    stream.WriteByte(2);
                    stream.Write(BitConverter.GetBytes((double) obj2), 0, 8);
                }
                else if (obj2 is float)
                {
                    stream.WriteByte(3);
                    stream.Write(BitConverter.GetBytes((float) obj2), 0, 4);
                }
                else if (obj2 is bool)
                {
                    stream.WriteByte(6);
                    stream.Write(BitConverter.GetBytes((bool) obj2), 0, 1);
                }
                else
                {
                    try
                    {
                        stream.WriteByte(4);
                        string s = obj2.GetType().ToString();
                        byte[] buffer = Encoding.UTF8.GetBytes(s);
                        short num2 = Convert.ToInt16(buffer.Length);
                        stream.Write(BitConverter.GetBytes(num2), 0, 2);
                        stream.Write(buffer, 0, buffer.Length);
                        MemoryStream serializationStream = new MemoryStream();
                        new BinaryFormatter().Serialize(serializationStream, obj2);
                        int count = Convert.ToInt32(serializationStream.Length);
                        byte[] buffer3 = new byte[serializationStream.Length];
                        serializationStream.Position = 0L;
                        serializationStream.Read(buffer3, 0, count);
                        stream.Write(BitConverter.GetBytes(count), 0, 4);
                        stream.Write(buffer3, 0, count);
                    }
                    catch (Exception exception)
                    {
                        throw new Exception("This type is not supported for command messages: " + obj2.GetType(), exception);
                    }
                }
            }
            stream.Position = 0L;
            this.Data = new byte[stream.Length];
            stream.Read(this.Data, 0, (int) stream.Length);
        }

        public void UnformatMessage(byte[] data)
        {
            this.CommandName = (Commands) BitConverter.ToInt16(data, 0);
            if (data.Length > 6)
            {
                this.Data = new byte[data.Length - 6];
                Array.Copy(data, 6, this.Data, 0, this.Data.Length);
            }
        }

        public Commands CommandName
        {
            get
            {
                return this.mCommandName;
            }
            set
            {
                this.mCommandName = value;
            }
        }

        public byte[] Data
        {
            get
            {
                return this.mData;
            }
            set
            {
                this.mData = value;
            }
        }
    }
}

