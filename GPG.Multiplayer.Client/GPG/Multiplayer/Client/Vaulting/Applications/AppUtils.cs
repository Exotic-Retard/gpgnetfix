namespace GPG.Multiplayer.Client.Vaulting.Applications
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public static class AppUtils
    {
        public static string ChkSumFile(string filename)
        {
            MD5 md = MD5.Create();
            StringBuilder builder = new StringBuilder();
            using (FileStream stream = File.Open(filename, FileMode.Open, FileAccess.Read))
            {
                foreach (byte num in md.ComputeHash(stream))
                {
                    builder.Append(num.ToString("x2").ToLower());
                }
                stream.Close();
            }
            return builder.ToString();
        }
    }
}

