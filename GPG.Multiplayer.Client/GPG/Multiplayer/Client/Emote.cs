namespace GPG.Multiplayer.Client
{
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Quazal;
    using GPG.Network;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Windows.Forms;

    [Serializable]
    public class Emote : INetData
    {
        public const string EMOTE_FILE_EXT = "gpg";
        internal const int HEIGHT = 20;
        private static Dictionary<string, Emote> mAllEmotes = new Dictionary<string, Emote>();
        [NonSerialized]
        private bool mAnimated = false;
        [NonSerialized]
        private int mAnimationFrames = 0;
        public const int MAX_SEQ_LENGTH = 8;
        [NonSerialized]
        private bool mCanAnimate = false;
        private string mCharSequence = null;
        private string mFileName = null;
        private System.Drawing.Image mImage = null;
        private byte[] mSoundData = null;
        private const string REL_PATH = "emotes";
        public const bool ServerDefault = true;
        internal const int WIDTH = 40;

        public static  event EventHandler EmotesChanged;

        static Emote()
        {
            LoadAll();
        }

        public Emote(System.Drawing.Image img, string charSequence, string fileName)
        {
            this.mImage = img;
            this.mCharSequence = charSequence;
            this.mFileName = fileName;
        }

        public static FileStream CreateEmptyEmoteFile(string fileName)
        {
            FileStream stream;
            Exception exception;
            if (File.Exists(EmoteDirectory + fileName))
            {
                stream = null;
                try
                {
                    stream = File.OpenWrite(EmoteDirectory + fileName);
                    new BinaryFormatter().Serialize(stream, new List<Emote>());
                    return stream;
                }
                catch (Exception exception1)
                {
                    exception = exception1;
                    if (stream != null)
                    {
                        stream.Close();
                        stream = null;
                    }
                    ErrorLog.WriteLine(exception);
                    return null;
                }
            }
            stream = null;
            try
            {
                if (Path.IsPathRooted(fileName))
                {
                    stream = File.Create(fileName);
                }
                else
                {
                    stream = File.Create(EmoteDirectory + fileName);
                }
                new BinaryFormatter().Serialize(stream, new List<Emote>());
                return stream;
            }
            catch (Exception exception2)
            {
                exception = exception2;
                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }
                ErrorLog.WriteLine(exception);
                return null;
            }
        }

        public void Delete()
        {
            this.Delete(false, null);
        }

        public void Delete(bool prompt, FrmMain dialogParent)
        {
            if (!(prompt && ((dialogParent == null) || (new DlgYesNo(dialogParent, "<LOC>Confirm Delete", "<LOC>Are you sure you want to delete this emote?").ShowDialog() != DialogResult.Yes))))
            {
                FileStream serializationStream = null;
                try
                {
                    string fileName = this.FileName;
                    if (File.Exists(fileName))
                    {
                        serializationStream = File.OpenRead(fileName);
                        List<Emote> graph = new BinaryFormatter().Deserialize(serializationStream) as List<Emote>;
                        serializationStream.Close();
                        if (graph != null)
                        {
                            graph.Remove(this);
                            serializationStream = File.OpenWrite(fileName);
                            new BinaryFormatter().Serialize(serializationStream, graph);
                            serializationStream.Close();
                            serializationStream = null;
                        }
                    }
                    AllEmotes.Remove(this.CharSequence);
                    OnEmotesChanged();
                }
                finally
                {
                    if (serializationStream != null)
                    {
                        serializationStream.Close();
                    }
                }
            }
        }

        public override bool Equals(object obj)
        {
            return ((obj is Emote) && ((obj as Emote).CharSequence == this.CharSequence));
        }

        public static System.Drawing.Image FormatImage(System.Drawing.Image img)
        {
            System.Drawing.Image image = img;
            if ((img.Width <= 40) && (img.Height <= 20))
            {
                return image;
            }
            int width = 40;
            int height = (int) (img.Height * (40.0 / ((double) img.Width)));
            if (height > 20)
            {
                height = 20;
                width = (int) (img.Width * (20.0 / ((double) img.Height)));
            }
            Bitmap bitmap = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.DrawImage(img, 0, 0, width, height);
            graphics.Dispose();
            return bitmap;
        }

        public override int GetHashCode()
        {
            return this.CharSequence.GetHashCode();
        }

        public static void LoadAll()
        {
            if (!Directory.Exists(EmoteDirectory))
            {
                Directory.CreateDirectory(EmoteDirectory);
            }
            foreach (string str in Directory.GetFiles(EmoteDirectory, "*.gpg", SearchOption.AllDirectories))
            {
                FileStream serializationStream = null;
                try
                {
                    serializationStream = File.OpenRead(str);
                    List<Emote> list = null;
                    try
                    {
                        list = new BinaryFormatter().Deserialize(serializationStream) as List<Emote>;
                    }
                    catch
                    {
                        serializationStream.Close();
                        serializationStream = CreateEmptyEmoteFile(Path.GetFileName(str));
                        serializationStream.Close();
                    }
                    if (list != null)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            AllEmotes[list[i].CharSequence] = list[i];
                        }
                    }
                    else
                    {
                        ErrorLog.WriteLine("Unable to deserialize file in emote directory: {0}", new object[] { str });
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                    if (serializationStream != null)
                    {
                        serializationStream.Close();
                        serializationStream = null;
                    }
                }
                finally
                {
                    if (serializationStream != null)
                    {
                        serializationStream.Close();
                    }
                }
            }
        }

        internal static void OnEmotesChanged()
        {
            if (EmotesChanged != null)
            {
                EmotesChanged(null, EventArgs.Empty);
            }
        }

        public void OnRecieve(Node sender)
        {
            this.mFileName = DownloadsEmoteFile;
            if (!AllEmotes.ContainsKey(this.CharSequence))
            {
                FileStream serializationStream = null;
                try
                {
                    List<Emote> graph = new List<Emote>();
                    if (!File.Exists(DownloadsEmoteFile))
                    {
                        serializationStream = CreateEmptyEmoteFile(DownloadsEmoteFile);
                    }
                    else
                    {
                        serializationStream = File.OpenRead(DownloadsEmoteFile);
                        try
                        {
                            graph = new BinaryFormatter().Deserialize(serializationStream) as List<Emote>;
                        }
                        catch
                        {
                            ErrorLog.WriteLine("Unable to deserialize file in emote directory: {0}", new object[] { DownloadsEmoteFile });
                            serializationStream.Close();
                            serializationStream = CreateEmptyEmoteFile(DownloadsEmoteFile);
                            graph = new BinaryFormatter().Deserialize(serializationStream) as List<Emote>;
                        }
                    }
                    serializationStream.Close();
                    graph.Add(this);
                    serializationStream = File.OpenWrite(DownloadsEmoteFile);
                    new BinaryFormatter().Serialize(serializationStream, graph);
                    serializationStream.Close();
                    serializationStream = null;
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
                        serializationStream = null;
                    }
                    AllEmotes.Add(this.CharSequence, this);
                    if (EmotesChanged != null)
                    {
                        EmotesChanged(this, EventArgs.Empty);
                    }
                }
            }
            else
            {
                ErrorLog.WriteLine("An emote with char sequence {0} already exists in memory", new object[] { this.CharSequence });
            }
        }

        public void PlaySound()
        {
            if ((this.SoundData != null) && (this.SoundData.Length > 0))
            {
                Sound.Play(this.SoundData);
            }
        }

        public override string ToString()
        {
            return this.CharSequence;
        }

        internal static void ValidateEmotes()
        {
            string str = ConfigSettings.GetString("RestrictedEmotes", null);
            if (str != null)
            {
                string[] array = str.Split(new char[] { ',' });
                if (array.Length > 0)
                {
                    Emote[] emoteArray = new Emote[AllEmotes.Values.Count];
                    AllEmotes.Values.CopyTo(emoteArray, 0);
                    foreach (Emote emote in emoteArray)
                    {
                        if (Array.IndexOf<string>(array, emote.CharSequence) >= 0)
                        {
                            emote.Delete();
                            EventLog.WriteLine("Deleted bad emote '{0}'.", new object[] { emote.CharSequence });
                        }
                    }
                    emoteArray = null;
                }
            }
        }

        public static Dictionary<string, Emote> AllEmotes
        {
            get
            {
                return mAllEmotes;
            }
        }

        internal bool Animated
        {
            get
            {
                return this.mAnimated;
            }
            set
            {
                this.mAnimated = value;
            }
        }

        internal int AnimationFrames
        {
            get
            {
                return this.mAnimationFrames;
            }
            set
            {
                this.mAnimationFrames = value;
            }
        }

        internal bool CanAnimate
        {
            get
            {
                return this.mCanAnimate;
            }
            set
            {
                this.mCanAnimate = value;
            }
        }

        public string CharSequence
        {
            get
            {
                if (this.mCharSequence.IndexOf(":") != 0)
                {
                    this.mCharSequence = ":" + this.mCharSequence;
                }
                return this.mCharSequence;
            }
        }

        public static string DefaultEmoteFile
        {
            get
            {
                return string.Format(@"{0}\default.{1}", EmoteDirectory.TrimEnd(@"\".ToCharArray()), "gpg");
            }
        }

        public static string DownloadsEmoteFile
        {
            get
            {
                return string.Format(@"{0}\downloads.{1}", EmoteDirectory.TrimEnd(@"\".ToCharArray()), "gpg");
            }
        }

        public static string EmoteDirectory
        {
            get
            {
                return (AppDomain.CurrentDomain.BaseDirectory + @"emotes\");
            }
        }

        public string FileName
        {
            get
            {
                return this.mFileName;
            }
        }

        public System.Drawing.Image Image
        {
            get
            {
                System.Drawing.Image image = FormatImage(this.mImage);
                if (this.mImage != image)
                {
                    this.mImage = image;
                }
                return this.mImage;
            }
        }

        public byte[] SoundData
        {
            get
            {
                return this.mSoundData;
            }
        }
    }
}

