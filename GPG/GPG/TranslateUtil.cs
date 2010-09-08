namespace GPG
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;

    public class TranslateUtil
    {
        public static Encoding Encode = Encoding.GetEncoding("UTF-8");

        private TranslateUtil()
        {
        }

        public static string GetTranslatedText(string textToTranslate, string newlang)
        {
            return GetTranslatedText(textToTranslate, newlang, "en");
        }

        public static string GetTranslatedText(string textToTranslate, string newlang, string oldlang)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create("http://translate.google.com/translate_t?text=" + textToTranslate + "&langpair=" + oldlang + "%7C" + newlang + "?test=0");
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.0; .NET CLR v2.0.50727)";
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream(), Encoding.UTF8, true);
                string str2 = reader.ReadToEnd();
                reader.Close();
                string[] strArray = str2.Split("<>".ToCharArray());
                bool flag = false;
                foreach (string str3 in strArray)
                {
                    if (flag)
                    {
                        string str4 = str3;
                        for (int i = 0; i < 0x100; i++)
                        {
                            str4 = str4.Replace("&#" + i.ToString(), ((char) i).ToString());
                        }
                        return str4;
                    }
                    if (str3 == "div id=result_box dir=ltr")
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            return textToTranslate;
        }

        public static string GetTranslatedTextxx(string textToTranslate, string newlang, string oldlang)
        {
            string str = newlang;
            if (str.ToUpper() == "ZH-CN")
            {
                str = "zh";
            }
            BabelTranslate translate = new BabelTranslate();
            return translate.BabelFish(oldlang + "_" + str, textToTranslate);
        }
    }
}

