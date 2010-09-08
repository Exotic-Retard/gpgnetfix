namespace GPG
{
    using GPG.Logging;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Windows.Forms;

    [Serializable]
    public class ProgramSettings
    {
        private string[] Fields = new string[0];
        private string mFilePath = SAVE_DEFAULT;
        private System.Version mVersion;
        public static readonly string SAVE_DEFAULT = (AppDomain.CurrentDomain.BaseDirectory + "usr.gpg");

        public ProgramSettings()
        {
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                this.mVersion = entryAssembly.GetName().Version;
            }
        }

        private static void _Merge(ProgramSettings source, object sourceRoot, object destRoot)
        {
            try
            {
                foreach (PropertyInfo info in sourceRoot.GetType().GetProperties())
                {
                    try
                    {
                        if (info.GetCustomAttributes(typeof(OptionsRootAttribute), false).Length > 0)
                        {
                            _Merge(source, info.GetValue(sourceRoot, null), info.GetValue(destRoot, null));
                        }
                        else if (((source.Fields == null) || (Array.IndexOf<string>(source.Fields, info.Name) >= 0)) && (info.CanWrite && (info.GetSetMethod() != null)))
                        {
                            object obj2 = info.GetValue(sourceRoot, null);
                            object obj3 = info.GetValue(destRoot, null);
                            if (((obj2 != null) && !obj2.Equals(obj3)) && (obj2 != obj3))
                            {
                                destRoot.GetType().GetProperty(info.Name).SetValue(destRoot, obj2, null);
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        ErrorLog.WriteLine(exception);
                    }
                }
            }
            catch (Exception exception2)
            {
                ErrorLog.WriteLine(exception2);
            }
        }

        private void GetVersionFields(List<string> list, object root)
        {
            try
            {
                foreach (PropertyInfo info in root.GetType().GetProperties())
                {
                    if (info.GetCustomAttributes(typeof(OptionsRootAttribute), false).Length > 0)
                    {
                        this.GetVersionFields(list, info.GetValue(root, null));
                    }
                    else
                    {
                        list.Add(info.Name);
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public static ProgramSettings Load()
        {
            return LoadFrom(SAVE_DEFAULT);
        }

        public static ProgramSettings LoadFrom(string path)
        {
            if ((path != null) && File.Exists(path))
            {
                FileStream serializationStream = null;
                try
                {
                    serializationStream = File.OpenRead(path);
                    ProgramSettings root = new BinaryFormatter().Deserialize(serializationStream) as ProgramSettings;
                    root.mFilePath = path;
                    root.PerformSaveLoad(SaveLoadDirections.Load, root);
                    return root;
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                    return null;
                }
                finally
                {
                    if (serializationStream != null)
                    {
                        serializationStream.Close();
                    }
                }
            }
            return null;
        }

        public static ProgramSettings Merge(ProgramSettings source, ProgramSettings destination)
        {
            try
            {
                if (source.GetType() != destination.GetType())
                {
                    ErrorLog.WriteLine("Types do not match between settings objects; source: {0}, destination: {1}", new object[] { source.GetType(), destination.GetType() });
                    MessageBox.Show(Loc.Get("<LOC>An error occured while saving settings, some changes may not take effect and it is recommended you restart the application."));
                }
                _Merge(source, source, destination);
                return destination;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return destination;
            }
        }

        private void PerformSaveLoad(SaveLoadDirections direction, object root)
        {
            foreach (PropertyInfo info in root.GetType().GetProperties())
            {
                if (info.GetCustomAttributes(typeof(OptionsRootAttribute), false).Length > 0)
                {
                    this.PerformSaveLoad(direction, info.GetValue(root, null));
                }
                else
                {
                    object[] customAttributes = info.GetCustomAttributes(typeof(SaveLoadFormatterAttribute), false);
                    if ((customAttributes != null) && (customAttributes.Length > 0))
                    {
                        SaveLoadFormatterAttribute attribute = customAttributes[0] as SaveLoadFormatterAttribute;
                        try
                        {
                            info.SetValue(root, attribute.Formatter.OnSaveLoad(info.GetValue(root, null), direction), null);
                        }
                        catch (Exception exception)
                        {
                            ErrorLog.WriteLine(exception);
                        }
                    }
                }
            }
        }

        public virtual void Save()
        {
            this.SaveAs(this.FilePath);
        }

        public virtual void SaveAs(string path)
        {
            this.mFilePath = path;
            FileStream serializationStream = null;
            MemoryStream stream2 = null;
            try
            {
                List<string> list = new List<string>();
                this.GetVersionFields(list, this);
                this.Fields = list.ToArray();
                stream2 = new MemoryStream();
                new BinaryFormatter().Serialize(stream2, this);
                stream2.Position = 0L;
                ProgramSettings root = new BinaryFormatter().Deserialize(stream2) as ProgramSettings;
                root.PerformSaveLoad(SaveLoadDirections.Save, root);
                serializationStream = File.Open(path, FileMode.Create);
                new BinaryFormatter().Serialize(serializationStream, root);
                root = null;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
            finally
            {
                if (serializationStream != null)
                {
                    serializationStream.Close();
                }
                if (stream2 != null)
                {
                    stream2.Close();
                }
            }
        }

        [Browsable(false)]
        public string FilePath
        {
            get
            {
                return this.mFilePath;
            }
        }

        [Browsable(false)]
        public System.Version Version
        {
            get
            {
                return this.mVersion;
            }
        }

        public interface ISaveLoadFormatter
        {
            object OnSaveLoad(object data, ProgramSettings.SaveLoadDirections direction);
        }

        public enum SaveLoadDirections
        {
            Save,
            Load
        }

        public delegate object SaveLoadEventHandler(object data, ProgramSettings.SaveLoadDirections direction);

        public class SaveLoadFormatterAttribute : Attribute
        {
            private ProgramSettings.ISaveLoadFormatter mFormatter;

            public SaveLoadFormatterAttribute(System.Type iSaveLoadType)
            {
                object obj2 = Activator.CreateInstance(iSaveLoadType);
                if (!(obj2 is ProgramSettings.ISaveLoadFormatter))
                {
                    throw new InvalidOperationException("iSaveLoadType must implement the interface ISaveLoadFormatter.");
                }
                this.mFormatter = obj2 as ProgramSettings.ISaveLoadFormatter;
            }

            internal ProgramSettings.ISaveLoadFormatter Formatter
            {
                get
                {
                    return this.mFormatter;
                }
            }
        }
    }
}

