namespace GPG.Security
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using System.Text;

    public class Crypto
    {
        public event ProgressDelegate OnDecryptAmount;

        public event ProgressDelegate OnEncryptAmount;

        public bool DecryptFile(byte[] aPublicKey, byte[] aIV, string aFileName)
        {
            try
            {
                FileStream stream = new FileStream(aFileName, FileMode.Open);
                RijndaelManaged managed = new RijndaelManaged {
                    Padding = PaddingMode.None
                };
                CryptoStream stream2 = new CryptoStream(stream, managed.CreateDecryptor(aPublicKey, aIV), CryptoStreamMode.Read);
                FileStream stream3 = new FileStream(aFileName.Replace(".enc", ""), FileMode.Create);
                try
                {
                    int num;
                    int bytecount = 0;
                    while ((num = stream2.ReadByte()) != -1)
                    {
                        bytecount += num;
                        stream3.WriteByte((byte) num);
                        if (this.OnDecryptAmount != null)
                        {
                            this.OnDecryptAmount(bytecount);
                        }
                    }
                }
                finally
                {
                    stream3.Close();
                    stream2.Close();
                    stream.Close();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DecryptFileString(string aPublicKey, string aIV, string aFileName)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] bytes = encoding.GetBytes(aPublicKey);
            byte[] buffer2 = encoding.GetBytes(aIV);
            return this.DecryptFile(bytes, buffer2, aFileName);
        }

        public bool EncryptFile(byte[] aPublicKey, byte[] aIV, string aFileName)
        {
            try
            {
                int num;
                string path = aFileName;
                FileStream stream = new FileStream(aFileName + ".enc", FileMode.Create);
                RijndaelManaged managed = new RijndaelManaged();
                CryptoStream stream2 = new CryptoStream(stream, managed.CreateEncryptor(aPublicKey, aIV), CryptoStreamMode.Write);
                FileStream stream3 = new FileStream(path, FileMode.Open);
                int bytecount = 0;
                while ((num = stream3.ReadByte()) != -1)
                {
                    bytecount += num;
                    stream2.WriteByte((byte) num);
                    if (this.OnEncryptAmount != null)
                    {
                        this.OnEncryptAmount(bytecount);
                    }
                }
                stream3.Close();
                stream2.Close();
                stream.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EncryptFileString(string aPublicKey, string aIV, string aFileName)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] bytes = encoding.GetBytes(aPublicKey);
            byte[] buffer2 = encoding.GetBytes(aIV);
            return this.EncryptFile(bytes, buffer2, aFileName);
        }

        private string OutputFileNameEn(string aFullPath)
        {
            int index = 0;
            for (int i = 0; aFullPath.IndexOf('\\', i) != -1; i = index + 1)
            {
                index = aFullPath.IndexOf('\\', i);
            }
            return (aFullPath.Substring(index + 1).Replace('.', '_') + ".enc");
        }
    }
}

