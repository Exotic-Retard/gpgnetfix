namespace GPG.UI
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.IO;

    public class PackagedFile
    {
        private const string BEGININFO = "===FILE INFO===";
        private Hashtable cacheData = new Hashtable();
        private const string ENDINFO = "===END INFO===";
        private Hashtable imageCacheData = new Hashtable();
        private bool mCacheInMemory;
        private string mPackageFile = "";
        private ArrayList packageData = new ArrayList();

        public void ClearCache()
        {
            this.cacheData.Clear();
        }

        public void ExplodeFiles(string path)
        {
            foreach (FileData data in this.packageData)
            {
                string str = path + data.FileName;
                string str2 = str.Substring(0, str.LastIndexOf(@"\"));
                if (!Directory.Exists(str2))
                {
                    Directory.CreateDirectory(str2);
                }
                FileStream output = new FileStream(str, FileMode.Create);
                BinaryWriter writer = new BinaryWriter(output);
                try
                {
                    writer.Write(this.GetFile(data.FileName));
                    continue;
                }
                finally
                {
                    writer.Close();
                    output.Close();
                }
            }
        }

        public byte[] GetFile(string filename)
        {
            byte[] buffer = this.cacheData[filename] as byte[];
            if (buffer == null)
            {
                foreach (FileData data in this.packageData)
                {
                    if (data.FileName == filename)
                    {
                        buffer = new byte[data.FileSize];
                        FileStream input = new FileStream(this.mPackageFile, FileMode.Open);
                        BinaryReader reader = new BinaryReader(input);
                        try
                        {
                            input.Position = data.FilePosition;
                            for (int i = 0; i < data.FileSize; i++)
                            {
                                buffer[i] = reader.ReadByte();
                            }
                            return buffer;
                        }
                        finally
                        {
                            reader.Close();
                            input.Close();
                        }
                    }
                }
            }
            return buffer;
        }

        public Image GetFileAsImage(string filename)
        {
            Image image = this.imageCacheData[filename] as Image;
            if (image != null)
            {
                return image;
            }
            byte[] file = this.GetFile(filename);
            if (file == null)
            {
                return new Bitmap(20, 20);
            }
            MemoryStream stream = new MemoryStream(file);
            stream.Position = 0L;
            Image image2 = null;
            image2 = Image.FromStream(stream);
            if (this.mCacheInMemory)
            {
                this.imageCacheData.Add(filename, image2);
            }
            return image2;
        }

        public void LoadPackage(string filename, bool cacheinmemory)
        {
            this.packageData.Clear();
            this.cacheData.Clear();
            this.imageCacheData.Clear();
            this.mPackageFile = filename;
            this.mCacheInMemory = cacheinmemory;
            int num = 0;
            StreamReader reader = new StreamReader(filename);
            try
            {
                if (reader.ReadLine().IndexOf("===FILE INFO===") < 0)
                {
                    throw new FileLoadException("This file is not a valid package.", filename);
                }
                string str = reader.ReadLine();
                while (str.IndexOf("===END INFO===") < 0)
                {
                    try
                    {
                        FileData data = new FileData();
                        data.FileSize = Convert.ToInt32(str.Split(new char[] { '|' })[0]);
                        data.FileName = str.Split(new char[] { '|' })[1];
                        data.FilePosition = num;
                        num += data.FileSize;
                        this.packageData.Add(data);
                        str = reader.ReadLine();
                        continue;
                    }
                    catch (Exception exception)
                    {
                        throw new FileLoadException("This file is not a valid package.", filename, exception);
                    }
                }
                int num2 = ((int) reader.BaseStream.Length) - num;
                foreach (FileData data2 in this.packageData)
                {
                    data2.FilePosition += num2;
                }
            }
            finally
            {
                reader.Close();
            }
            if (cacheinmemory)
            {
                FileStream stream = new FileStream(this.mPackageFile, FileMode.Open);
                try
                {
                    foreach (FileData data3 in this.packageData)
                    {
                        byte[] buffer = new byte[data3.FileSize];
                        stream.Position = data3.FilePosition;
                        stream.Read(buffer, 0, data3.FileSize);
                        this.cacheData.Add(data3.FileName, buffer);
                    }
                }
                finally
                {
                    stream.Close();
                }
            }
        }

        public void PackageFiles(string path, string searchPattern, string outfile)
        {
            this.PackageFiles(path, searchPattern, outfile, false);
        }

        public void PackageFiles(string path, string searchPattern, string outfile, bool getSubDirs)
        {
            ArrayList filelist = new ArrayList();
            string str = "===FILE INFO===";
            string str2 = this.SearchDir(path, path, searchPattern, ref filelist, getSubDirs);
            if (str2 != "")
            {
                str = str + str2 + "\r\n===END INFO===\r\n";
                FileStream output = new FileStream(outfile, FileMode.Create);
                BinaryWriter writer = new BinaryWriter(output);
                try
                {
                    writer.Write(str);
                    foreach (string str3 in filelist)
                    {
                        FileStream input = new FileStream(str3, FileMode.Open);
                        BinaryReader reader = new BinaryReader(input);
                        try
                        {
                            writer.Write(reader.ReadBytes((int) input.Length));
                            continue;
                        }
                        finally
                        {
                            reader.Close();
                            input.Close();
                        }
                    }
                }
                finally
                {
                    writer.Close();
                    output.Close();
                }
            }
        }

        private string SearchDir(string dir, string homepath, string searchpattern, ref ArrayList filelist, bool getSubDirs)
        {
            string str = "";
            foreach (string str2 in Directory.GetFiles(dir, searchpattern))
            {
                if ((str2.IndexOf(".cs") < 0) && (str2.IndexOf(".gpgnet") < 0))
                {
                    FileInfo info = new FileInfo(str2);
                    object obj2 = str;
                    str = string.Concat(new object[] { obj2, "\r\n", info.Length, "|", str2.Replace(homepath, "") });
                    filelist.Add(str2);
                }
            }
            if (getSubDirs)
            {
                foreach (string str3 in Directory.GetDirectories(dir))
                {
                    str = str + this.SearchDir(str3, homepath, searchpattern, ref filelist, getSubDirs);
                }
            }
            return str;
        }
    }
}

