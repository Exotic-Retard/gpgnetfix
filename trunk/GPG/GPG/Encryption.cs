namespace GPG
{
    using GPG.Logging;
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public static class Encryption
    {
        public static string Decode(string text, EncodingMethods method)
        {
            if (method != EncodingMethods.Base64)
            {
                throw new NotImplementedException("Encoding method not yet suported.");
            }
            byte[] bytes = Convert.FromBase64String(text);
            return Encoding.UTF8.GetString(bytes);
        }

        public static string Encode(string text, EncodingMethods method)
        {
            if (method != EncodingMethods.Base64)
            {
                throw new NotImplementedException("Encoding method not yet suported.");
            }
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        }

        public static string Hash(string text)
        {
            return Encoding.UTF8.GetString(HashAlgorithm.Create().ComputeHash(Encoding.UTF8.GetBytes(text)));
        }

        public static string MD5(string file)
        {
            if (file == null)
            {
                return null;
            }
            if (!File.Exists(file))
            {
                return null;
            }
            string path = file;
            System.Security.Cryptography.MD5 md = System.Security.Cryptography.MD5.Create();
            StringBuilder builder = new StringBuilder();
            try
            {
                foreach (byte num in md.ComputeHash(File.ReadAllBytes(path)))
                {
                    builder.Append(num.ToString("x2").ToLower());
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return null;
            }
            return builder.ToString();
        }
    }
}

