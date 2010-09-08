namespace GPG.Network
{
    using System;
    using System.Collections.Generic;

    public static class NetUtil
    {
        public static string[] GetChunks(string text, int chunkSize)
        {
            List<string> list = new List<string>((text.Length / chunkSize) + 1);
            int startIndex = 0;
            while (startIndex < text.Length)
            {
                if (chunkSize >= (text.Length - startIndex))
                {
                    list.Add(text.Substring(startIndex, text.Length - startIndex));
                    startIndex += text.Length - startIndex;
                }
                else
                {
                    list.Add(text.Substring(startIndex, chunkSize));
                    startIndex += chunkSize;
                }
            }
            return list.ToArray();
        }
    }
}

