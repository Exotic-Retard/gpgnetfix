namespace GPG
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    public class BabelTranslate
    {
        private const string BABELFISHREFERER = "http://babelfish.altavista.com/";
        private const string BABELFISHURL = "http://babelfish.altavista.com/babelfish/tr";
        private const string ERRORSTRINGEND = "</font>";
        private const string ERRORSTRINGSTART = "<font color=red>";
        private readonly string[] VALIDTRANSLATIONMODES = new string[] { 
            "en_zh", "en_fr", "en_de", "en_it", "en_ja", "en_ko", "en_pt", "en_es", "zh_en", "fr_en", "fr_de", "de_en", "de_fr", "it_en", "ja_en", "ko_en", 
            "pt_en", "ru_en", "es_en"
         };

        public string BabelFish(string translationmode, string sourcedata)
        {
            try
            {
                if ((translationmode == null) || (translationmode.Length == 0))
                {
                    throw new ArgumentNullException("translationmode");
                }
                if ((sourcedata == null) || (translationmode.Length == 0))
                {
                    throw new ArgumentNullException("sourcedata");
                }
                translationmode = translationmode.Trim();
                sourcedata = sourcedata.Trim();
                bool flag = false;
                for (int i = 0; i < this.VALIDTRANSLATIONMODES.Length; i++)
                {
                    if (this.VALIDTRANSLATIONMODES[i] == translationmode)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    return "<font color=red>The translationmode specified was not a valid translation translationmode</font>";
                }
                Uri requestUri = new Uri("http://babelfish.altavista.com/babelfish/tr");
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUri);
                request.Referer = "http://babelfish.altavista.com/";
                string s = "lp=" + translationmode + "&tt=urltext&intl=1&doit=done&urltext=" + HttpUtility.UrlEncode(sourcedata);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = s.Length;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";
                Stream requestStream = request.GetRequestStream();
                byte[] bytes = new UTF8Encoding().GetBytes(s);
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                string input = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                MatchCollection matchs = new Regex(@"<div style=padding:10px;>((?:.|\n)*?)</div>", RegexOptions.IgnoreCase).Matches(input);
                if ((matchs.Count != 1) || (matchs[0].Groups.Count != 2))
                {
                    return "<font color=red>The HTML returned from Babelfish appears to have changed. Please check for an updated regular expression</font>";
                }
                return matchs[0].Groups[1].Value;
            }
            catch (Exception)
            {
                return sourcedata;
            }
        }
    }
}

