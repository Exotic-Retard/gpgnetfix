namespace GPG
{
    using GPG.Logging;
    using Microsoft.Win32;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Windows.Forms;

    public class Loc
    {
        private static Hashtable sLocStrings = Hashtable.Synchronized(new Hashtable());

        private Loc()
        {
        }

        public static string Get(string data)
        {
            return Get(data, null);
        }

        public static string Get(string data, params object[] paramdata)
        {
            string str = InternalGet(data, paramdata);
            if (str != null)
            {
                str = str.Replace(@"\r", "\r").Replace(@"\n", "\n").Replace("GPGnet: Supreme Commander", "GPGnet");
            }
            return str;
        }

        public static string GetStringMD5(string data)
        {
            byte[] buffer = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(data));
            StringBuilder builder = new StringBuilder();
            foreach (byte num in buffer)
            {
                builder.Append(num.ToString("x2").ToLower());
            }
            return builder.ToString();
        }

        private static string InternalGet(string data, params object[] paramdata)
        {
            if (((data == null) || (data.Length < 1)) || (data.IndexOf("<LOC") < 0))
            {
                return data;
            }
            string format = "";
            string key = "";
            if ((data.IndexOf("<LOC") == 0) && (data.IndexOf(">") >= 4))
            {
                format = data.Split(">".ToCharArray(), 2)[1];
                if ((data.IndexOf("=") > 0) && (data.IndexOf("=") < data.IndexOf(">")))
                {
                    key = data.Split("=>".ToCharArray(), 3)[1];
                }
            }
            else
            {
                format = data;
            }
            if (paramdata != null)
            {
                string.Format(format, paramdata);
            }
            if (key.Trim() == "")
            {
                key = GetStringMD5(format);
                if (sLocStrings.ContainsKey("_" + key))
                {
                    key = "_" + key;
                }
            }
            if (!sLocStrings.ContainsKey(key))
            {
                return format;
            }
            string str3 = sLocStrings[key].ToString();
            if (str3.Length > 2)
            {
                return str3.Substring(1, str3.Length - 2).Replace("\\\"", "\"");
            }
            return str3.Replace("\\\"", "\"");
        }

        public static void LocObject(object obj)
        {
            LocObject(obj, new Hashtable());
        }

        private static void LocObject(object obj, Hashtable hash)
        {
        }

        public static void SetLoc()
        {
            string name = "";
            if (name == "")
            {
                string str2 = @"Software\GPG";
                try
                {
                    if (Registry.LocalMachine.OpenSubKey(str2) != null)
                    {
                        object obj2 = Registry.LocalMachine.OpenSubKey(str2, true).GetValue("install_language");
                        if (obj2 != null)
                        {
                            name = obj2.ToString();
                        }
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
                try
                {
                    if (Registry.CurrentUser.OpenSubKey(str2) != null)
                    {
                        object obj3 = Registry.CurrentUser.OpenSubKey(str2, true).GetValue("install_language");
                        if (obj3 != null)
                        {
                            name = obj3.ToString();
                        }
                    }
                }
                catch (Exception exception2)
                {
                    ErrorLog.WriteLine(exception2);
                }
                if (name == "")
                {
                    name = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
                    if (name.ToLower() == "cs")
                    {
                        name = "cz";
                    }
                    if (name.ToLower() == "zh")
                    {
                        name = "cn";
                    }
                    if (name.ToLower() == "ko")
                    {
                        name = "kr";
                    }
                }
            }
            SetLoc(name);
        }

        public static void SetLoc(string name)
        {
            sLocStrings.Clear();
            string path = Application.StartupPath + @"\loc\" + name + "_strings.txt";
            if (File.Exists(path))
            {
                StreamReader reader = new StreamReader(path);
                while (reader.Peek() >= 0)
                {
                    string[] strArray = reader.ReadLine().Split("=".ToCharArray(), 2);
                    if ((strArray.Length == 2) && (strArray[0].Trim().IndexOf("#") != 0))
                    {
                        sLocStrings.Add(strArray[0], strArray[1]);
                    }
                }
            }
            path = Application.StartupPath + @"\loc\AUTO_" + name + "_strings.txt";
            if (File.Exists(path))
            {
                StreamReader reader2 = new StreamReader(path);
                while (reader2.Peek() >= 0)
                {
                    string[] strArray2 = reader2.ReadLine().Split("=".ToCharArray(), 2);
                    if (((strArray2.Length == 2) && (strArray2[0].Trim().IndexOf("#") != 0)) && !sLocStrings.ContainsKey(strArray2[0]))
                    {
                        sLocStrings.Add(strArray2[0], "\"" + strArray2[1] + "\"");
                    }
                }
            }
        }
    }
}

