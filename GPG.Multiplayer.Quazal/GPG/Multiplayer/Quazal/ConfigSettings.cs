namespace GPG.Multiplayer.Quazal
{
    using GPG.DataAccess;
    using GPG.Logging;
    using System;

    public static class ConfigSettings
    {
        private static MappedObjectList<ConfigSetting> mSettings;

        public static bool GetBool(string key, bool defaultValue)
        {
            bool flag;
            try
            {
                object obj2 = GetObject(key, defaultValue);
                if (obj2 == null)
                {
                    flag = defaultValue;
                }
                else
                {
                    try
                    {
                        int num;
                        if (int.TryParse(obj2.ToString(), out num))
                        {
                            flag = num > 0;
                        }
                        else
                        {
                            try
                            {
                                flag = Convert.ToBoolean(obj2);
                            }
                            catch (Exception exception)
                            {
                                EventLog.WriteLine("An error occured reading key " + key + ".  Using the default value.  The error will follow in the log.", new object[0]);
                                ErrorLog.WriteLine(exception);
                                flag = false;
                            }
                        }
                    }
                    catch
                    {
                        try
                        {
                            flag = Convert.ToBoolean(obj2);
                        }
                        catch (Exception exception2)
                        {
                            EventLog.WriteLine("An error occured reading key " + key + ".  Using the default value.  The error will follow in the log.", new object[0]);
                            ErrorLog.WriteLine(exception2);
                            flag = false;
                        }
                    }
                }
            }
            catch (Exception exception3)
            {
                EventLog.WriteLine("An error occured reading key " + key + ".  Using the default value.  The error will follow in the log.", new object[0]);
                ErrorLog.WriteLine(exception3);
                flag = false;
            }
            return flag;
        }

        public static DateTime GetDateTime(string key, DateTime defaultValue)
        {
            try
            {
                return Convert.ToDateTime(GetObject(key, defaultValue));
            }
            catch (Exception exception)
            {
                EventLog.WriteLine("An error occured reading key " + key + ".  Using the default value.  The error will follow in the log.", new object[0]);
                ErrorLog.WriteLine(exception);
                return DateTime.MinValue;
            }
        }

        public static double GetDouble(string key, double defaultValue)
        {
            try
            {
                return Convert.ToDouble(GetObject(key, defaultValue));
            }
            catch (Exception exception)
            {
                EventLog.WriteLine("An error occured reading key " + key + ".  Using the default value.  The error will follow in the log.", new object[0]);
                ErrorLog.WriteLine(exception);
                return 0.0;
            }
        }

        public static int GetInt(string key, int defaultValue)
        {
            try
            {
                return Convert.ToInt32(GetObject(key, defaultValue));
            }
            catch (Exception exception)
            {
                EventLog.WriteLine("An error occured reading key " + key + ".  Using the default value.  The error will follow in the log.", new object[0]);
                ErrorLog.WriteLine(exception);
                return 0;
            }
        }

        private static object GetObject(string key, object defaultValue)
        {
            if (mSettings != null)
            {
                foreach (ConfigSetting setting in mSettings)
                {
                    if (setting.ConfigKey.ToUpper() == key.ToUpper())
                    {
                        return setting.Value;
                    }
                }
            }
            return defaultValue;
        }

        public static string GetString(string key, string defaultValue)
        {
            try
            {
                object obj2 = GetObject(key, defaultValue);
                if (obj2 != null)
                {
                    return obj2.ToString();
                }
                return null;
            }
            catch (Exception exception)
            {
                EventLog.WriteLine("An error occured reading key " + key + ".  Using the default value.  The error will follow in the log.", new object[0]);
                ErrorLog.WriteLine(exception);
                return "";
            }
        }

        public static void LoadSettings(DataList data)
        {
            mSettings = new MappedObjectList<ConfigSetting>(data);
        }
    }
}

