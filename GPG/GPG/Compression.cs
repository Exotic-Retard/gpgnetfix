namespace GPG
{
    using java.io;
    using java.util;
    using java.util.zip;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Runtime.CompilerServices;

    public class Compression
    {
        public static  event StringEventHandler OnDecompress;

        private static void AddEntries(java.util.zip.ZipFile file, string rootDirectory, string[] newFiles, bool flattenHierarchy)
        {
            string destFileName = file.getName();
            string tempFileName = Path.GetTempFileName();
            ZipOutputStream destination = new ZipOutputStream(new FileOutputStream(tempFileName));
            try
            {
                CopyEntries(file, destination);
                if (newFiles != null)
                {
                    foreach (string str3 in newFiles)
                    {
                        string directoryName;
                        if (flattenHierarchy)
                        {
                            directoryName = Path.GetDirectoryName(str3);
                        }
                        else if (rootDirectory == null)
                        {
                            directoryName = Path.GetPathRoot(str3);
                        }
                        else
                        {
                            directoryName = rootDirectory;
                        }
                        directoryName = directoryName + @"\";
                        ZipEntry ze = new ZipEntry(str3.Remove(0, directoryName.Length));
                        ze.setMethod(8);
                        destination.putNextEntry(ze);
                        try
                        {
                            FileInputStream source = new FileInputStream(str3);
                            try
                            {
                                CopyStream(source, destination);
                            }
                            finally
                            {
                                source.close();
                            }
                        }
                        finally
                        {
                            destination.closeEntry();
                        }
                    }
                }
            }
            finally
            {
                destination.close();
            }
            file.close();
            System.IO.File.Copy(tempFileName, destFileName, true);
            System.IO.File.Delete(tempFileName);
        }

        public static void AddToZip(string zipFile, string[] newFiles, bool flattenHierarchy)
        {
            java.util.zip.ZipFile file = new java.util.zip.ZipFile(zipFile);
            AddEntries(file, "", newFiles, flattenHierarchy);
            file.close();
        }

        public static void AddToZip(string zipFile, string newFile, bool flattenHierarchy)
        {
            AddToZip(zipFile, new string[] { newFile }, flattenHierarchy);
        }

        public static byte[] CompressData(byte[] data)
        {
            MemoryStream stream = new MemoryStream();
            GZipStream stream2 = new GZipStream(stream, CompressionMode.Compress, true);
            stream2.Write(data, 0, data.Length);
            stream2.Close();
            stream.Position = 0L;
            byte[] buffer = new byte[(int) stream.Length];
            stream.Read(buffer, 0, (int) stream.Length);
            stream.Close();
            return buffer;
        }

        public static byte[] CompressFile(string sourcePath)
        {
            FileStream stream = new FileStream(sourcePath, FileMode.Open);
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int) stream.Length);
            stream.Close();
            return CompressData(buffer);
        }

        private static void CopyEntries(java.util.zip.ZipFile source, ZipOutputStream destination)
        {
            foreach (ZipEntry entry in GetZippedItems(source))
            {
                destination.putNextEntry(entry);
                InputStream stream = source.getInputStream(entry);
                CopyStream(stream, destination);
                destination.closeEntry();
                stream.close();
            }
        }

        private static void CopyEntries(java.util.zip.ZipFile source, ZipOutputStream destination, string[] entryNames)
        {
            List<ZipEntry> zippedItems = GetZippedItems(source);
            for (int i = 0; i < entryNames.Length; i++)
            {
                foreach (ZipEntry entry in zippedItems)
                {
                    if (entry.getName() == entryNames[i])
                    {
                        destination.putNextEntry(entry);
                        InputStream stream = source.getInputStream(entry);
                        CopyStream(stream, destination);
                        destination.closeEntry();
                        stream.close();
                    }
                }
            }
        }

        private static void CopyStream(InputStream source, OutputStream destination)
        {
            sbyte[] b = new sbyte[0x1f40];
        Label_000B:
            try
            {
                int count = source.read(b, 0, b.Length);
                if (count > 0)
                {
                    destination.write(b, 0, count);
                    goto Label_000B;
                }
            }
            catch (Exception exception)
            {
                string message = exception.Message;
                goto Label_000B;
            }
        }

        public static byte[] DecompressData(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            MemoryStream stream2 = new MemoryStream();
            GZipStream stream3 = new GZipStream(stream, CompressionMode.Decompress);
            byte[] buffer = new byte[0x100];
            int count = stream3.Read(buffer, 0, 0x100);
            int tickCount = 0;
            while (count > 0)
            {
                stream2.Write(buffer, 0, count);
                count = stream3.Read(buffer, 0, 0x100);
                if ((Environment.TickCount - tickCount) > 0x3e8)
                {
                    if (OnDecompress != null)
                    {
                        OnDecompress(stream2.Length.ToString());
                    }
                    tickCount = Environment.TickCount;
                }
            }
            stream3.Close();
            stream.Close();
            byte[] buffer2 = new byte[stream2.Length];
            stream2.Position = 0L;
            stream2.Read(buffer2, 0, (int) stream2.Length);
            stream2.Close();
            return buffer2;
        }

        public static byte[] DecompressFile(string sourcePath)
        {
            FileStream stream = new FileStream(sourcePath, FileMode.Open);
            MemoryStream stream2 = new MemoryStream();
            GZipStream stream3 = new GZipStream(stream, CompressionMode.Decompress);
            byte[] buffer = new byte[0x100];
            int count = stream3.Read(buffer, 0, 0x100);
            int tickCount = 0;
            while (count > 0)
            {
                stream2.Write(buffer, 0, count);
                count = stream3.Read(buffer, 0, 0x100);
                if ((Environment.TickCount - tickCount) > 0x3e8)
                {
                    if (OnDecompress != null)
                    {
                        OnDecompress(stream2.Length.ToString());
                    }
                    tickCount = Environment.TickCount;
                }
            }
            stream3.Close();
            stream.Close();
            byte[] buffer2 = new byte[stream2.Length];
            stream2.Position = 0L;
            stream2.Read(buffer2, 0, (int) stream2.Length);
            stream2.Close();
            return buffer2;
        }

        private static List<ZipEntry> GetZippedItems(java.util.zip.ZipFile file)
        {
            List<ZipEntry> list = new List<ZipEntry>();
            Enumeration enumeration = file.entries();
            while (enumeration.hasMoreElements())
            {
                ZipEntry item = (ZipEntry) enumeration.nextElement();
                list.Add(item);
            }
            return list;
        }

        private static void RemoveEntries(java.util.zip.ZipFile file, string[] items)
        {
            string destFileName = file.getName();
            string tempFileName = Path.GetTempFileName();
            ZipOutputStream destination = new ZipOutputStream(new FileOutputStream(tempFileName));
            try
            {
                List<ZipEntry> zippedItems = GetZippedItems(file);
                List<string> list2 = new List<string>();
                foreach (ZipEntry entry in zippedItems)
                {
                    bool flag = false;
                    foreach (string str3 in items)
                    {
                        if (str3 != entry.getName())
                        {
                            flag = true;
                        }
                    }
                    if (flag)
                    {
                        list2.Add(entry.getName());
                    }
                }
                CopyEntries(file, destination, list2.ToArray());
            }
            finally
            {
                destination.close();
            }
            file.close();
            System.IO.File.Copy(tempFileName, destFileName, true);
            System.IO.File.Delete(tempFileName);
        }

        public static void RemoveFromZipFile(string zipFile, string[] items)
        {
            java.util.zip.ZipFile file = new java.util.zip.ZipFile(zipFile);
            RemoveEntries(file, items);
            file.close();
        }

        public static void RemoveFromZipFile(string zipFile, string item)
        {
            RemoveFromZipFile(zipFile, new string[] { item });
        }

        public static void Unzip(string filename)
        {
            Unzip(filename, Path.GetDirectoryName(filename));
        }

        public static void Unzip(string zipFile, string output)
        {
            Unzip(zipFile, new string[0], output);
        }

        public static void Unzip(string zipFile, string[] targetFiles, string output)
        {
            java.util.zip.ZipFile file = new java.util.zip.ZipFile(zipFile);
            try
            {
                using (List<ZipEntry>.Enumerator enumerator = GetZippedItems(file).GetEnumerator())
                {
                    Predicate<string> match = null;
                    ZipEntry entry;
                    while (enumerator.MoveNext())
                    {
                        entry = enumerator.Current;
                        if ((targetFiles != null) && (targetFiles.Length > 0))
                        {
                            if (match == null)
                            {
                                match = delegate (string s) {
                                    return entry.getName().ToLower().EndsWith(s.ToLower().Replace(@"\", "/").TrimStart("/".ToCharArray()));
                                };
                            }
                            if (Array.FindIndex<string>(targetFiles, match) < 0)
                            {
                                continue;
                            }
                        }
                        if (!entry.isDirectory())
                        {
                            InputStream source = file.getInputStream(entry);
                            try
                            {
                                string fileName;
                                string directoryName;
                                if (Path.HasExtension(output) && !Directory.Exists(output))
                                {
                                    fileName = Path.GetFileName(output);
                                    directoryName = Path.GetDirectoryName(output);
                                }
                                else
                                {
                                    fileName = Path.GetFileName(entry.getName());
                                    directoryName = Path.GetDirectoryName(entry.getName());
                                    directoryName = output + @"\" + directoryName;
                                }
                                Directory.CreateDirectory(directoryName);
                                FileOutputStream destination = new FileOutputStream(Path.Combine(directoryName, fileName));
                                try
                                {
                                    CopyStream(source, destination);
                                }
                                finally
                                {
                                    destination.close();
                                }
                                continue;
                            }
                            finally
                            {
                                source.close();
                            }
                        }
                    }
                }
            }
            finally
            {
                if (file != null)
                {
                    file.close();
                }
            }
        }

        public static void Unzip(string zipFile, string targetFile, string output)
        {
            Unzip(zipFile, new string[] { targetFile }, output);
        }

        public static bool ZipContainsFile(string zipFile, string searchFile)
        {
            bool flag;
            java.util.zip.ZipFile file = new java.util.zip.ZipFile(zipFile);
            try
            {
                foreach (ZipEntry entry in GetZippedItems(file))
                {
                    if (!entry.isDirectory() && entry.getName().EndsWith(searchFile))
                    {
                        return true;
                    }
                }
                flag = false;
            }
            finally
            {
                if (file != null)
                {
                    file.close();
                }
            }
            return flag;
        }

        public static void ZipDirectory(string outputName, string directory)
        {
            ZipDirectory(outputName, directory, false, false);
        }

        public static void ZipDirectory(string outputName, string directory, bool recurse)
        {
            ZipDirectory(outputName, directory, false, recurse);
        }

        public static void ZipDirectory(string outputName, string directory, bool keepDirectoryRoot, bool recurse)
        {
            SearchOption topDirectoryOnly = SearchOption.TopDirectoryOnly;
            if (recurse)
            {
                topDirectoryOnly = SearchOption.AllDirectories;
            }
            if (keepDirectoryRoot)
            {
                directory = new DirectoryInfo(directory).Parent.FullName;
            }
            ZipFiles(outputName, directory, Directory.GetFiles(directory, "*.*", topDirectoryOnly));
        }

        public static void ZipFile(string outputName, string file)
        {
            ZipFiles(outputName, new string[] { file });
        }

        public static void ZipFiles(string outputName, string[] files)
        {
            ZipFiles(outputName, null, files);
        }

        public static void ZipFiles(string outputName, string rootDirectory, string[] files)
        {
            ZipFiles(outputName, rootDirectory, files, false);
        }

        public static void ZipFiles(string outputName, string rootDirectory, string[] files, bool flattenHierarchy)
        {
            FileOutputStream @out = new FileOutputStream(outputName);
            new ZipOutputStream(@out).close();
            java.util.zip.ZipFile file = new java.util.zip.ZipFile(outputName);
            AddEntries(file, rootDirectory, files, flattenHierarchy);
        }
    }
}

