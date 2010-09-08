namespace GPG.Multiplayer.Quazal
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class PersistentStoreHelper
    {
        private const string gpgWebUrl = "http://gpgnet.gaspowered.com";
        private int mNewVersion;
        public int PatchFileCount;

        public event FileDownloadHandler OnFileDownloaded;

        [DllImport("MultiplayerBackend.dll", EntryPoint="AddFile", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool _AddFile([MarshalAs(UnmanagedType.LPStr)] string file, int type, [MarshalAs(UnmanagedType.LPStr)] string tag);
        [DllImport("MultiplayerBackend.dll", EntryPoint="GetFile", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool _GetFile([MarshalAs(UnmanagedType.LPStr)] string file, int type, [MarshalAs(UnmanagedType.LPStr)] string tag);
        [return: MarshalAs(UnmanagedType.LPStr)]
        [DllImport("MultiplayerBackend.dll", EntryPoint="GetTags", CallingConvention=CallingConvention.Cdecl)]
        private static extern string _GetTags(int type, [MarshalAs(UnmanagedType.LPStr)] string delimiter);
        public bool AddFile(string file, int type, string tag)
        {
            bool flag;
            Lobby.Lock();
            try
            {
                Lobby.IncDBCallCount(new object[0]);
                flag = _AddFile(file, type, tag);
            }
            finally
            {
                Lobby.Unlock();
            }
            return flag;
        }

        public bool CheckForPatch()
        {
            try
            {
                WebClient client = new WebClient();
                client.QueryString.Add("pragma", "no-cache");
                StreamReader reader = new StreamReader(client.OpenRead("http://gpgnet.gaspowered.com/patch/version.txt"));
                this.mNewVersion = 0;
                try
                {
                    this.mNewVersion = Convert.ToInt32(reader.ReadToEnd());
                }
                catch
                {
                }
                reader.Close();
                return (this.mNewVersion > this.GetCurrentVersion());
            }
            catch
            {
                MessageBox.Show("Unable to check the patch server at gpgnet.gaspowered.com.  You will not receive any updates if they are available.  Press OK to continue.");
                return false;
            }
        }

        public int GetCurrentVersion()
        {
            return Convert.ToInt32(Application.ProductVersion.Split(new char[] { '.' })[2]);
        }

        public bool GetFile(string file, int type, string tag)
        {
            bool flag;
            Lobby.Lock();
            try
            {
                Lobby.IncDBCallCount(new object[0]);
                flag = _GetFile(file, type, tag);
            }
            finally
            {
                Lobby.Unlock();
            }
            return flag;
        }

        public string GetTags(int type, string delimiter)
        {
            string str;
            Lobby.Lock();
            try
            {
                Lobby.IncDBCallCount(new object[0]);
                str = _GetTags(type, delimiter);
            }
            finally
            {
                Lobby.Unlock();
            }
            return str;
        }

        public bool PatchApplication()
        {
            this.CheckForPatch();
            string path = Application.StartupPath + @"\download\patch\";
            string str2 = Application.StartupPath + @"\download\processed\" + this.mNewVersion.ToString() + @"\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!Directory.Exists(str2))
            {
                Directory.CreateDirectory(str2);
            }
            string str3 = "http://gpgnet.gaspowered.com/patch/" + this.mNewVersion.ToString() + "/";
            WebClient client = new WebClient();
            client.QueryString.Add("pragma", "no-cache");
            StreamReader reader = new StreamReader(client.OpenRead(str3 + "filemanifest.txt"));
            string[] strArray = reader.ReadToEnd().Split("\n".ToCharArray());
            if (strArray.Length == 0)
            {
                throw new Exception("A patch was detected, but an error occured reading the file manifest.");
            }
            reader.Close();
            int num = 0;
            int num2 = 0;
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strArray[i].IndexOf(" ") >= 0)
                {
                    string str4 = strArray[i].Split(" ".ToCharArray(), 2)[1].Replace("\r", "");
                    num += Convert.ToInt32(str4);
                }
            }
            int length = strArray.Length;
            for (int j = 0; j < strArray.Length; j++)
            {
                if (strArray[j].IndexOf(" ") >= 0)
                {
                    string str5 = strArray[j].Split(" ".ToCharArray(), 2)[1].Replace("\r", "");
                    string str6 = strArray[j].Split(" ".ToCharArray(), 2)[0];
                    string address = str3 + str6;
                    str6 = str6.Replace(".zip", "");
                    WebClient client2 = new WebClient();
                    client2.QueryString.Add("pragma", "no-cache");
                    BinaryReader reader2 = new BinaryReader(client2.OpenRead(address));
                    FileStream output = new FileStream(path + str6, FileMode.Create);
                    BinaryWriter writer = new BinaryWriter(output);
                    int num6 = 0;
                    int count = 0x5000;
                    for (byte[] buffer = reader2.ReadBytes(count); buffer.Length > 0; buffer = reader2.ReadBytes(count))
                    {
                        writer.Write(buffer, 0, buffer.Length);
                        if (this.OnFileDownloaded != null)
                        {
                            Application.DoEvents();
                            string[] strArray2 = new string[] { "File ", j.ToString(), " of ", length.ToString(), "\r\n", (num2 / 0x3e8).ToString(), " of ", (num / 0x3e8).ToString(), " KB\r\n", str6 };
                            string filename = string.Concat(strArray2);
                            this.OnFileDownloaded(this, filename, num6, Convert.ToInt32(str5));
                        }
                        num6 += count;
                        num2 += count;
                    }
                    writer.Close();
                    reader2.Close();
                }
            }
            if (System.IO.File.Exists(path + "gpgnetpatch.exe"))
            {
                System.IO.File.Delete(Application.StartupPath + @"\gpgnetpatch.exe");
                System.IO.File.Move(path + "gpgnetpatch.exe", Application.StartupPath + @"\gpgnetpatch.exe");
            }
            StreamWriter writer2 = new StreamWriter(path + "version.txt");
            try
            {
                writer2.WriteLine(this.mNewVersion.ToString());
            }
            finally
            {
                writer2.Close();
            }
            Process.Start(Application.StartupPath + @"\gpgnetpatch.exe");
            Application.Exit();
            return true;
        }

        public void UploadFile(string username, string password, string filename, string webdirectory)
        {
            Application.DoEvents();
            if ((username != "") && (password != ""))
            {
                string path = "ftpcommand.cmd";
                StreamWriter writer = new StreamWriter(path);
                try
                {
                    writer.WriteLine("open gpgnet.gaspowered.com");
                    writer.WriteLine(username);
                    writer.WriteLine(password);
                    writer.WriteLine("bin");
                    writer.WriteLine("hash");
                    foreach (string str2 in webdirectory.Split(new char[] { '/' }))
                    {
                        writer.WriteLine("mkdir " + str2);
                        writer.WriteLine("cd " + str2);
                    }
                    writer.WriteLine("put \"" + filename + "\"");
                    writer.WriteLine("quit");
                }
                finally
                {
                    writer.Close();
                }
                Process.Start("ftp", "-s:" + path).WaitForExit();
            }
        }

        public string FilePreface
        {
            get
            {
                if (Lobby.GetLobby().authenticationHelper.IsAdministrator)
                {
                    return "ADMIN";
                }
                return "NORMAL";
            }
        }
    }
}

