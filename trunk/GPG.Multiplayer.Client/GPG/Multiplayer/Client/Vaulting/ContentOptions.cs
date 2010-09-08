namespace GPG.Multiplayer.Client.Vaulting
{
    using System;
    using System.Windows.Forms;

    public class ContentOptions
    {
        private bool mHasOptions;
        private string mName;
        private Control mOptionsControl;

        private ContentOptions()
        {
            this.mHasOptions = false;
        }

        public ContentOptions(string name, Control optionsControl)
        {
            this.mName = name;
            this.mOptionsControl = optionsControl;
            this.mHasOptions = true;
        }

        public bool HasOptions
        {
            get
            {
                return this.mHasOptions;
            }
        }

        public string Name
        {
            get
            {
                return this.mName;
            }
        }

        public static ContentOptions None
        {
            get
            {
                return new ContentOptions();
            }
        }

        public Control OptionsControl
        {
            get
            {
                return this.mOptionsControl;
            }
        }
    }
}

