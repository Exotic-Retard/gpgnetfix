namespace GPG.UI
{
    using GPG.Logging;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.Statistics;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class TextLine : IText, IComparable
    {
        private ITextEffect mEffect;
        private Dictionary<string, bool> mFilters = new Dictionary<string, bool>();
        private Image mIcon = GPGResources.transparent;
        private int mImageID;
        private static Hashtable mImages = Hashtable.Synchronized(new Hashtable());
        private IFilterControl mParent;
        private object mTag;
        private Color mTextColor;
        private Font mTextFont;
        private List<TextSegment> mTextSegments = new List<TextSegment>();

        private static  event EventHandler OnImageUpdate;

        public TextLine(IFilterControl parent)
        {
            this.mParent = parent;
        }

        public void AddSegment(TextSegment segment)
        {
            segment.Parent = this;
            this.TextSegments.Add(segment);
        }

        public int CompareTo(object obj)
        {
            if (obj is TextLine)
            {
                return this.Text.CompareTo((obj as TextLine).Text);
            }
            if (obj is IComparable)
            {
                return (obj as IComparable).CompareTo(this);
            }
            return 0;
        }

        public override bool Equals(object obj)
        {
            if (obj is TextLine)
            {
                try
                {
                    return (obj.GetHashCode() == this.GetHashCode());
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                    return false;
                }
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            if (this.KeyText != null)
            {
                return this.KeyText.GetHashCode();
            }
            return IntPtr.Zero.ToInt32();
        }

        private void SetImage(int id)
        {
            WaitCallback callBack = null;
            this.mImageID = id;
            if (mImages.Contains(id))
            {
                if (mImages[id] == null)
                {
                    OnImageUpdate = (EventHandler) Delegate.Combine(OnImageUpdate, new EventHandler(this.TextLine_OnImageUpdate));
                }
                else
                {
                    this.Icon = mImages[id] as Bitmap;
                }
            }
            else
            {
                mImages[id] = null;
                if (callBack == null)
                {
                    callBack = delegate (object objid) {
                        try
                        {
                            Bitmap transparent;
                            WebClient client = new WebClient();
                            string address = ConfigSettings.GetString("ImagePath", "http://gpgnet.gaspowered.com/chatimages/") + objid.ToString();
                            try
                            {
                                MemoryStream stream = new MemoryStream(client.DownloadData(address));
                                transparent = Image.FromStream(stream) as Bitmap;
                                this.Icon = transparent;
                            }
                            catch (Exception)
                            {
                                transparent = GPGResources.transparent;
                            }
                            mImages[objid.ToString()] = transparent;
                            if (OnImageUpdate != null)
                            {
                                OnImageUpdate(this, EventArgs.Empty);
                            }
                        }
                        catch (Exception exception)
                        {
                            ErrorLog.WriteLine(exception);
                        }
                    };
                }
                ThreadPool.QueueUserWorkItem(callBack, id);
            }
        }

        private void TextLine_OnImageUpdate(object sender, EventArgs e)
        {
            if (mImages.Contains(this.mImageID))
            {
                this.Icon = mImages[this.mImageID] as Bitmap;
                OnImageUpdate = (EventHandler) Delegate.Remove(OnImageUpdate, new EventHandler(this.TextLine_OnImageUpdate));
            }
            else
            {
                this.Icon = Avatar.Default.Image;
            }
        }

        public override string ToString()
        {
            return this.KeyText;
        }

        public ITextEffect Effect
        {
            get
            {
                return this.mEffect;
            }
            set
            {
                this.mEffect = value;
            }
        }

        public Dictionary<string, bool> Filters
        {
            get
            {
                return this.mFilters;
            }
        }

        public Image Icon
        {
            get
            {
                return this.mIcon;
            }
            set
            {
                if (value != null)
                {
                    this.mIcon = value;
                }
            }
        }

        public bool IsVisible
        {
            get
            {
                if (!this.Parent.IsFiltered)
                {
                    return true;
                }
                bool flag = true;
                foreach (KeyValuePair<string, bool> pair in this.Parent.Filters)
                {
                    bool flag2 = pair.Value;
                    bool flag3 = !this.Filters.ContainsKey(pair.Key) ? false : this.Filters[pair.Key];
                    if (flag2)
                    {
                        if (this.Filters.ContainsKey(pair.Key) && flag3)
                        {
                            return true;
                        }
                        flag = false;
                        continue;
                    }
                    if (this.Filters.ContainsKey(pair.Key) && flag3)
                    {
                        flag = false;
                    }
                }
                return flag;
            }
        }

        public string KeyText
        {
            get
            {
                for (int i = 0; i < this.TextSegments.Count; i++)
                {
                    if (this.TextSegments[i].IsKeySegment)
                    {
                        return this.TextSegments[i].Text;
                    }
                }
                return this.Text;
            }
        }

        public IFilterControl Parent
        {
            get
            {
                return this.mParent;
            }
        }

        public object Tag
        {
            get
            {
                return this.mTag;
            }
            set
            {
                try
                {
                    this.mTag = value;
                    if (ConfigSettings.GetBool("ShowChatImage", true) && (this.mTag is User))
                    {
                        WaitCallback callBack = null;
                        User user = this.mTag as User;
                        if (user != null)
                        {
                            if (user.IsSystem)
                            {
                                this.Icon = Avatar.GPGIcon;
                            }
                            else if (mImages.ContainsKey(user.Avatar))
                            {
                                this.mImageID = user.Avatar;
                                this.Icon = mImages[user.Avatar] as Bitmap;
                            }
                            else
                            {
                                OnImageUpdate = (EventHandler) Delegate.Combine(OnImageUpdate, new EventHandler(this.TextLine_OnImageUpdate));
                                if (callBack == null)
                                {
                                    callBack = delegate (object o) {
                                        try
                                        {
                                            if (Avatar.AllAvatars.ContainsKey(user.Avatar))
                                            {
                                                Bitmap image = Avatar.AllAvatars[user.Avatar].Image;
                                                mImages[user.Avatar] = image;
                                                this.mImageID = user.Avatar;
                                                OnImageUpdate(this, EventArgs.Empty);
                                            }
                                            else
                                            {
                                                OnImageUpdate(this, EventArgs.Empty);
                                            }
                                        }
                                        catch (Exception exception)
                                        {
                                            ErrorLog.WriteLine(exception);
                                        }
                                    };
                                }
                                ThreadPool.QueueUserWorkItem(callBack);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        public virtual string Text
        {
            get
            {
                string text = "";
                foreach (TextSegment segment in this.TextSegments)
                {
                    text = text + segment.Text;
                }
                if (this.mEffect != null)
                {
                    text = this.mEffect.ChangeText(text);
                }
                return text;
            }
            set
            {
                this.TextSegments.Clear();
                this.AddSegment(new TextSegment(value, true));
            }
        }

        public Color TextColor
        {
            get
            {
                if (this.mEffect != null)
                {
                    return this.mEffect.ChangeColor(this.mTextColor);
                }
                return this.mTextColor;
            }
            set
            {
                this.mTextColor = value;
            }
        }

        public Font TextFont
        {
            get
            {
                if (this.mEffect != null)
                {
                    return this.mEffect.ChangeFont(this.mTextFont);
                }
                return this.mTextFont;
            }
            set
            {
                this.mTextFont = value;
            }
        }

        public List<TextSegment> TextSegments
        {
            get
            {
                return this.mTextSegments;
            }
        }
    }
}

