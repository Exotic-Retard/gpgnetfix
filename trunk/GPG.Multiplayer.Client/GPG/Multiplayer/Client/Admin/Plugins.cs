namespace GPG.Multiplayer.Client.Admin
{
    using GPG.Multiplayer.Plugin;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;

    public static class Plugins
    {
        private static List<PluginInfo> sPlugins = new List<PluginInfo>();

        public static List<PluginInfo> GetPlugins()
        {
            return sPlugins;
        }

        public static void LoadPlugins()
        {
            if (sPlugins.Count == 0)
            {
                string path = Application.StartupPath + @"\plugins\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                foreach (string str2 in Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories))
                {
                    Assembly assembly = Assembly.LoadFile(str2);
                    foreach (System.Type type in assembly.GetTypes())
                    {
                        foreach (System.Type type2 in type.GetInterfaces())
                        {
                            if (type2 == typeof(IFormPlugin))
                            {
                                PluginInfo item = new PluginInfo();
                                item.type = type;
                                IFormPlugin plugin = item.CreatePlugin();
                                if (plugin != null)
                                {
                                    UserControl control = plugin as UserControl;
                                    if (control != null)
                                    {
                                        item.MenuCaption = plugin.MenuCaption;
                                        item.Author = plugin.Author;
                                        item.FormTitle = plugin.FormTitle;
                                        item.MenuCaption = plugin.MenuCaption;
                                        item.Version = plugin.Version;
                                        item.Location = plugin.Location;
                                        sPlugins.Add(item);
                                        control.Dispose();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

